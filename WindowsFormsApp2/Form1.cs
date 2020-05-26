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

        private PictureBox[] CTLS = new PictureBox[10];
        private IntPtr[] Mems = new IntPtr[10];

        private int W;
        private int H;

        public BlockingCollection<IntPtr> bcTasks = new BlockingCollection<IntPtr>(); 

        private void Button1_Click(object sender, EventArgs e)
        {
            IntPtr tmp = Marshal.AllocCoTaskMem(W * H * 3);

            for (int i = 0; i < 10; i++)
            {

            }


            Marshal.FreeCoTaskMem(tmp);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i = 0;
            foreach(Control CTL in Controls)
            {

                if (CTL is PictureBox x && i < 10)
                {
                    CTLS[i] = (PictureBox)CTL;
                    i += 1;
                }
            }

            W = pictureBox1.Width;
            H = pictureBox1.Height;

        }

        private Bitmap BuildImage(Bitmap src,Bitmap tmp)
        {
            int tmp_W = src.Width;
            int tmp_H = src.Height;

            Bitmap bmp = new Bitmap(tmp_W,tmp_H,PixelFormat.Format24bppRgb);

            return bmp;
        }
    }
}
