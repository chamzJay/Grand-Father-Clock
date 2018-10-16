using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrandFatherClock
{
    public partial class Form1 : Form
    {
        
        double[,] xVal= new double[4, 2];      //second,min,hour,pend -> x1,x2
        double[,] yVal = new double[4, 2];

        int second = DateTime.Now.Second;
        int min = DateTime.Now.Minute;
        int hour = DateTime.Now.Hour;

        double theta = -(2 * Math.PI / 60);




        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void DrawCircle(){
            int centerX = 100;
            int centerY =panel1.Height-900;
            int diameter = 400;
            panel1.CreateGraphics().DrawEllipse(Pens.Black, centerX, centerY, diameter, diameter);
        }

        public void DrawRectangle()
        {
            Graphics g = panel1.CreateGraphics();
            g.DrawRectangle(Pens.Black, 50,panel1.Height-400, 500, 300);

        }

        public void DrawHour(float x1, float y1, float x2, float y2)
        {
            Graphics g = panel1.CreateGraphics();
            g.DrawLine(Pens.DarkGray, x1, panel1.Height - y1, x2, panel1.Height - y2);
        }

        public void DrawMinute(float x1, float y1, float x2, float y2)
        {
            Graphics g = panel1.CreateGraphics();
            g.DrawLine(Pens.Black, x1, panel1.Height - y1, x2, panel1.Height - y2);
        }

        public void DrawSecond(float x1, float y1, float x2, float y2)
        {
            Graphics g = panel1.CreateGraphics();
            g.DrawLine(Pens.Red, x1, panel1.Height - y1, x2, panel1.Height - y2);
        }

        public void DrawPendulum(float x1, float y1, float x2, float y2)
        {
            Graphics g = panel1.CreateGraphics();
            g.DrawLine(Pens.Red, x1, panel1.Height - y1, x2, panel1.Height - y2);
        }

        public void DrawNumbers(int number,float x, float y)
        {
            Graphics g = panel1.CreateGraphics();
            g.DrawString(number.ToString(), new Font("Arial", 16), new SolidBrush(Color.Red), x,panel1.Height- y);
        }

        public void EraseLine(float x1, float y1, float x2, float y2)
        {
            Graphics g = panel1.CreateGraphics();
            g.DrawLine(Pens.DarkCyan, x1, panel1.Height - y1, x2, panel1.Height - y2);

        }

        void Rotate(int obj, int point, double t)
        {
            double x1 = xVal[obj,point];
            double y1 = yVal[obj, point];
            xVal[obj, point] = (x1 * Math.Cos(t)) - (y1 * Math.Sin(t));
            yVal[obj, point] = (x1 * Math.Sin(t)) + (y1 * Math.Cos(t));
            
        }
        
        void Translate(int obj, int point, double tx, double ty)
        {
            xVal[obj, point] += tx;
            yVal[obj, point] += ty;

        }

        void Scale(int obj,int point,double sx,double sy)
        {
            xVal[obj, point] *= sx;
            yVal[obj, point] *= sy;
        }

        void F_Rotate(int obj, int point, double t, double tx, double ty)
        {
            Translate(obj, point, -tx, -ty);
            Rotate(obj, point, t);
            Translate(obj, point, tx, ty);
        }

        async void Pendulum()
        {
            double t = 0;
            while (true)
            {
                double theta = 0.1;

                for(int i = 0; i < 4;i++)
                {
                    DrawPendulum((float)xVal[3, 0], (float)yVal[3, 0], (float)xVal[3, 1], (float)yVal[3, 1]);
                    await Task.Delay(50);
                    EraseLine((float)xVal[3, 0], (float)yVal[3, 0], (float)xVal[3, 1], (float)yVal[3, 1]);
                    F_Rotate(3, 1, theta, 300, 400);
                }

                for (int i = 0; i < 8; i++)
                {
                    DrawPendulum((float)xVal[3, 0], (float)yVal[3, 0], (float)xVal[3, 1], (float)yVal[3, 1]);
                    await Task.Delay(50);
                    EraseLine((float)xVal[3, 0], (float)yVal[3, 0], (float)xVal[3, 1], (float)yVal[3, 1]);
                    F_Rotate(3, 1, -theta, 300, 400);
                }

                for (int i = 0; i < 4; i++)
                {
                    DrawPendulum((float)xVal[3, 0], (float)yVal[3, 0], (float)xVal[3, 1], (float)yVal[3, 1]);
                    await Task.Delay(50);
                    EraseLine((float)xVal[3, 0], (float)yVal[3, 0], (float)xVal[3, 1], (float)yVal[3, 1]);
                    F_Rotate(3, 1, theta, 300, 400);
                }


            }

            

        }

        async void Clock()
        {
            while (true)
            {


                DrawSecond((float)xVal[0, 0], (float)yVal[0, 0], (float)xVal[0, 1], (float)yVal[0, 1]);
                DrawMinute((float)xVal[0, 0], (float)yVal[0, 0], (float)xVal[1, 1], (float)yVal[1, 1]);
                DrawHour((float)xVal[0, 0], (float)yVal[0, 0], (float)xVal[2, 1], (float)yVal[2, 1]);

                await Task.Delay(1000);
                EraseLine((float)xVal[0, 0], (float)yVal[0, 0], (float)xVal[0, 1], (float)yVal[0, 1]);
                EraseLine((float)xVal[0, 0], (float)yVal[0, 0], (float)xVal[1, 1], (float)yVal[1, 1]);
                EraseLine((float)xVal[0, 0], (float)yVal[0, 0], (float)xVal[2, 1], (float)yVal[2, 1]);

                F_Rotate(0, 1, theta, 300, 700);
                F_Rotate(1, 1, -(2 * Math.PI / (60 * 60)), 300, 700);
                F_Rotate(2, 1, -(2 * Math.PI / (60 * 60 * 60)) * hour, 300, 700);

                //panel1.Invalidate();
            }
        }

        private  void Form1_Paint(object sender, PaintEventArgs e)
        {

            xVal[0, 0] = 300;   //center
            yVal[0, 0] = 700;

            xVal[2, 0] = 300;   //numbers
            yVal[2, 0] = 880;

            xVal[0, 1] = 300;   //second
            yVal[0, 1] = 870;

            xVal[1, 1] = 300;   //min
            yVal[1, 1] = 880;

            xVal[2, 1] = 300;   //hour
            yVal[2, 1] = 850;

            xVal[3, 0] = 300;   //pend
            yVal[3, 0] = 400;
            xVal[3, 1] = 300;   
            yVal[3, 1] = 150;


            if (hour > 12)
                hour -= 12;

            F_Rotate(0, 1, theta*second, 300, 700);
            F_Rotate(1, 1, theta * min, 300, 700);
            F_Rotate(2, 1, -(2*Math.PI/12) * hour, 300, 700);

            DrawNumbers(12, (float)xVal[2, 0], (float)yVal[2, 0]);
            F_Rotate(2, 0, -((2 * Math.PI) / 12) , 300, 716);

            for (int i = 0; i < 11; i++)
            {
                DrawNumbers(i+1, (float)xVal[2, 0], (float)yVal[2, 0]);
                F_Rotate(2, 0, -((2 * Math.PI) / 12), 300, 716);
     
            }
            DrawCircle();
            DrawRectangle();
            Thread t1 = new Thread(Pendulum);
            Thread t2 = new Thread(Clock);
            t1.Start();
            t2.Start();


            







            }


            
        

    }


    
}
