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

        // Sirinata na formata ne smej da bide pomala 120 pikseli
        // t.e SIRINA*Zmija.StranaKvadrat >= 120

        // Visina na formata ne smej na bide pomala od 120 pikseli
        // t.e VISINA*Zmija.StranaKvadrat >= 120
        public const int SIRINA = 30; // kolku kvadratcina po sirina ima stazata
        public const int VISINA = 20; // kolku kvadratcina po visina ima stazata

        public Form1()
        {
            InitializeComponent();
            NovaIgra();
        }

        public void NovaIgra()
        {
            zmija = new Zmija();
            timer1.Start();
            toolStripStatusLabel1.Text = "Поени " + zmija.Poeni.ToString();
            timer1.Interval = zmija.Brzina;

            // podesuvanje na dimenziite na stazata
            this.Width = this.Width - pictureBox1.Width + zmija.StranaKvadrat * SIRINA + 1;
            this.Height = this.Height - pictureBox1.Height + zmija.StranaKvadrat * VISINA + statusStrip1.Height + 1;
            this.MinimumSize = new Size(this.Width, this.Height);
            this.MaximumSize = new Size(this.Width, this.Height);
            pictureBox1.Size = new Size(zmija.StranaKvadrat * SIRINA, zmija.StranaKvadrat * VISINA);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            zmija.Premesti(SIRINA, VISINA);
            toolStripStatusLabel1.Text = "Поени " + zmija.Poeni;
            //Invalidate(true);

            if (!zmija.KrajNaIgra)
            {
                Bitmap BitMapa = new Bitmap(zmija.StranaKvadrat * SIRINA + 1, zmija.StranaKvadrat * VISINA + 1);
                using (Graphics g = Graphics.FromImage(BitMapa))
                {
                    zmija.Crtanje(g, SIRINA, VISINA);
                    pictureBox1.CreateGraphics().DrawImageUnscaled(BitMapa, 0, 0);
                }
            }

            if (zmija.SamoUnistuvanje() || zmija.KrajNaIgra)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            // ova e gotov kod od Predavanje 09 - WindowsForms3
            // od profesorot Dejan potreben e za BitMapata
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //int a;
            /*Bitmap BitMapa = new Bitmap(zmija.StranaKvadrat * SIRINA + 1, zmija.StranaKvadrat * VISINA + 1);
            using (Graphics g = Graphics.FromImage(BitMapa))
            {
                zmija.Crtanje(g, SIRINA, VISINA);
                pictureBox1.CreateGraphics().DrawImageUnscaled(BitMapa, 0, 0);
            }*/
        }
    }
}