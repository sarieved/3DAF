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

namespace _3DAF
{

    public partial class Form1 : Form
    {
        public delegate double Funct(double x, double y);
        public static Graphics g;
        public static Font drawFont = new Font("Arial", 10);
        public static SolidBrush b = new SolidBrush(Color.Black);
        public static Pen pen = new Pen(Color.Black, 2);
        Polyhedron3DAF cur;
        Point3DAF cur_cent;
        Edge3DAF line;
        static double xcent = 0;
        static double ycent = 0;
        string projection = "persp";
        static double xdispl = 0;
        static double ydispl = 0;
        static int pbh = 0;
        static int pbw = 0;
        static string pl;
        public class Object3DAF
        {
            public virtual void drawP(ref PictureBox pb, Pen p)
            {

            }

            public virtual void drawIso(ref PictureBox pb, Pen p)
            {
                
            }

            public virtual void drawOrto(ref PictureBox pb, Pen p)
            {
                
            }
        }
        //--------------------AXIS------------------------
        void drawAxP()
        {
            g = Graphics.FromImage(pictureBox1.Image);

            Point3DAF x = new Point3DAF(Convert.ToDouble(pictureBox1.Width), ycent, 0);
            Point3DAF y = new Point3DAF(xcent, Convert.ToDouble(0), 0);
            Point3DAF z = new Point3DAF(xcent, ycent, 5000);
            Point3DAF zero = new Point3DAF(xcent, ycent, 0);

            Edge3DAF xAx = new Edge3DAF(zero, x);
            Edge3DAF yAx = new Edge3DAF(zero, y);
            Edge3DAF zAx = new Edge3DAF(zero, z);

            g.DrawString("X", drawFont, b, 420.0F, 300.0F);
            g.DrawString("Y", drawFont, b, 150.0F, 5.0F);
            g.DrawString("Z", drawFont, b, 20.0F, 69.0F);

            xAx.drawP(ref pictureBox1, new Pen(Color.Blue, 2));
            yAx.drawP(ref pictureBox1, new Pen(Color.Red, 2));
            zAx.drawP(ref pictureBox1, new Pen(Color.Green, 2));
        }

        void drawAxIso()
        {
            g = Graphics.FromImage(pictureBox1.Image);

            Point3DAF x = new Point3DAF(Convert.ToDouble(pictureBox1.Width) + 100, ycent, 0);
            Point3DAF y = new Point3DAF(xcent, Convert.ToDouble(pictureBox1.Height) + 2000, 0);
            Point3DAF z = new Point3DAF(xcent, ycent, 400);
            Point3DAF zero = new Point3DAF(xcent, ycent, 0);

            Edge3DAF xAx = new Edge3DAF(zero, x);
            Edge3DAF yAx = new Edge3DAF(zero, y);
            Edge3DAF zAx = new Edge3DAF(zero, z);

            g.DrawString("X", drawFont, b, 58.0F, 459.0F);
            g.DrawString("Y", drawFont, b, 150.0F, 5.0F);
            g.DrawString("Z", drawFont, b, 418.0F, 439.0F);

            xAx.drawIso(ref pictureBox1, new Pen(Color.Blue, 2));
            yAx.drawIso(ref pictureBox1, new Pen(Color.Red, 2));
            zAx.drawIso(ref pictureBox1, new Pen(Color.Green, 2));
        }

        void drawAxOrto()
        {
            g = Graphics.FromImage(pictureBox1.Image);
            Point zero = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);

            Pen px = new Pen(Color.Blue, 2);
            Pen py = new Pen(Color.Red, 2);
            Pen pz = new Pen(Color.Green, 2);

            if (comboBox3.SelectedItem.ToString() == "XZ")
            {
                pl = "XZ";
                Point x = new Point(pictureBox1.Width, pictureBox1.Height / 2);
                Point z = new Point(pictureBox1.Width / 2, pictureBox1.Height);

                g.DrawString("X", drawFont, b, 420.0F, 220.0F);
                g.DrawString("Z", drawFont, b, 200.0F, 460.0F);

                g.DrawLine(px, x, zero);
                g.DrawLine(pz, z, zero);
                g.Dispose();
                pictureBox1.Invalidate();
            }
            else if (comboBox3.SelectedItem.ToString() == "XY")
            {
                pl = "XY";
                Point x = new Point(pictureBox1.Width, pictureBox1.Height / 2);
                Point y = new Point(pictureBox1.Width / 2, 0);

                g.DrawString("X", drawFont, b, 420.0F, 220.0F);
                g.DrawString("Y", drawFont, b, 200.0F, 3.0F);

                g.DrawLine(px, x, zero);
                g.DrawLine(py, y, zero);
                g.Dispose();
                pictureBox1.Invalidate();
            }
            else if (comboBox3.SelectedItem.ToString() == "YZ")
            {
                pl = "YZ";
                Point y = new Point(pictureBox1.Width / 2, 0);
                Point z = new Point(0, pictureBox1.Height / 2);

                g.DrawString("Y", drawFont, b, 200.0F, 3.0F);
                g.DrawString("Z", drawFont, b, 5.0F, 220.0F);

                g.DrawLine(py, y, zero);
                g.DrawLine(pz, z, zero);
                g.Dispose();
                pictureBox1.Invalidate();
            }
        }

        //-------------------MATRIX-----------------------
        public class matr
        {
            public double[,] m;
            public int height;
            public int length;

            public matr(matr matrix, int i)
            {
                m = new double[1, 4];
                height = 1;
                length = 4;
                m[0, 0] = matrix.m[i, 0];
                m[0, 1] = matrix.m[i, 1];
                m[0, 2] = matrix.m[i, 2];
                m[0, 3] = matrix.m[i, 3];
            }

            public matr(matr m2)
            {
                height = m2.height;
                length = m2.length;
                m = new double[height, length];
                for (int i = 0; i < height; ++i)
                {
                    for (int j = 0; j < length; ++j)
                    {
                        m[i, j] = m2.m[i, j];
                    }
                }
            }

            public matr(int h, int l)
            {
                m = new double[h, l];
                height = h;
                length = l;
            }

            public matr(string s, int h, int l)
            {
                m = new double[h, l];
                height = h;
                length = l;
                string[] bits = s.Split(';');
                int k = 0;
                for (int i = 0; i < h; ++i)
                    for (int j = 0; j < l; ++j)
                    {
                        double d;
                        d = double.Parse(bits[k]);
                        m[i, j] = d;
                        ++k;
                    }
            }
            public matr(double x, double y, double z) 
            {
                m = new double[1, 4];
                m[0, 0] = x;
                m[0, 1] = y;
                m[0, 2] = z;
                m[0, 3] = 1;
                height = 1;
                length = 4;
            }

            public matr(matr matrix, int str1, int str2)
            {
                height = 2;
                length = 4;
                m = new double[height, length];
                m[0, 0] = matrix.m[str1, 0];
                m[0, 1] = matrix.m[str1, 1];
                m[0, 2] = matrix.m[str1, 2];
                m[0, 3] = matrix.m[str1, 3];
                m[1, 0] = matrix.m[str2, 0];
                m[1, 1] = matrix.m[str2, 1];
                m[1, 2] = matrix.m[str2, 2];
                m[1, 3] = matrix.m[str2, 3];
            }

            public double midEl2()
            {
                double max = double.MinValue;
                double min = double.MaxValue;
                for (int i = 0; i < height; ++i)
                {
                    if (m[i, 1] < min)
                    {
                        min = m[i, 1];
                    }
                    else if (m[i, 1] > max)
                    {
                        max = m[i, 1];
                    }
                }
                return (max - min) / 2;
            }

            public double midEl3()
            {
                double max = double.MinValue;
                double min = double.MaxValue;
                for (int i = 0; i < height; ++i)
                {
                    if (m[i, 2] < min)
                    {
                        min = m[i, 2];
                    }
                    else if (m[i, 2] > max)
                    {
                        max = m[i, 2];
                    }
                }
                return (max - min) / 2;
            }

            public double[] multV(double[] v)
            {
                double[] res = new double[4];
                for (int i = 0; i < 4; ++i)
                {
                    double sum = 0;
                    for (int j = 0; j < 4; ++j)
                        sum += v[j] * m[j, i];
                    res[i] = sum;
                }
                return res;
            }

            public static matr operator *(matr m1, matr m2)
            {
                matr res = new matr(m1.height, m2.length);
                res.height = m1.height;
                res.length = m2.length;
                for (int i = 0; i < m1.height; ++i)
                {
                    for (int j = 0; j < m2.length; ++j)
                    {
                        double sum = 0;
                        for (int k = 0; k < m1.length; ++k)
                        {
                            sum += m1.m[i, k] * m2.m[k, j];
                        }
                        res.m[i, j] = sum;
                    }
                }
                return res;
            }

            public matr expand(matr exp)
            {
                matr res = new matr(height + exp.height, exp.length);
                for (int i = 0; i < height; ++i)
                    for (int j = 0; j < length; ++j)
                        res.m[i, j] = m[i, j];

                for (int i = 0; i < exp.height; ++i)
                    for (int j = 0; j < exp.length; ++j)
                        res.m[i + height, j] = exp.m[i, j];
                return res;
            }

            public void ToFile()
            {
                using (StreamWriter writer = new StreamWriter(File.Open("Matr.txt", FileMode.Create)))
                {
                    for (int i = 0; i < height; ++i)
                    {
                        for (int j = 0; j < length; ++j)
                        {
                            writer.Write(m[i, j]);
                            writer.Write(';');
                        }
                        writer.Write(System.Environment.NewLine);
                    }
                }
            }
        }
        //-------------------POINT-----------------------
        public class Point3DAF : Object3DAF
        {
            public matr M;
            public Point3DAF(double nx, double ny, double nz)
            {
                M = new matr(1, 4);
                M.m[0, 0] = nx;
                M.m[0, 1] = ny;
                M.m[0, 2] = nz;
                M.m[0, 3] = 1;
            }

            public Point3DAF(Point3DAF p2)
            {
                M = new matr(1, 4);
                M.m[0, 0] = p2.M.m[0, 0];
                M.m[0, 1] = p2.M.m[0, 1];
                M.m[0, 2] = p2.M.m[0, 2];
                M.m[0, 3] = p2.M.m[0, 3];
            }

            public override void drawP(ref PictureBox pb, Pen p)
            {
                Point dP = ToPersp();
                g = Graphics.FromImage(pb.Image);
                g.FillRectangle(b, dP.X, dP.Y, 2, 2);
                g.Dispose();
                pb.Invalidate();
            }

            public override void drawIso(ref PictureBox pb, Pen p)
            {
                Point dP = ToIso();
                g = Graphics.FromImage(pb.Image);
                g.FillRectangle(b, dP.X, dP.Y, 2, 2);
                g.Dispose();
                pb.Invalidate();
            }

            public override void drawOrto(ref PictureBox pb, Pen p)
            {
                Point dP = ToOrto();
                g = Graphics.FromImage(pb.Image);
                g.FillRectangle(b, dP.X, dP.Y, 2, 2);
                g.Dispose();
                pb.Invalidate();
            }

            public Point ToIso()
            {
                double angle = 120;
                double sin = Math.Sin(angle * Math.PI / 180);
                double cos = Math.Cos(angle * Math.PI / 180);

                double Tx = -xcent;
                double Ty = -ycent;
                double Tz = 0;

                matr res = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" 
                    + Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);
                matr iso = new matr(cos.ToString() + ";" + (sin * sin).ToString() + ";0;0;0;" + cos.ToString() + 
                    ";0;0;" + sin.ToString() + ";" + (-sin * cos).ToString() + ";0;0;0;0;0;1", 4, 4);
                res = res * iso;
                
                Tx = xcent;
                Ty = ycent;

                matr trans = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" 
                    + Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);

                res = res * trans;
                matr dr = M * res;
                return new Point(Convert.ToInt32(dr.m[0, 0]), Convert.ToInt32(dr.m[0, 1]));
            }

            public Point ToPersp()
            {
                matr m = new matr("1;0;0;0;0;1;0;0;0;0;1;" + (0.001).ToString() + ";0;0;0;1", 4, 4);
                matr res = M * m;
                return new Point(Convert.ToInt32(res.m[0, 0] / res.m[0, 3]), Convert.ToInt32(res.m[0, 1] / res.m[0, 3]));
            }

            public Point ToOrto()
            {
                if (pl == "XZ")
                {
                    return new Point(Convert.ToInt32(M.m[0, 0] - xdispl), Convert.ToInt32(M.m[0, 2]) + Convert.ToInt32(ydispl));
                }
                else if (pl == "XY")
                {
                    return new Point(Convert.ToInt32(M.m[0, 0] - xdispl), -Convert.ToInt32(M.m[0, 1]) + Convert.ToInt32(ycent - ydispl) + pbh);
                }
                else if (pl == "YZ")
                {
                    return new Point(-Convert.ToInt32(M.m[0, 2]) + Convert.ToInt32(xdispl + pbw - xcent), -Convert.ToInt32(M.m[0, 1]) + Convert.ToInt32(ycent - ydispl) + pbh);
                }
                return new Point(0, 0);
            }
        }

        //-------------------EDGE-----------------------
        public class Edge3DAF : Object3DAF
        {
            public matr M;

            public Edge3DAF(Point3DAF p1, Point3DAF p2)
            {
                M = new matr(2, 4);
                for (int j = 0; j < 4; ++j)
                {
                    M.m[0, j] = p1.M.m[0, j];
                    M.m[1, j] = p2.M.m[0, j];
                }
            }

            public override void drawP(ref PictureBox pb, Pen p)
            {
                matr pers = new matr("1;0;0;0;0;1;0;0;0;0;1;" + (0.001).ToString() + ";0;0;0;1", 4, 4);
                matr dr = M * pers;
                Point dp1 = new Point(Convert.ToInt32(dr.m[0, 0] / dr.m[0, 3]), Convert.ToInt32(dr.m[0, 1] / dr.m[0, 3]));
                Point dp2 = new Point(Convert.ToInt32(dr.m[1, 0] / dr.m[1, 3]), Convert.ToInt32(dr.m[1, 1] / dr.m[1, 3]));
                g = Graphics.FromImage(pb.Image);
                g.DrawLine(p, dp1, dp2);
                g.Dispose();
                pb.Invalidate();
            }

            public override void drawIso(ref PictureBox pb, Pen p)
            {
                double angle = 120;
                double sin = Math.Sin(angle * Math.PI / 180);
                double cos = Math.Cos(angle * Math.PI / 180);

                double Tx = -xcent;
                double Ty = -ycent;
                double Tz = 0;

                matr res = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" + Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);

                matr iso = new matr(cos.ToString() + ";" + (sin * sin).ToString() + ";0;0;0;" + cos.ToString() + ";0;0;"
                    + sin.ToString() + ";" + (-sin * cos).ToString() + ";0;0;0;0;0;1", 4, 4);
                res = res * iso;

                Tx = xcent;
                Ty = ycent;

                matr trans = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" + Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);

                res = res * trans;
                matr dr = M * res;

                Point dp1 = new Point(Convert.ToInt32(dr.m[0, 0]), Convert.ToInt32(dr.m[0, 1]));
                Point dp2 = new Point(Convert.ToInt32(dr.m[1, 0]), Convert.ToInt32(dr.m[1, 1]));
                g = Graphics.FromImage(pb.Image);
                g.DrawLine(p, dp1, dp2);
                g.Dispose();
                pb.Invalidate();
            }

            public override void drawOrto(ref PictureBox pb, Pen p)
            {
                Point p1 = new Point();
                Point p2 = new Point();
                if (pl == "XZ")
                {
                    p1 = new Point(Convert.ToInt32(M.m[0, 0] - xdispl), Convert.ToInt32(M.m[0, 2]) + Convert.ToInt32(ydispl));
                    p2 = new Point(Convert.ToInt32(M.m[1, 0] - xdispl), Convert.ToInt32(M.m[1, 2]) + Convert.ToInt32(ydispl));
                }
                else if (pl == "XY")
                {
                    p1 = new Point(Convert.ToInt32(M.m[0, 0] - xdispl), -Convert.ToInt32(M.m[0, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                    p2 = new Point(Convert.ToInt32(M.m[1, 0] - xdispl), -Convert.ToInt32(M.m[1, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                }
                else if (pl == "YZ")
                {
                    p1 = new Point(-Convert.ToInt32(M.m[0, 2]) + Convert.ToInt32(xdispl + pb.Width - xcent), -Convert.ToInt32(M.m[0, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                    p2 = new Point(-Convert.ToInt32(M.m[1, 2]) + Convert.ToInt32(xdispl + pb.Width - xcent), -Convert.ToInt32(M.m[1, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                }

                g = Graphics.FromImage(pb.Image);
                g.DrawLine(p, p1, p2);
                g.Dispose();
                pb.Invalidate();
            }
        }
        //-------------------POLYGON-----------------------
        public class Polygon3DAF : Object3DAF
        {
            public List<Edge3DAF> edges;
            public matr M;

            public Polygon3DAF()
            {
                edges = new List<Edge3DAF>();
                M = new matr(0, 0);
            }

            public Polygon3DAF(matr m2)
            {
                edges = new List<Edge3DAF>();
                M = new matr(m2);
            }

            public override void drawP(ref PictureBox pb, Pen p)
            {
                matr pers = new matr("1;0;0;0;0;1;0;0;0;0;1;" + (0.001).ToString() + ";0;0;0;1", 4, 4);
                matr dr = M * pers;
                int l = 0;
                for (int i = 0; i < edges.Count; ++i)
                {
                    Point dp1 = new Point(Convert.ToInt32(dr.m[l, 0] / dr.m[l, 3]), Convert.ToInt32(dr.m[l, 1] / dr.m[l, 3]));
                    Point dp2 = new Point(Convert.ToInt32(dr.m[l + 1, 0] / dr.m[l + 1, 3]), Convert.ToInt32(dr.m[l + 1, 1] / dr.m[l + 1, 3]));
                    g = Graphics.FromImage(pb.Image);
                    g.DrawLine(p, dp1, dp2);
                    g.Dispose();
                    pb.Invalidate();
                    l += 2;
                }
            }

            public override void drawIso(ref PictureBox pb, Pen p)
            {
                double angle = 120;
                double sin = Math.Sin(angle * Math.PI / 180);
                double cos = Math.Cos(angle * Math.PI / 180);

                double Tx = -xcent;
                double Ty = -ycent;
                double Tz = 0;

                matr res = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" + Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);

                matr iso = new matr(cos.ToString() + ";" + (sin * sin).ToString() + ";0;0;0;" + cos.ToString() +
                    ";0;0;" + sin.ToString() + ";" + (-sin * cos).ToString() + ";0;0;0;0;0;1", 4, 4);
                res = res * iso;

                Tx = xcent;
                Ty = ycent;

                matr trans = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" + Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);

                res = res * trans;
                matr dr = M * res;
                int l = 0;
                for (int i = 0; i < edges.Count; ++i)
                {
                    Point dp1 = new Point(Convert.ToInt32(dr.m[l, 0] / dr.m[l, 3]), Convert.ToInt32(dr.m[l, 1] / dr.m[l, 3]));
                    Point dp2 = new Point(Convert.ToInt32(dr.m[l + 1, 0] / dr.m[l + 1, 3]), Convert.ToInt32(dr.m[l + 1, 1] / dr.m[l + 1, 3]));
                    g = Graphics.FromImage(pb.Image);
                    g.DrawLine(p, dp1, dp2);
                    g.Dispose();
                    pb.Invalidate();
                    l += 2;
                }
            }

            public override void drawOrto(ref PictureBox pb, Pen p)
            {
                Point p1;
                Point p2;
                int l = 0;
                if (pl == "XZ")
                {
                    for (int i = 0; i < edges.Count; ++i)
                    {
                        p1 = new Point(Convert.ToInt32(M.m[l, 0] - xdispl), Convert.ToInt32(M.m[l, 2]) + Convert.ToInt32(ydispl));
                        p2 = new Point(Convert.ToInt32(M.m[l + 1, 0] - xdispl), Convert.ToInt32(M.m[l + 1, 2]) + Convert.ToInt32(ydispl));
                        g = Graphics.FromImage(pb.Image);
                        g.DrawLine(p, p1, p2);
                        g.Dispose();
                        pb.Invalidate();
                        l += 2;
                    }
                }
                else if (pl == "XY")
                {
                    for (int i = 0; i < edges.Count; ++i)
                    {
                        p1 = new Point(Convert.ToInt32(M.m[l, 0] - xdispl), -Convert.ToInt32(M.m[l, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                        p2 = new Point(Convert.ToInt32(M.m[l + 1, 0] - xdispl), -Convert.ToInt32(M.m[l + 1, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                        g = Graphics.FromImage(pb.Image);
                        g.DrawLine(p, p1, p2);
                        g.Dispose();
                        pb.Invalidate();
                        l += 2;
                    }
                }
                else if (pl == "YZ")
                {
                    for (int i = 0; i < edges.Count; ++i)
                    {
                        p1 = new Point(-Convert.ToInt32(M.m[l, 2]) + Convert.ToInt32(xdispl + pb.Width - xcent), -Convert.ToInt32(M.m[l, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                        p2 = new Point(-Convert.ToInt32(M.m[l + 1, 2]) + Convert.ToInt32(xdispl + pb.Width - xcent), -Convert.ToInt32(M.m[l + 1, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                        g = Graphics.FromImage(pb.Image);
                        g.DrawLine(p, p1, p2);
                        g.Dispose();
                        pb.Invalidate();
                        l += 2;
                    }
                }
            }

            public void Add(Edge3DAF e)
            {
                edges.Add(e);
                M = M.expand(e.M);
            }
        }
        //-------------------Polyhedron-----------------------
        public class Polyhedron3DAF : Object3DAF
        {
            public List<Polygon3DAF> polygons;
            public matr M;

            public Polyhedron3DAF()
            {
                polygons = new List<Polygon3DAF>();
                M = new matr(0, 0);
            }

            public Polyhedron3DAF(matr Matrix)
            {
                polygons = new List<Polygon3DAF>();
                M = Matrix;
            }

            public Polyhedron3DAF(Polyhedron3DAF p)
            {
                polygons = new List<Polygon3DAF>();
                M = new matr(p.M);
            }

            public override void drawP(ref PictureBox pb, Pen p)
            {
                matr pers = new matr("1;0;0;0;0;1;0;0;0;0;1;" + (0.001).ToString() + ";0;0;0;1", 4, 4);

                matr dr = M * pers;
                for (int l = 0; l < M.height - 1; l += 2)
                {
                    Point dp1 = new Point(Convert.ToInt32(dr.m[l, 0] / dr.m[l, 3]), 
                        Convert.ToInt32((dr.m[l, 1] - 2 * (dr.m[l, 1] - ycent)) / dr.m[l, 3]));
                    Point dp2 = new Point(Convert.ToInt32(dr.m[l + 1, 0] / dr.m[l + 1, 3]), 
                        Convert.ToInt32((dr.m[l + 1, 1] - 2 * (dr.m[l + 1, 1] - ycent)) / dr.m[l + 1, 3]));
                    g = Graphics.FromImage(pb.Image);
                    g.DrawLine(p, dp1, dp2);
                    g.Dispose();
                    pb.Invalidate();
                }
            }

            public override void drawIso(ref PictureBox pb, Pen p)
            {
                double angle = 120;
                double sin = Math.Sin(angle * Math.PI / 180);
                double cos = Math.Cos(angle * Math.PI / 180);

                double Tx = -xcent;
                double Ty = -ycent;
                double Tz = 0;

                matr res = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" + 
                    Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);

                matr iso = new matr(cos.ToString() + ";" + (sin * sin).ToString() + ";0;0;0;" + cos.ToString() + 
                    ";0;0;" + sin.ToString() + ";" + (-sin * cos).ToString() + ";0;0;0;0;0;1", 4, 4);
                res = res * iso;


                Tx = xcent;
                Ty = ycent;

                matr trans = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" + 
                    Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);

                res = res * trans;
                matr dr = M * res;
                for (int l = 0; l < M.height - 1; l += 2)
                {
                    Point dp1 = new Point(Convert.ToInt32(dr.m[l, 0]), Convert.ToInt32(dr.m[l, 1]));
                    Point dp2 = new Point(Convert.ToInt32(dr.m[l + 1, 0]), Convert.ToInt32(dr.m[l + 1, 1]));
                    g = Graphics.FromImage(pb.Image);
                    g.DrawLine(p, dp1, dp2);
                    g.Dispose();
                    pb.Invalidate();
                }
            }

            public override void drawOrto(ref PictureBox pb, Pen p)
            {
                Point p1;
                Point p2;
                if (pl == "XZ")
                {
                    for (int l = 0; l < M.height - 1; l += 2)
                    {
                        p1 = new Point(Convert.ToInt32(M.m[l, 0] - xdispl), Convert.ToInt32(M.m[l, 2]) + Convert.ToInt32(ydispl));
                        p2 = new Point(Convert.ToInt32(M.m[l + 1, 0] - xdispl), Convert.ToInt32(M.m[l + 1, 2]) + Convert.ToInt32(ydispl));
                        g = Graphics.FromImage(pb.Image);
                        g.DrawLine(p, p1, p2);
                        g.Dispose();
                        pb.Invalidate();
                    }
                }
                else if (pl == "XY")
                {
                    for (int l = 0; l < M.height - 1; l += 2)
                    {
                        p1 = new Point(Convert.ToInt32(M.m[l, 0] - xdispl), -Convert.ToInt32(M.m[l, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                        p2 = new Point(Convert.ToInt32(M.m[l + 1, 0] - xdispl), -Convert.ToInt32(M.m[l + 1, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                        g = Graphics.FromImage(pb.Image);
                        g.DrawLine(p, p1, p2);
                        g.Dispose();
                        pb.Invalidate();
                    }
                }
                else if (pl == "YZ")
                {
                    for (int l = 0; l < M.height - 1; l += 2)
                    {
                        p1 = new Point(-Convert.ToInt32(M.m[l, 2]) + Convert.ToInt32(xdispl + pb.Width - xcent), -Convert.ToInt32(M.m[l, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                        p2 = new Point(-Convert.ToInt32(M.m[l + 1, 2]) + Convert.ToInt32(xdispl + pb.Width - xcent), -Convert.ToInt32(M.m[l + 1, 1]) + Convert.ToInt32(ycent - ydispl) + pb.Height);
                        g = Graphics.FromImage(pb.Image);
                        g.DrawLine(p, p1, p2);
                        g.Dispose();
                        pb.Invalidate();
                    }
                }
            }

            public void Add(Polygon3DAF p)
            {
                polygons.Add(p);
                M = M.expand(p.M);
            }
        }
        
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            pbh = pictureBox1.Height;
            xcent = pictureBox1.Width / 3.0;
            ycent = pictureBox1.Height / 3.0 * 2;
            xdispl = xcent - pictureBox1.Width / 2.0;
            ydispl = pictureBox1.Height / 2.0;

            if (projection == "persp")
                drawAxP();
            if (projection == "iso")
                drawAxIso();
            if (projection == "orto")
                drawAxOrto();
        }

        private Polyhedron3DAF CreateCube(double stx, double sty, double stz, int length)
        {
            double curx = stx;
            double cury = sty;
            double curz = stz;

            Point3DAF curp = new Point3DAF(curx, cury, curz);
            Polyhedron3DAF PH = new Polyhedron3DAF();
            Point3DAF nxp;
            Polygon3DAF pol;

            for (int dz = 0; dz <= 1; ++dz)
            {
                pol = new Polygon3DAF();
                for (int d = 1; d >= -1; --d)
                {
                    if (d == 0)
                        continue;
                    nxp = new Point3DAF(curx + d * length, cury, curz + dz * length);
                    pol.Add(new Edge3DAF(curp, nxp));
                    curx += d * length;

                    curp = nxp;
                    nxp = new Point3DAF(curx, cury + d * length, curz + dz * length);
                    pol.Add(new Edge3DAF(curp, nxp));
                    cury += d * length;
                    curp = nxp;
                }
                PH.Add(pol);
                curx = stx;
                cury = sty;
                curp = new Point3DAF(stx, sty, stz + length);
            }

            curp = new Point3DAF(stx, sty, stz);
            for (int dy = 0; dy <= 1; ++dy)
            {
                pol = new Polygon3DAF();
                for (int d = 1; d >= -1; --d)
                {
                    if (d == 0)
                        continue;
                    nxp = new Point3DAF(curx + d * length, cury + dy * length, curz);
                    pol.Add(new Edge3DAF(curp, nxp));
                    curx += d * length;

                    curp = nxp;
                    nxp = new Point3DAF(curx, cury + dy * length, curz + d * length);
                    pol.Add(new Edge3DAF(curp, nxp));
                    curz += d * length;
                    curp = nxp;
                }
                PH.Add(pol);
                curx = stx;
                curz = stz;

                curp = new Point3DAF(stx, sty + length, stz);
            }

            curp = new Point3DAF(stx, sty, stz);
            for (int dx = 0; dx <= 1; ++dx)
            {
                pol = new Polygon3DAF();
                for (int d = 1; d >= -1; --d)
                {
                    if (d == 0)
                        continue;
                    nxp = new Point3DAF(curx + dx * length, cury + d * length, curz);
                    pol.Add(new Edge3DAF(curp, nxp));
                    cury += d * length;

                    curp = nxp;
                    nxp = new Point3DAF(curx + dx * length, cury, curz + d * length);
                    pol.Add(new Edge3DAF(curp, nxp));
                    curz += d * length;
                    curp = nxp;
                }
                PH.Add(pol);
                curx = stx;
                cury = sty;

                curp = new Point3DAF(stx + length, sty, stz);
            }
            cur_cent = new Point3DAF(xcent + (double)(numericUpDown1.Value + numericUpDown4.Value / 2), 
                ycent + (double)(numericUpDown2.Value + numericUpDown4.Value / 2), (double)(numericUpDown3.Value + numericUpDown4.Value / 2));
            return PH;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double stx = Convert.ToDouble(numericUpDown1.Value);
            double sty = Convert.ToDouble(numericUpDown2.Value);
            double stz = Convert.ToDouble(numericUpDown3.Value);
            int length = (int)numericUpDown4.Value;

            cur = CreateCube(xcent + stx, ycent + sty, stz, length);
            cur.M.ToFile();

            if (projection == "persp")
                cur.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                cur.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                cur.drawOrto(ref pictureBox1, pen);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            projection = "iso";
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            drawAxIso();
            if (cur != null)
                cur.drawIso(ref pictureBox1, pen);
            if (line != null)
            {
                pen.Color = Color.Red;
                line.drawIso(ref pictureBox1, pen);
                pen.Color = Color.Black;
            }
        }

        public matr Transition(double x, double y, double z)
        {
            return new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + x.ToString() + ";" 
                + y.ToString() + ";" + z.ToString() + ";1", 4, 4);
        }

        public matr Scale(double kx, double ky, double kz)
        {
            double Tx = xcent;
            double Ty = ycent;
            double Tz = 0;
            matr res = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + (-Tx).ToString() + ";" 
                + (-Ty).ToString() + ";" + Tz.ToString() + ";1", 4, 4);


            matr scale = new matr(kx.ToString() + ";0;0;0;0;" + ky.ToString() + 
                ";0;0;0;0;" + kz.ToString() + ";0;0;0;0;1", 4, 4);
            res = res * scale;

            matr trans = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + (Tx).ToString() + ";" 
                + (Ty).ToString() + ";" + Tz.ToString() + ";1", 4, 4);
            res = res * trans;
            return res;
        }

        public matr Rotation(string ax, double angle)
        {
            double Tx = -xcent;
            double Ty = -ycent;
            double Tz = 0;
            matr res = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" 
                + Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);

            double sin = Math.Sin(angle * Math.PI / 180);
            double cos = Math.Cos(angle * Math.PI / 180);
            matr rot = new matr(0, 0);
            if (ax == "X")
                rot = new matr("1;0;0;0;0;" + cos.ToString() + ";" + sin.ToString() + 
                    ";0;0;" + (-sin).ToString() + ";" + cos.ToString() + ";0;0;0;0;1", 4, 4);
            if (ax == "Y")
                rot = new matr(cos.ToString() + ";0;" + (-sin).ToString() + 
                    ";0;0;1;0;0;" + sin.ToString() + ";0;" + cos.ToString() + ";0;0;0;0;1", 4, 4);
            if (ax == "Z")
                rot = new matr(cos.ToString() + ";" + sin.ToString() + ";0;0;" 
                    + (-sin).ToString() + ";" + cos.ToString() + ";0;0;0;0;1;0;0;0;0;1", 4, 4);

            res = res * rot;

            Tx = xcent;
            Ty = ycent;

            matr trans = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" + Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);

            res = res * trans;
            return res;
        }

        public matr Reflection(string ax)
        {
            double Tx = 2 * xcent;
            double Ty = 2 * ycent;

            matr res = new matr(0, 0);

            if (ax == "X")
                res = new matr("-1;0;0;0;0;1;0;0;0;0;1;0;0;0;0;1", 4, 4);
            if (ax == "Y")
                res = new matr("1;0;0;0;0;-1;0;0;0;0;1;0;0;0;0;1", 4, 4);
            if (ax == "Z")
                res = new matr("1;0;0;0;0;1;0;0;0;0;-1;0;0;0;0;1", 4, 4);

            matr trans;
            if (ax == "X")
            {
                trans = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";0;0;1", 4, 4);
                res = res * trans;
            }
            if (ax == "Y")
            {
                trans = new matr("1;0;0;0;0;1;0;0;0;0;1;0;0;" + Ty.ToString() + ";0;1", 4, 4);
                res = res * trans;
            }
            return res;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            if (projection == "persp")
                drawAxP();
            if (projection == "iso")
                drawAxIso();
            if (projection == "orto")
                drawAxOrto();

            cur.M = cur.M * Transition(Convert.ToDouble(numericUpDown5.Value), 
                Convert.ToDouble(numericUpDown6.Value), Convert.ToDouble(numericUpDown7.Value));
            cur_cent.M = cur_cent.M * Transition(Convert.ToDouble(numericUpDown5.Value), 
                Convert.ToDouble(numericUpDown6.Value), Convert.ToDouble(numericUpDown7.Value));

            if (projection == "persp")
                cur.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                cur.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                cur.drawOrto(ref pictureBox1, pen);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);

            if (projection == "persp")
                drawAxP();
            if (projection == "iso")
                drawAxIso();
            if (projection == "orto")
                drawAxOrto();

            cur.M = cur.M * Scale(Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox3.Text));
            cur_cent.M = cur_cent.M * Scale(Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox3.Text));

            if (projection == "persp")
                cur.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                cur.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                cur.drawOrto(ref pictureBox1, pen);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);

            if (projection == "persp")
                drawAxP();
            if (projection == "iso")
                drawAxIso();
            if (projection == "orto")
                drawAxOrto();

            cur.M = cur.M * Rotation(comboBox1.SelectedItem.ToString(), Convert.ToDouble(numericUpDown8.Value));
            cur_cent.M = cur_cent.M * Rotation(comboBox1.SelectedItem.ToString(), Convert.ToDouble(numericUpDown8.Value));

            if (projection == "persp")
                cur.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                cur.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                cur.drawOrto(ref pictureBox1, pen);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);

            if (projection == "persp")
                drawAxP();
            if (projection == "iso")
                drawAxIso();
            if (projection == "orto")
                drawAxOrto();

            cur.M = cur.M * Reflection(comboBox2.SelectedItem.ToString());
            cur_cent.M = cur_cent.M * Reflection(comboBox2.SelectedItem.ToString());

            if (projection == "persp")
                cur.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                cur.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                cur.drawOrto(ref pictureBox1, pen);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            double a = (double)anum.Value;
            double b = (double)bnum.Value;
            double c = (double)cnum.Value;

            double p1 = Convert.ToDouble(numericUpDown9.Value);
            double p2 = -Convert.ToDouble(numericUpDown10.Value);
            double p3 = Convert.ToDouble(numericUpDown11.Value);
            double x = a + 50; 
            double y = (x - a) / p1 * p2 + b;
            double z = (x - a) / p1 * p3 + c;

            pen.Color = Color.Red;
            line = new Edge3DAF(new Point3DAF(0, 0, 0), new Point3DAF(x, y, z));
            line.M = line.M * Transition(xcent+a, ycent-b, 0+c);

            if (projection == "persp")
                line.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                line.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                line.drawOrto(ref pictureBox1, pen);
            pen.Color = Color.Black;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            projection = "persp";
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            drawAxP();
            if (cur != null)
                cur.drawP(ref pictureBox1, pen);
            if (line != null)
            {
                pen.Color = Color.Red;
                line.drawP(ref pictureBox1, pen);
                pen.Color = Color.Black;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            projection = "orto";
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            drawAxOrto();
            if (cur != null)
                cur.drawOrto(ref pictureBox1, pen);
            if (line != null)
            {
                pen.Color = Color.Red;
                line.drawOrto(ref pictureBox1, pen);
                pen.Color = Color.Black;
            }
        }

        public Polyhedron3DAF Tetra(Polyhedron3DAF cube)
        {
            matr tetr = new matr(0, 0);

            tetr = tetr.expand(new matr(cube.M, 1));
            tetr = tetr.expand(new matr(cube.M, 5));
            tetr = tetr.expand(new matr(cube.M, 5));

            tetr = tetr.expand(new matr(cube.M, 8));
            tetr = tetr.expand(new matr(cube.M, 8));
            tetr = tetr.expand(new matr(cube.M, 11));

            tetr = tetr.expand(new matr(cube.M, 8));
            tetr = tetr.expand(new matr(cube.M, 1));
            tetr = tetr.expand(new matr(cube.M, 11));

            tetr = tetr.expand(new matr(cube.M, 1));
            tetr = tetr.expand(new matr(cube.M, 5));
            tetr = tetr.expand(new matr(cube.M, 11));
            return new Polyhedron3DAF(tetr);

        }

        public Polyhedron3DAF Oct(Polyhedron3DAF cube)
        {
            matr oct = new matr(0, 0);

            //tetr = tetr.expand(new matr(cube.M, 1));
            //tetr = tetr.expand(new matr(cube.M, 5));
            //tetr = tetr.expand(new matr(cube.M, 5));

            //tetr = tetr.expand(new matr(cube.M, 8));
            //tetr = tetr.expand(new matr(cube.M, 8));
            //tetr = tetr.expand(new matr(cube.M, 11));

            //tetr = tetr.expand(new matr(cube.M, 8));
            //tetr = tetr.expand(new matr(cube.M, 1));
            //tetr = tetr.expand(new matr(cube.M, 11));

            //tetr = tetr.expand(new matr(cube.M, 1));
            //tetr = tetr.expand(new matr(cube.M, 5));
            //tetr = tetr.expand(new matr(cube.M, 11));

            return new Polyhedron3DAF(oct);

        }

        private void button10_Click(object sender, EventArgs e)
        {
            double stx = Convert.ToDouble(numericUpDown1.Value);
            double sty = Convert.ToDouble(numericUpDown2.Value);
            double stz = Convert.ToDouble(numericUpDown3.Value);
            int length = Convert.ToInt32(Convert.ToDouble(numericUpDown4.Value) / Math.Sqrt(2));

            cur = Tetra(CreateCube(xcent + stx, ycent + sty, stz, length));
            cur.M.ToFile();
            if (projection == "persp")
                cur.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                cur.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                cur.drawOrto(ref pictureBox1, pen);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            double stx = Convert.ToDouble(numericUpDown1.Value);
            double sty = Convert.ToDouble(numericUpDown2.Value);
            double stz = Convert.ToDouble(numericUpDown3.Value);
            int length = Convert.ToInt32(Convert.ToDouble(numericUpDown4.Value) * Math.Sqrt(2));

            cur = Oct(CreateCube(xcent + stx, ycent + sty, stz, length));
            cur.M.ToFile();
            if (projection == "persp")
                cur.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                cur.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                cur.drawOrto(ref pictureBox1, pen);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            if (projection == "persp")
                drawAxP();
            if (projection == "iso")
                drawAxIso();
            if (projection == "orto")
                drawAxOrto();


            double angle = Convert.ToDouble(numericUpDown12.Value);

            double x = cur_cent.M.m[0, 0] - xcent;
            double y = -(ycent - cur_cent.M.m[0, 1]);
            double z = cur_cent.M.m[0, 2];
            cur.M *= Transition(-x, -y, -z);

            if (comboBox4.SelectedItem.ToString() == "X")
            {
                cur.M *= Rotation("X", angle);
            }
            if (comboBox4.SelectedItem.ToString() == "Y")
            {
                cur.M *= Rotation("Y", angle);
            }
            if (comboBox4.SelectedItem.ToString() == "Z")
            {
                cur.M *= Rotation("Z", angle);
            }

            cur.M *= Transition(x, y, z);
            if (projection == "persp")
                cur.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                cur.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                cur.drawOrto(ref pictureBox1, pen);

        }

        private void button13_Click(object sender, EventArgs e)
        {
            cur = new Polyhedron3DAF();
            openFileDialog1.ShowDialog();
            string fname = openFileDialog1.FileName;
            Polygon3DAF pol = new Polygon3DAF();
            Point3DAF stp = new Point3DAF(0, 0, 0);
            Point3DAF p1 = new Point3DAF(0, 0, 0);
            Point3DAF p2 = new Point3DAF(0, 0, 0);
            Edge3DAF edg;
            using (StreamReader reader = new StreamReader(File.Open(fname, FileMode.Open)))
            {
                string text;
                bool first = true;
                text = reader.ReadLine();
                string[] bits = text.Split(';');
                cur_cent = new Point3DAF(Convert.ToDouble(bits[0]), Convert.ToDouble(bits[1]), Convert.ToDouble(bits[2]));
                while ((text = reader.ReadLine()) != null)
                {
                    if (text == "f")
                    {
                        edg = new Edge3DAF(p1, stp);
                        pol.Add(edg);

                        cur.Add(pol);
                        pol = new Polygon3DAF();
                        first = true;
                    }
                    else
                    {
                        bits = text.Split(';');
                        if (first)
                        {
                            p1 = new Point3DAF(Convert.ToDouble(bits[0]), Convert.ToDouble(bits[1]), Convert.ToDouble(bits[2]));
                            stp = new Point3DAF(p1);
                            first = false;
                        }
                        else
                        {
                            p2 = new Point3DAF(Convert.ToDouble(bits[0]), Convert.ToDouble(bits[1]), Convert.ToDouble(bits[2]));
                            edg = new Edge3DAF(p1, p2);
                            pol.Add(edg);
                            p1 = new Point3DAF(p2);
                        }
                    }

                }

            }
            cur_cent.M *= Transition(xcent, ycent, 0);
            cur.M *= Transition(xcent, ycent, 0);
            if (projection == "persp")
                cur.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                cur.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                cur.drawOrto(ref pictureBox1, pen);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            button8_Click(sender, e);
            openFileDialog2.ShowDialog();
            string fname = openFileDialog2.FileName;
            Point3DAF p1 = new Point3DAF(0, 0, 0);
            Point3DAF p2 = new Point3DAF(0, 0, 0);
            Edge3DAF edg = new Edge3DAF(p1, p2);
            cur = new Polyhedron3DAF();
            matr pts = new matr(0, 0);
            string ax = comboBox5.SelectedItem.ToString();

            Polygon3DAF pol = new Polygon3DAF();
            using (StreamReader reader = new StreamReader(File.Open(fname, FileMode.Open)))
            {
                string text;
                while ((text = reader.ReadLine()) != null)
                {
                    pts = pts.expand(new matr(text, 1, 2));
                }
            }


            if (comboBox5.SelectedItem.ToString() == "X")
            {
                double xc = pts.midEl2();
                double zc = 0;
                double yc = 0;
                cur_cent = new Point3DAF(xc, yc, zc);
                p1 = new Point3DAF(pts.m[0, 1], 0, pts.m[0, 0]);

                for (int i = 1; i < pts.height; ++i)
                {
                    p2 = new Point3DAF(pts.m[i, 1], 0, pts.m[i, 0]);
                    edg = new Edge3DAF(p1, p2);
                    pol.Add(edg);
                    p1 = new Point3DAF(p2);
                }

                cur.Add(pol);
                cur_cent.M *= Transition(xcent, ycent, 0);
                cur.M *= Transition(xcent, ycent, 0);
            }

            if (comboBox5.SelectedItem.ToString() == "Y")
            {
                double xc = 0;
                double zc = 0;
                double yc = pts.midEl2();
                cur_cent = new Point3DAF(xc, yc, zc);
                p1 = new Point3DAF(pts.m[0, 0], pts.m[0, 1], 0);

                for (int i = 1; i < pts.height; ++i)
                {
                    p2 = new Point3DAF(pts.m[i, 0], pts.m[i, 1], 0);
                    edg = new Edge3DAF(p1, p2);
                    pol.Add(edg);
                    p1 = new Point3DAF(p2);
                }

                cur.Add(pol);
                cur_cent.M *= Transition(xcent, ycent, 0);
                cur.M *= Transition(xcent, ycent, 0);
            }

            if (comboBox5.SelectedItem.ToString() == "Z")
            {
                double xc = 0;
                double zc = pts.midEl2();
                double yc = 0;
                cur_cent = new Point3DAF(xc, yc, zc);
                p1 = new Point3DAF(0, pts.m[0, 0], pts.m[0, 1]);

                for (int i = 1; i < pts.height; ++i)
                {
                    p2 = new Point3DAF(0, pts.m[i, 0], pts.m[i, 1]);
                    edg = new Edge3DAF(p1, p2);
                    pol.Add(edg);
                    p1 = new Point3DAF(p2);
                }

                cur.Add(pol);
                cur_cent.M *= Transition(xcent, ycent, 0);
                cur.M *= Transition(xcent, ycent, 0);
            }

            double ang = 360 / (double)numericUpDown13.Value;

            Polyhedron3DAF p = new Polyhedron3DAF(cur);

            for (int i = 0; i < numericUpDown13.Value; ++i)
            {
                cur.M = cur.M.expand(p.M *= Rotation(ax, ang));
            }

            int c = cur.M.height;

            for (int i = 0; i < c - 2 * pts.height + 2; ++i)
            {
                cur.M = cur.M.expand(new matr(cur.M, i, i + 2 * pts.height - 2));
            }

            if (projection == "persp")
                cur.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                cur.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                cur.drawOrto(ref pictureBox1, pen);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            if (projection == "persp")
                drawAxP();
            if (projection == "iso")
                drawAxIso();
            if (projection == "orto")
                drawAxOrto();
            double angle = (double)ang.Value *  Math.PI / 180;

            double a = (double)anum.Value;
            double b = (double)bnum.Value;
            double c = (double)cnum.Value;

            double l = Convert.ToDouble(numericUpDown9.Value);
            double m = -Convert.ToDouble(numericUpDown10.Value);
            double n = Convert.ToDouble(numericUpDown11.Value);
            
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            double e1 = l * l + cos * (1 - l * l);
            double e2 = l * (1 - cos) * m + n * sin;
            double e3 = l * (1 - cos) * n - m * sin;

            double e4 = l * (1 - cos) * m - n * sin;
            double e5 = m * m + cos * (1 - m * m);
            double e6 = m * (1 - cos) * n + l * sin;

            double e7 = l * (1 - cos) * n + m * sin;
            double e8 = m * (1 - cos) * n - l * sin;
            double e9 = n * n + cos * (1 - n * n);

            double Tx = -xcent;
            double Ty = -ycent;
            double Tz = 0;
            matr res = new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" 
                + Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);

            res*= new matr(e1.ToString() + ";" + e2.ToString() + ";" + e3.ToString() + ";0;" + 
                e4.ToString() + ";" + e5.ToString() + ";" + e6.ToString() + ";0;" + e7.ToString() 
                + ";" + e8.ToString() + ";" + e9.ToString() + ";0;0;0;0;1", 4, 4);

            Tx = xcent;
            Ty = ycent;

            res = res * new matr("1;0;0;0;0;1;0;0;0;0;1;0;" + Tx.ToString() + ";" 
                + Ty.ToString() + ";" + Tz.ToString() + ";1", 4, 4);

            cur.M *= res;
            line.M *= res;
            if (projection == "persp")
            {
                cur.drawP(ref pictureBox1, pen);
                pen.Color = Color.Red;
                line.drawP(ref pictureBox1, pen);
                pen.Color = Color.Black;
            }
            if (projection == "iso")
            {
                cur.drawIso(ref pictureBox1, pen);
                pen.Color = Color.Red;
                line.drawIso(ref pictureBox1, pen);
                pen.Color = Color.Black;
            }
            if (projection == "orto")
            {
                cur.drawOrto(ref pictureBox1, pen);
                pen.Color = Color.Red;
                line.drawOrto(ref pictureBox1, pen);
                pen.Color = Color.Black;
            }
        }

        double linear1(double x, double y)
        {
            return 2 * x + 2 * y;
        }

        double linear2(double x, double y)
        {
            return (x * x + y * y) / 10;
        }

        double linear3(double x, double y)
        {
            return (x * x * x + y) / 100;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            double p0 = (double)numericUpDown14.Value;
            double p1 = (double)numericUpDown15.Value;
            List<Polygon3DAF> Polygons = new List<Polygon3DAF>();

            double delt = (p1 - p0) / (double)(numericUpDown16.Value);
            Funct F = linear1;

            switch(comboBox6.SelectedItem.ToString())
            {
                case "2x + 2y = z":
                    F = linear1;
                    break;

                case "x^2 + y^2 = z":
                    F = linear2;
                    break;

                case "x^3 + y = z":
                    F = linear3;
                    break;
            }

            for (double x = p0; x < p1; x += delt)
            {
                for (double y = p0; y < p1; y += delt)
                {
                    matr M = new matr(0,0);
                    M = M.expand(new matr(x, y, F(x, y)));
                    M = M.expand(new matr(x, y + delt, F(x, y + delt)));
                    M = M.expand(new matr(x + delt, y, F(x + delt, y)));
                    M = M.expand(new matr(x + delt, y + delt, F(x + delt, y + delt)));

                    M = M.expand(new matr(x, y, F(x, y)));
                    M = M.expand(new matr(x + delt, y, F(x + delt, y)));
                    M = M.expand(new matr(x, y + delt, F(x, y + delt)));
                    M = M.expand(new matr(x + delt, y + delt, F(x + delt, y + delt)));
                    Polygons.Add(new Polygon3DAF(M));
                }
            }
            cur = new Polyhedron3DAF();
            matr res = new matr(0, 0);
            for (int i = 0; i < Polygons.Count; ++i) 
            {
                res = res.expand(Polygons[i].M);
            }
            cur.M = new matr(res);
            

            double xc = (p1 - p0) / 2;
            double yc = xc;
            cur_cent = new Point3DAF(xc, yc, cur.M.midEl3());

            cur.M *= Transition(xcent, ycent, 0);
            cur_cent.M *= Transition(xcent, ycent, 0);

            pen = new Pen(Color.Black, 1);
            if (projection == "persp")
                cur.drawP(ref pictureBox1, pen);
            if (projection == "iso")
                cur.drawIso(ref pictureBox1, pen);
            if (projection == "orto")
                cur.drawOrto(ref pictureBox1, pen);
            pen = new Pen(Color.Black, 2);
        }
    }
}

