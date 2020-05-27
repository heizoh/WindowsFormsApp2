using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public PictureBox[] CTLS = new PictureBox[10];
        public IntPtr[] Mems = new IntPtr[10];

        private int W;
        private int H;

        public BlockingCollection<IntPtr> bcTasks = new BlockingCollection<IntPtr>();

        private void Button1_Click(object sender, EventArgs e)
        {
            IntPtr tmp = Marshal.AllocCoTaskMem(W * H * 3);
            ClassLibrary1.Class1 CL = new ClassLibrary1.Class1();
            Rectangle rect = new Rectangle(0, 0, W, H);

            for (int i = 0; i < 10; i++)
            {
                Bitmap bm = (Bitmap)CTLS[i].Image;
                BitmapData b = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                CL.MoveMemory(b.Scan0, ref tmp, (uint)(W * H * 3));
                Mems[i] = Marshal.AllocCoTaskMem(W * H * 3);
                CL.MoveMemory(tmp, ref Mems[i], (uint)(W * H * 3));
                bm.UnlockBits(b);
            }

            Marshal.FreeCoTaskMem(tmp);

            var P = Task.Run(() =>
            {

                Random w = new Random();

                for (int i = 0; i < 10; i++)
                {
                    bcTasks.Add(Mems[i]);
                    Console.WriteLine($"{i + 1}番目タスク登録");
                    Console.WriteLine($"{(int)(5 * w.NextDouble())}");
                    if ((int)(10 * w.NextDouble()) > 2)
                    {
                        Console.WriteLine("Wait...");
                        System.Threading.Thread.Sleep(5);
                    }
                }
            });

            var C = Task.Run(() =>
            {

                using (Graphics G = Graphics.FromImage(pictureBox12.Image))
                {
                    using (Graphics G2 = Graphics.FromImage(pictureBox3.Image))
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            IntPtr data = bcTasks.Take();
                            Bitmap tmpBM = new Bitmap(W, H, PixelFormat.Format24bppRgb);
                            BitmapData tmpBD = tmpBM.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                            IntPtr t = tmpBD.Scan0;
                            CL.MoveMemory(data, ref t, (uint)(W * H * 3));
                            tmpBM.UnlockBits(tmpBD);

                            G.DrawImage(tmpBM, W * (i % 5), H * (i / 5), W, H);


                            tmpBD = tmpBM.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                            t = tmpBD.Scan0;
                            CL.MoveMemory(data, ref t, (uint)(W * H * 3));
                            tmpBM.UnlockBits(tmpBD);

                            G2.DrawImage(tmpBM, W * (i % 3), H * (i / 3), W, H);

                            Console.WriteLine($"{i + 1}番目タスク処理");
                        }
                    }
                }

                Invoke((MethodInvoker)delegate
                {
                    pictureBox12.Refresh();
                    pictureBox3.Refresh();
                });
            });



            //Marshal.FreeCoTaskMem(tmp);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i = 0;
            foreach(Control CTL in Controls)
            {

                if (CTL is PictureBox x && i < 10)
                {
                    CTLS[i] = (PictureBox)CTL;
                    if (CTLS[i].Image != null)
                    {
                        i += 1;
                    }
                }
            }

            W = pictureBox1.Width;
            H = pictureBox1.Height;
            pictureBox12.Image = new Bitmap(W * 5, H * 2);
            pictureBox3.Image = new Bitmap(W * 3, H * 4);
        }

    }
}
