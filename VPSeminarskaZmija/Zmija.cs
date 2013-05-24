using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace VPSeminarskaZmija
{
    public enum NASOKA { LEVO, DESNO, GORE, DOLU };
    
    public class Zmija
    {
        // proba
        public int Poeni { get; set; }
        public int Brzina { get; set; }
        public int StranaKvadrat { get; set; }
        public NASOKA Nasoka { get; set; }
        public SolidBrush Cetka { get; set; }
        public Pen Penkalo { get; set; }
        public LinkedList<Point> TeloZmija { get; set; }
        public Random PomosenRandom { get; set; }
        public Point Hrana { get; set; }
        public Point BonusHrana { get; set; }
        public bool Znamence { get; set; }

        
        public Zmija(int sirina, int visina)
        {
            Poeni = 0;
            Brzina = 100; // vo mili sekundi
            StranaKvadrat = 10; // dimenzija na kvadratotot od koj e sostavena zmijata
            Nasoka = NASOKA.DOLU; // pocetna nasoka levo
            Cetka = new SolidBrush(Color.Green);
            Penkalo = new Pen(Color.Black);
            PomosenRandom = new Random(); // potreben objekt

            TeloZmija = new LinkedList<Point>(); // pocetna inicijalizacija na zmijata
            TeloZmija.AddLast(new Point(10, 10)); // so odredena dolzina minimum 2
            TeloZmija.AddLast(new Point(10, 9));
            //TeloZmija.AddLast(new Point(10, 8));
            //TeloZmija.AddLast(new Point(10, 7));
            // komentar

            Hrana = NovaHrana(1, 1); // ovaj povik mora da e posle inicijalizacija na TeloZmija
            Znamence = false;
            BonusHrana = new Point(sirina, visina);
        }

        // funkcija koja ni sluzi za generiranje na nova hrana
        // implementirana e rekurzivno
        public Point NovaHrana(int sirina, int visina)
        {
            Point pomosna = new Point(PomosenRandom.Next(0, sirina-1), PomosenRandom.Next(0, visina-1));
            
            foreach (Point i in TeloZmija)
            {
                if (pomosna.X == i.X && pomosna.Y == i.Y)
                {
                    pomosna = NovaHrana(sirina, visina);
                    break;
                }
            }

            return pomosna;
        }

        public Point NovaBonusHrana(int sirina, int visina)
        {
            Point pomosna = new Point(PomosenRandom.Next(0, sirina - 1), PomosenRandom.Next(0, visina - 1));

            foreach (Point i in TeloZmija)
            {
                if (pomosna.X == i.X && pomosna.Y == i.Y)
                {
                    pomosna = NovaBonusHrana(sirina, visina);
                    break;
                }
            }

            if (Hrana.X == pomosna.X && Hrana.Y == pomosna.Y)
            {
                pomosna = NovaBonusHrana(sirina, visina);
            }

            return pomosna;
        }

        // Menuvanje na nasokata na zmijata
        // Vrsi proverka za da ne dozvoli zmijata
        // da se dvizi nanazad
        public void PromeniNasoka(NASOKA nova)
        {
            Point Glava = TeloZmija.ElementAt(0);
            Point DoGlava = TeloZmija.ElementAt(1);

            if ((Glava.X == DoGlava.X && (nova == NASOKA.GORE || nova == NASOKA.DOLU)) ||
                (Glava.Y == DoGlava.Y && (nova == NASOKA.LEVO || nova == NASOKA.DESNO)))
                return;
            
            Nasoka = nova;
        }

        public void Premesti(int sirina, int visina)
        {
            Point pomosna = TeloZmija.First();
            
            // presmetaj ja slednata pozicija na glavata od zmijata
            // vo zavisnost od Nasoka
            switch (Nasoka)
            {
                case NASOKA.DESNO:
                    if (pomosna.X+1 >= sirina)
                        TeloZmija.AddFirst(new Point(0, pomosna.Y));
                    else
                        TeloZmija.AddFirst(new Point(pomosna.X+1, pomosna.Y));
                    break;
                case NASOKA.LEVO:
                    if (pomosna.X-1 < 0)
                        TeloZmija.AddFirst(new Point(sirina-1, pomosna.Y));
                    else
                        TeloZmija.AddFirst(new Point(pomosna.X-1, pomosna.Y));
                    break;
                case NASOKA.DOLU:
                    if (pomosna.Y+1 >= visina)
                        TeloZmija.AddFirst(new Point(pomosna.X, 0));
                    else
                        TeloZmija.AddFirst(new Point(pomosna.X, pomosna.Y+1));
                    break;
                case NASOKA.GORE:
                    if (pomosna.Y-1 < 0)
                        TeloZmija.AddFirst(new Point(pomosna.X, visina-1));
                    else
                        TeloZmija.AddFirst(new Point(pomosna.X, pomosna.Y-1));
                    break;
            }

            // Ako starata hrana e izedena presmetaj pozicija na nova
            // i ne ja otstranuvaj opaskata.
            // Dokolku starata hrana ne e izedena otstrani ja opaskata
            pomosna = TeloZmija.First();
            if (pomosna.X == Hrana.X && pomosna.Y == Hrana.Y)
            {
                Hrana = NovaHrana(sirina, visina);
                Poeni += 3;
                if (TeloZmija.Count % 5 == 0)
                {
                    Znamence = true;
                    BonusHrana = NovaBonusHrana(sirina, visina);
                }
            }
            else
            {
                TeloZmija.RemoveLast();
            }

            if (pomosna.X == BonusHrana.X && pomosna.Y == BonusHrana.Y)
            {
                //Poeni += 10;
                Znamence = false;
            }
        }

        public void Crtanje(Graphics g)
        {
            g.Clear(Color.White);

            // iscrtuvanje na zmijata
            foreach (Point item in TeloZmija)
            {
                g.FillRectangle(Cetka, item.X * StranaKvadrat, item.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
                g.DrawRectangle(Penkalo, item.X * StranaKvadrat, item.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
            }

            // iscrtuvanje na glavata
            SolidBrush pom = new SolidBrush(Color.Black);
            Point pomosnaTocka = TeloZmija.First();
            g.FillRectangle(pom, pomosnaTocka.X * StranaKvadrat, pomosnaTocka.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
            //g.DrawRectangle(Penkalo, pomosnaTocka.X * StranaKvadrat, pomosnaTocka.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);

            // iscrtuvanje na hranata
            g.FillRectangle(Cetka, Hrana.X * StranaKvadrat, Hrana.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
            g.DrawRectangle(Penkalo, Hrana.X * StranaKvadrat, Hrana.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);

            if (Znamence)
            {
                SolidBrush BonusCetka = new SolidBrush(Color.Red);
                g.FillRectangle(BonusCetka, BonusHrana.X * StranaKvadrat, BonusHrana.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
                g.DrawRectangle(Penkalo, BonusHrana.X * StranaKvadrat, BonusHrana.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
            }
        }

        // Dokolku zmijata sama se izedi sebesi ke vrati true
        // vo sprotiven slucaj false
        public bool SamoUnistuvanje()
        {
            Point pomosna = TeloZmija.First();
            for (int i = 1; i < TeloZmija.Count; i++)
            {
                if (TeloZmija.ElementAt(i).X == pomosna.X && TeloZmija.ElementAt(i).Y == pomosna.Y)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
