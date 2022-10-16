using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DFinMath;

namespace OptionSimulatio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private const int MStep = 12;
        private const int NPath = 16;
        private double[,] St = new double[NPath, MStep + 1];
        private double mu = 0.0;

        private void button1_Click(object sender, EventArgs e)
        {
            Random rv = new Random(1234);
            double S0 = double.Parse(textBox1.Text); //股價初始價格
            mu = double.Parse(textBox2.Text); //單位時間內預期資產的年化報酬率
            double dt = double.Parse(textBox3.Text)/12.0;  //單位時間1/12年
            double sigma = double.Parse(textBox4.Text);

            for(int i = 0; i < NPath; i++)   //16條交易路線
            {
                St[i, 0] = S0;
                for(int j = 0; j < MStep; j++)   //每個交易做13次
                {
                    St[i, j + 1] = St[i, j] + St[i, j] * (mu * dt + sigma * Math.Sqrt(dt) * DStat.N_Inv(rv.NextDouble()));
                }
            }
            for(int i = 0; i < NPath; i++)
            {
                for(int j = 0; j < MStep + 1; j++)
                {
                    listBox1.Items.Add(St[i, j].ToString());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.Clear(Color.White);

            Pen p = new Pen(Color.Black, 1);
            int x0 = 30;
            int y0 = 10;
            for(int i = 0; i <= 12; i++)
            {
                g.DrawLine(p, 30 * i + x0, 0 + y0, 30 * i + x0, 300 + y0);
            }
            for(int j = 0; j <= 20; j++)
            {
                g.DrawLine(p, 0 + x0, 15 * j + y0, 380 + y0, 15 * j + y0);
            }

            x0 = 30;
            y0 = 160;
            for(int k = 0; k < NPath; k++)
            {
                p = new Pen(Color.FromArgb(k * 15, (15 - k) * 15, k * 12), 1);
                for(int m = 0; m < 12; m++)
                {
                    g.DrawLine(p, x0 + m * 30, (int)(3 * (100.0 - St[k, m])) + y0, x0 + (m + 1) * 30, (int)(3 * (100.0 - St[k, m + 1])) + y0);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double sum = 0.0;
            double K = double.Parse(textBox5.Text);

            double[] CT = new double[NPath];
            for(int i = 0; i<NPath; i++)
            {
                sum = sum + Math.Max(St[i, MStep] - K, 0);
            }

            double C0 = (sum / NPath) * Math.Exp(-mu * 1);
            textBox6.Text = C0.ToString("F6");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
