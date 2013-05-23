using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VPSeminarskaZmija
{
    public partial class Form1 : Form
    {
        public Zmija zmija;
        public int bonusVreme;
        //public Rectangle panel1; PROBAAAAAAAAAAAAAAAA

        // Sirinata na formata ne smej da bide pomala 120 pikseli
        // t.e SIRINA*Zmija.StranaKvadrat >= 120

        // Visina na formata ne smej na bide pomala od 120 pikseli
        // t.e VISINA*Zmija.StranaKvadrat >= 120
        public const int SIRINA = 30; // kolku kvadratcina po sirina ima stazata
        public const int VISINA = 30; // kolku kvadratcina po visina ima stazata

        public Form1()
        {
            InitializeComponent();
            bonusVreme = 10;
            NovaIgra();
            DoubleBuffered = true;
            //panel1 = new Rectangle(0, 0, this.Width, this.Height);
        }

        public void NovaIgra()
        {
            zmija = new Zmija(this.Width, this.Height);
            timer1.Start();
            toolStripStatusLabel1.Text = "Поени " + zmija.Poeni.ToString();
            timer1.Interval = zmija.Brzina;

            // podesuvanje na dimenziite na stazata
            //this.Width = this.Width - panel1.Width + zmija.StranaKvadrat * SIRINA + 1;
            //this.Height = this.Height - panel1.Height + zmija.StranaKvadrat * VISINA + statusStrip1.Height + 1;
            this.MinimumSize = new Size(this.Width, this.Height);
            this.MaximumSize = new Size(this.Width, this.Height);
            //panel1.Size = new Size(zmija.StranaKvadrat * SIRINA, zmija.StranaKvadrat * VISINA);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            zmija.Premesti(SIRINA, VISINA);
            toolStripStatusLabel1.Text = "Поени " + zmija.Poeni;
            Invalidate(true);

            if (zmija.SamoUnistuvanje())
            {
                timer1.Enabled = false;
                MessageBox.Show("Резултат: " + zmija.Poeni.ToString() + " поени.");
                DialogResult rez =  MessageBox.Show("Дали сакате нова игра?", "Нова игра", MessageBoxButtons.YesNo);
                if (rez == DialogResult.Yes)
                {
                    NovaIgra();
                }
                else
                {
                    this.Close();
                }
            }

            if (zmija.Znamence)
            {
                timer2.Start();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    zmija.PromeniNasoka(NASOKA.GORE);
                    break;
                case Keys.Down:
                    zmija.PromeniNasoka(NASOKA.DOLU);
                    break;
                case Keys.Left:
                    zmija.PromeniNasoka(NASOKA.LEVO);
                    break;
                case Keys.Right:
                    zmija.PromeniNasoka(NASOKA.DESNO);
                    break;
                case Keys.Space:
                    if (timer1.Enabled)
                        timer1.Enabled = false;
                    else
                        timer1.Enabled = true;
                    break;
            }
        }

        /*private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //Bitmap BitMapa = new Bitmap(panel1.Width, panel1.Height - statusStrip1.Height);
            Bitmap BitMapa = new Bitmap(zmija.StranaKvadrat * SIRINA + 1, zmija.StranaKvadrat * VISINA + 1);
            using (Graphics g = e.Graphics)
            {
                zmija.Crtanje(g);
                panel1.CreateGraphics().DrawImageUnscaled(BitMapa, 0, 0);
            }
        }*/

        private void Form1_Load(object sender, EventArgs e)
        {
            // ova e gotov kod od Predavanje 09 - WindowsForms3
            // od profesorot Dejan potreben e za BitMapata
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            bonusVreme--;
            toolStripStatusLabel2.Text = bonusVreme.ToString();
            if (bonusVreme == 0 || zmija.Znamence==false)
            {
                zmija.Poeni += 3 * bonusVreme;
                bonusVreme = 10;
                zmija.BonusHrana = new Point(this.Width, this.Height);
                timer2.Stop();
                toolStripStatusLabel2.Text = "0";
                zmija.Znamence = false;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            zmija.Crtanje(e.Graphics);
        }
    }
}