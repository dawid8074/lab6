using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Image Wczytane;
        Bitmap BitmapWczytane;
        int WczytaneSzerokosc, WczytaneWysokosc;

        public Form1()
        {
            InitializeComponent();
            trackBar1.Value = 0;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open = new OpenFileDialog();

            if (Open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = Open.FileName;
                try
                {
                    pictureBox1.Load();
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

                    Wczytane = pictureBox1.Image;
                    //Wczytane = resizeImage(Wczytane, new Size(pictureBox1.Size.Width, pictureBox1.Size.Height));
                    BitmapWczytane = new Bitmap(Wczytane);
                    WczytaneSzerokosc = BitmapWczytane.Width;
                    WczytaneWysokosc = BitmapWczytane.Height;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }


       

        
        
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        
        /*
        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            tukan = pictureBox1.Image;
            tukan = resizeImage(tukan, new Size(pictureBox1.Size.Width, pictureBox1.Size.Height));
            bitmapTukan = new Bitmap(tukan);
            tukanWidth = bitmapTukan.Width;
            tukanHeight = bitmapTukan.Height;
            

        }
    */
        public int checkIfInRGB(int value)
        {
            if (value >= 255) return 254;
            if (value <= 0) return 1;
            else return value;
        }
        private int zmniejszKontrast(int color, int delta)
        {
            if (color < 127 + delta) return (127 / (127 + delta)) * color;
            else if (color > 127 - delta) return ((127 * color) + (255 * delta)) / (127 + delta);
            else return 127;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap temp = new Bitmap(Wczytane);
                if (trackBar1.Value >= 0)
                {
                    for (int y = 0; y < WczytaneWysokosc; y++)
                    {
                        for (int x = 0; x < WczytaneSzerokosc; x++)
                        {
                            Color Wczytany_Pixel = BitmapWczytane.GetPixel(x, y);

                            int a = Wczytany_Pixel.A;
                            int r = checkIfInRGB((127 / checkIfInRGB(127 - trackBar1.Value)) * (Wczytany_Pixel.R - trackBar1.Value));
                            int g = checkIfInRGB((127 / checkIfInRGB(127 - trackBar1.Value)) * (Wczytany_Pixel.G - trackBar1.Value));
                            int b = checkIfInRGB((127 / checkIfInRGB(127 - trackBar1.Value)) * (Wczytany_Pixel.B - trackBar1.Value));
                            temp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                        }
                    }
                    wypelnijHistrogram(temp);
                    pictureBox2.Image = temp;
                }
                else
                {
                    for (int y = 0; y < WczytaneWysokosc; y++)
                    {
                        for (int x = 0; x < WczytaneSzerokosc; x++)
                        {
                            Color Wczytany_Pixel = BitmapWczytane.GetPixel(x, y);

                            int a = Wczytany_Pixel.A;
                            int r = zmniejszKontrast(Wczytany_Pixel.R, trackBar1.Value);
                            int g = zmniejszKontrast(Wczytany_Pixel.G, trackBar1.Value);
                            int b = zmniejszKontrast(Wczytany_Pixel.B, trackBar1.Value);
                            temp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                        }
                    }
                    wypelnijHistrogram(temp);
                    pictureBox2.Image = temp;
                }
            }
        }

        private void wypelnijHistrogram(Bitmap temp)
        {
            int[] czerw = new int[256];
            int[] ziel = new int[256];
            int[] nieb = new int[256];
            for (int x = 0; x < WczytaneSzerokosc; x++)
            {
                for (int y = 0; y < WczytaneWysokosc; y++)
                {
                    Color pixel = temp.GetPixel(x, y);
                    czerw[pixel.R]++;
                    ziel[pixel.G]++;
                    nieb[pixel.B]++;
                }
            }

            //Wyswietl histogram na wykresie
            chart1.Series["red"].Points.Clear();
            chart1.Series["green"].Points.Clear();
            chart1.Series["blue"].Points.Clear();
            for (int i = 0; i < 256; i++)
            {
                chart1.Series["red"].Points.AddXY(i, czerw[i]);
                chart1.Series["green"].Points.AddXY(i, ziel[i]);
                chart1.Series["blue"].Points.AddXY(i, nieb[i]);
            }
            chart1.Invalidate();
        }

        



    }
}
