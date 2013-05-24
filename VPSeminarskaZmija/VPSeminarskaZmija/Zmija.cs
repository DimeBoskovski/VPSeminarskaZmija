using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace VPSeminarskaZmija
{
    public enum NASOKA { LEVO, DESNO, GORE, DOLU };
    
    public class Zmija
    {
        public int Poeni { get; set; }
        public int Brzina { get; set; }
        public int StranaKvadrat { get; set; }
        public NASOKA Nasoka { get; set; }
        public SolidBrush Cetka { get; set; }
        public Pen Penkalo { get; set; }
        public LinkedList<Point> TeloZmija { get; set; }
        public Random PomosenRandom { get; set; }
        public Point Hrana { get; set; }
        public float CetStr { get; set; }
        public float PolStr { get; set; }
        public float Dijagonala { get; set; }
        public bool KrajNaIgra { get; set; }

        public Zmija()
        {
            Poeni = 0;
            Brzina = 100; // vo mili sekundi
            StranaKvadrat = 20; // dimenzija na kvadratotot od koj e sostavena zmijata
            Nasoka = NASOKA.DOLU; // pocetna nasoka levo
            Cetka = new SolidBrush(Color.Green);
            Penkalo = new Pen(Color.Black);
            PomosenRandom = new Random(); // potreben objekt

            TeloZmija = new LinkedList<Point>(); // pocetna inicijalizacija na zmijata
            TeloZmija.AddLast(new Point(10, 10)); // so odredena dolzina minimum 2
            TeloZmija.AddLast(new Point(10, 9));
            TeloZmija.AddLast(new Point(10, 8));
            TeloZmija.AddLast(new Point(10, 7));

            Hrana = NovaHrana(1, 1); // ovaj povik mora da e posle inicijalizacija na TeloZmija

            // za da ne gi presmetuvam postojano
            // gi cuvam kako klasni promenlivi
            CetStr = StranaKvadrat / 4;
            PolStr = StranaKvadrat / 2;
            Dijagonala = PolStr * 1.4142f;

            //
            KrajNaIgra = false; // promenliva koja sluzi za kraj na igra dokolku se izlezi nadvor od stazata
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
                    if (pomosna.X + 1 >= sirina)
                        //TeloZmija.AddFirst(new Point(0, pomosna.Y));
                        KrajNaIgra = true;
                    else
                        TeloZmija.AddFirst(new Point(pomosna.X + 1, pomosna.Y));
                    break;
                case NASOKA.LEVO:
                    if (pomosna.X-1 < 0)
                        //TeloZmija.AddFirst(new Point(sirina-1, pomosna.Y));
                        KrajNaIgra = true;
                    else
                        TeloZmija.AddFirst(new Point(pomosna.X-1, pomosna.Y));
                    break;
                case NASOKA.DOLU:
                    if (pomosna.Y+1 >= visina)
                        //TeloZmija.AddFirst(new Point(pomosna.X, 0));
                        KrajNaIgra = true;
                    else
                        TeloZmija.AddFirst(new Point(pomosna.X, pomosna.Y+1));
                    break;
                case NASOKA.GORE:
                    if (pomosna.Y-1 < 0)
                        //TeloZmija.AddFirst(new Point(pomosna.X, visina-1));
                        KrajNaIgra = true;
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
            }
            else
            {
                TeloZmija.RemoveLast();
            }

            if (TeloZmija.Last.Value.X == -1 || TeloZmija.Last.Value.Y == -1)
                TeloZmija.RemoveLast();
        }

        public void Crtanje(Graphics g, int sirina, int visina)
        {
            g.Clear(Color.White);

            for (int i = 1; i < TeloZmija.Count-1; i++)
            {
                bool pomosna = false;
                if (TeloZmija.ElementAt(i - 1).X == TeloZmija.ElementAt(i + 1).X)
                {
                    g.FillEllipse(Cetka, TeloZmija.ElementAt(i).X * StranaKvadrat + CetStr,
                    TeloZmija.ElementAt(i).Y * StranaKvadrat, PolStr, StranaKvadrat);
                }
                else if (TeloZmija.ElementAt(i - 1).Y == TeloZmija.ElementAt(i + 1).Y)
                {
                    g.FillEllipse(Cetka, TeloZmija.ElementAt(i).X * StranaKvadrat,
                    TeloZmija.ElementAt(i).Y * StranaKvadrat + CetStr, StranaKvadrat, PolStr);
                }
                else
                {
                    g.TranslateTransform(PolStr + TeloZmija.ElementAt(i).X * StranaKvadrat, StranaKvadrat * TeloZmija.ElementAt(i).Y);

                    if (TeloZmija.ElementAt(i - 1).X > TeloZmija.ElementAt(i + 1).X &&
                        TeloZmija.ElementAt(i - 1).Y > TeloZmija.ElementAt(i + 1).Y ||
                        TeloZmija.ElementAt(i + 1).X > TeloZmija.ElementAt(i - 1).X &&
                        TeloZmija.ElementAt(i + 1).Y > TeloZmija.ElementAt(i - 1).Y)
                    {
                        g.RotateTransform(-45);
                        if (TeloZmija.ElementAt(i + 1).X == TeloZmija.ElementAt(i).X && TeloZmija.ElementAt(i + 1).Y < TeloZmija.ElementAt(i).Y
                            || TeloZmija.ElementAt(i - 1).X == TeloZmija.ElementAt(i).X && TeloZmija.ElementAt(i - 1).Y < TeloZmija.ElementAt(i).Y
                            )
                        {
                            // gore desno
                            g.FillEllipse(Cetka, -CetStr, 0, PolStr, Dijagonala);
                        }
                        else
                        {
                            // dolu levo
                            g.FillEllipse(Cetka, -(StranaKvadrat), 0, PolStr, Dijagonala);
                        }
                        g.RotateTransform(45);
                    }
                    else
                    {
                        g.RotateTransform(+45);
                        if (TeloZmija.ElementAt(i + 1).X == TeloZmija.ElementAt(i).X && TeloZmija.ElementAt(i + 1).Y < TeloZmija.ElementAt(i).Y
                            || TeloZmija.ElementAt(i - 1).X == TeloZmija.ElementAt(i).X && TeloZmija.ElementAt(i - 1).Y < TeloZmija.ElementAt(i).Y)
                        {
                            // gore levo
                            g.FillEllipse(Cetka, -CetStr, 0, PolStr, Dijagonala);
                        }
                        else
                        {
                            // dolu desno
                            g.FillEllipse(Cetka, CetStr + CetStr, 0, PolStr, Dijagonala);
                        }
                        g.RotateTransform(-45);
                    }
                    /*if (pomosna)
                        g.RotateTransform(-45);*/

                    g.TranslateTransform(-PolStr - TeloZmija.ElementAt(i).X * StranaKvadrat, -StranaKvadrat * TeloZmija.ElementAt(i).Y);
                }
            }


            // iscrtuvanje na zmijata
            /*foreach (Point item in TeloZmija)
            {
                g.FillRectangle(Cetka, item.X * StranaKvadrat, item.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
                g.DrawRectangle(Penkalo, item.X * StranaKvadrat, item.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
            }*/

            // iscrtuvanje na glavata
            //SolidBrush pom = new SolidBrush(Color.Black);
            //Point pomosnaTocka = TeloZmija.First();
            //g.FillRectangle(pom, pomosnaTocka.X * StranaKvadrat, pomosnaTocka.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
            //g.DrawRectangle(Penkalo, pomosnaTocka.X * StranaKvadrat, pomosnaTocka.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
            if (TeloZmija.ElementAt(0).Y == TeloZmija.ElementAt(1).Y)
            {
                if (Nasoka == NASOKA.LEVO)
                    g.FillPie(Cetka, TeloZmija.ElementAt(0).X * StranaKvadrat, TeloZmija.ElementAt(0).Y * StranaKvadrat + CetStr, StranaKvadrat, PolStr, 225, 270);
                else
                    g.FillPie(Cetka, TeloZmija.ElementAt(0).X * StranaKvadrat, TeloZmija.ElementAt(0).Y * StranaKvadrat + CetStr, StranaKvadrat, PolStr, 45, 270);
            }
            else
            {
                if (Nasoka == NASOKA.GORE)
                    g.FillPie(Cetka, TeloZmija.ElementAt(0).X * StranaKvadrat + CetStr, TeloZmija.ElementAt(0).Y * StranaKvadrat, PolStr, StranaKvadrat, 315, 270);
                else
                    g.FillPie(Cetka, TeloZmija.ElementAt(0).X * StranaKvadrat + CetStr, TeloZmija.ElementAt(0).Y * StranaKvadrat, PolStr, StranaKvadrat, 135, 270);
                //g.FillEllipse(Cetka, TeloZmija.ElementAt(0).X * StranaKvadrat + CetStr, TeloZmija.ElementAt(0).Y * StranaKvadrat, PolStr, StranaKvadrat);
            }

            // opaska
            int size = TeloZmija.Count;
            if (TeloZmija.ElementAt(size-1).Y == TeloZmija.ElementAt(size-2).Y)
                g.FillEllipse(Cetka, TeloZmija.ElementAt(size-1).X * StranaKvadrat, TeloZmija.ElementAt(size-1).Y * StranaKvadrat + CetStr, StranaKvadrat, PolStr);
            else
                g.FillEllipse(Cetka, TeloZmija.ElementAt(size-1).X * StranaKvadrat + CetStr, TeloZmija.ElementAt(size-1).Y * StranaKvadrat, PolStr, StranaKvadrat);
            

            // iscrtuvanje na hranata
            g.FillRectangle(Cetka, Hrana.X * StranaKvadrat, Hrana.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
            g.DrawRectangle(Penkalo, Hrana.X * StranaKvadrat, Hrana.Y * StranaKvadrat, StranaKvadrat, StranaKvadrat);
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
