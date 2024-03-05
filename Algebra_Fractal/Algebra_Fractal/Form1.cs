using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algebra_Fractal
{
    public partial class Form1 : Form
    {
        Bitmap B = new Bitmap(1100, 1100);
        Graphics g;
        int CNT = 100, stack_size = 0;
        int D1 = 10, D2 = 15, D3 = 20;
        double[] stack = new double[200];
        double minX = -2.5, minY = -2, maxX = 1.5, maxY = 2;
        double MaxLen = 10;
        bool Clicked = false;
        Point MousePos;
        Color[] pallete = new Color[1000];

        private void Form1_Load(object sender, EventArgs e)
        {
            g = pictureBox1.CreateGraphics();
        }



        public Form1()
        {
            InitializeComponent();
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }



        private int ToInt(double x)
        {
            return int.Parse(Math.Round(x).ToString());
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Clicked = true;
            MousePos = new Point(e.X, e.Y);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Clicked = false;
            pictureBox1.Image = B;
            Add(minX);
            Add(maxX);
            Add(minY);
            Add(maxY);
            Complex a = ToPoint(MousePos);
            Complex b = ToPoint(new Point(e.X, e.Y));
            minX = Math.Min(a.R, b.R);
            maxX = Math.Max(a.R, b.R);
            minY = Math.Min(a.I, b.I);
            maxY = Math.Max(a.I, b.I);
            label5.Text = "Center:\n(" + ((minX + maxX) / 2).ToString() + ";" + ((minY + maxY) / 2).ToString() + ")";
            label6.Text = "Width: " + (maxX - minX).ToString();
            label7.Text = "Height: " + (maxY - minY).ToString();
            Draw();
        }


        void Add(double elem)
        {
            stack[stack_size] = elem;
            ++stack_size;
        }

        double Pop()
        {
            double res = stack[stack_size - 1];
            --stack_size;
            return res;
        }
        private int NextColor(int C, ref int D)
        {
            if (C + D >= 255 || C + D < 0)
                D = -D;
            C += D;
            return C;
        }
        private void Draw()
        {
            for (int x = 0; x < B.Width; ++x)
            {
                for (int y = 0; y < B.Height; ++y)
                {
                    Complex C = ToPoint(new Point(x, y));
                    Complex Z = new Complex(0, 0);
                    int C1 = 0, C2 = 0, C3 = 0;
                    for (int i = 0; i < CNT; ++i)
                    {
                        if (Z.SquareLength() > MaxLen)
                            break;
                        C1 = NextColor(C1, ref D1);
                        C2 = NextColor(C2, ref D2);
                        C3 = NextColor(C3, ref D3);
                        Z = Z * Z + C;
                    }
                    B.SetPixel(x, y, Color.FromArgb(C1, C2, C3));
                }
            }
            pictureBox1.Image = B;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (stack_size != 0)
            {
                maxY = Pop();
                minY = Pop();
                maxX = Pop();
                minX = Pop();
                Draw();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stack_size = 0;
            minX = -2.5;
            maxX = 1.5;
            minY = -2;
            maxY = 2;
            Draw();
        }

        private void Create_Pallete()
        {
            pallete[0] = Color.FromArgb(0, 0, 0);
            for (int i = 1; i < CNT; ++i)
            {
                pallete[i] = Color.FromArgb(NextColor(pallete[i - 1].R, ref D1), NextColor(pallete[i - 1].G, ref D2), NextColor(pallete[i - 1].B, ref D3));
            }
        }
        private void Draw1()
        {
            Create_Pallete();
            for (int x = 0; x < B.Width; ++x)
            {
                for (int y = 0; y < B.Height; ++y)
                {
                    Complex C = ToPoint(new Point(x, y));
                    Complex Z = new Complex(0, 0);
                    int i;
                    for (i = 0; i < CNT; ++i)
                    {
                        if (Z.SquareLength() > MaxLen)
                            break;
                        Z = Z * Z + C;
                    }
                    if (i < CNT)
                    {
                        double log_zn = Math.Log(Z.SquareLength(), 2) / 2;
                        double nu = Math.Log(log_zn / Math.Log(2, 2), 2) / Math.Log(2, 2);
                        double ind = i + 1 - nu;
                        Color color1 = pallete[ToInt(Math.Floor(ind)) % 1000];
                        Color color2 = pallete[(ToInt(Math.Floor(ind)) + 1) % 1000];
                        Color color;
                        Random r = new Random();
                        int aR = r.Next() % CNT;
                        int bR = r.Next() % CNT;
                        int aG = r.Next() % CNT;
                        int bG = r.Next() % CNT;
                        int aB = r.Next() % CNT;
                        int bB = r.Next() % CNT;
                        color = Color.FromArgb(ToInt(ToDouble(aR * color1.R + bR * color2.R) / (aR + bR)),
                            ToInt(ToDouble(aG * color1.G + bG * color2.G) / (aG + bG)),
                            ToInt(ToDouble(aB * color1.B + bB * color2.B) / (aB + bB)));
                        B.SetPixel(x, y, color);
                    }
                    else
                        B.SetPixel(x, y, Color.Black);
                }
            }
            pictureBox1.Image = B;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Draw();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CNT = int.Parse(numericUpDown1.Value.ToString());
            CNT *= 2;
            Draw();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private double ToDouble(int x)
        {
            return double.Parse(x.ToString());
        }

        private Point ToPixel(Complex a)
        {
            Point Res = new Point(ToInt((a.R - minX) / (maxX - minX) * B.Width), ToInt((a.I - minY) / (maxY - minY) * B.Height));
            Res.Y = B.Height - Res.Y;
            return Res;
        }
        private Complex ToPoint(Point d)
        {
            d.Y = B.Height - d.Y;
            Complex Res = new Complex(d.X * (maxX - minX) / B.Width + minX, d.Y * (maxY - minY) / B.Height + minY);
            return Res;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            D1 = int.Parse(numericUpDown2.Value.ToString());
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            D2 = int.Parse(numericUpDown3.Value.ToString());
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            D3 = int.Parse(numericUpDown4.Value.ToString());
        }


    }
}

public class Complex
{
    public double R, I;
    public Complex(double a = 0, double b = 0)
    {
        this.R = a;
        this.I = b;
    }

    public Complex Conjugate()
    {
        return new Complex(R, -I);
    }

    public double SquareLength()
    {
        return R * R + I * I;
    }

    public double Abs()
    {
        return Math.Sqrt(R * R + I * I);
    }
    public static Complex operator +(Complex x, Complex y)
    {
        return new Complex(x.R + y.R, x.I + y.I);
    }

    public static Complex operator -(Complex x, Complex y)
    {
        return new Complex(x.R - y.R, x.I - y.I);
    }

    public static Complex operator *(Complex x, Complex y)
    {
        return new Complex(x.R * y.R - x.I * y.I, x.R * y.I + x.I * y.R);
    }

    public static Complex operator /(Complex x, double k)
    {
        return new Complex(x.R / k, x.I / k);
    }

    public static Complex operator *(Complex x, double k)
    {
        return new Complex(x.R * k, x.I * k);
    }

    public static Complex operator *(double k, Complex x)
    {
        return new Complex(x.R * k, x.I * k);
    }

    public static Complex operator /(Complex x, Complex y)
    {
        return (x * y.Conjugate()) / y.SquareLength();
    }
}