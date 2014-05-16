using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Slicer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            pictureBox1.Paint += pictureBox1_Paint;
            comboBox1.SelectedIndex = 0;
        }

        void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int wx, wy;
            int sx, sy;
            sx = Convert.ToInt32(xSlicesNum.Value) + 1;
            sy = Convert.ToInt32(ySlicesNum.Value) + 1;

            if (sx > 0)
                wx = pictureBox1.Width / sx;
            else
                wx = pictureBox1.Width;

            if (sy > 0)
                wy = pictureBox1.Height / sy;
            else
                wy = pictureBox1.Height;


            var p1 = new Pen(Color.White, 1);
            p1.DashCap = System.Drawing.Drawing2D.DashCap.Flat;
            p1.DashPattern = new float[] { 4.0F, 4.0F };
            var p2 = new Pen(Color.Black, 2);
            p2.DashCap = System.Drawing.Drawing2D.DashCap.Flat;
            p2.DashPattern = new float[] { 4.0F, 4.0F };

            for (int x = 0; x < Convert.ToInt32(xSlicesNum.Value); x++)
            {
                e.Graphics.DrawLine(p2, new Point((x + 1) * wx, 0), new Point((x + 1) * wx, pictureBox1.Height));
                e.Graphics.DrawLine(p1, new Point((x + 1) * wx, 0), new Point((x + 1) * wx, pictureBox1.Height));

            }
            for (int y = 0; y < Convert.ToInt32(ySlicesNum.Value); y++)
            {
                e.Graphics.DrawLine(p2, new Point(0, (y + 1) * wy), new Point(pictureBox1.Width, (y + 1) * wy));
                e.Graphics.DrawLine(p1, new Point(0, (y + 1) * wy), new Point(pictureBox1.Width, (y + 1) * wy));
            }

            p1.Dispose();
            p2.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (pictureBox1.Image != null && fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageFormat format = ImageFormat.Png;
                if (comboBox1.SelectedIndex == 0)
                    format = ImageFormat.Png;
                else if (comboBox1.SelectedIndex == 1)
                    format = ImageFormat.Jpeg;

                Image originalImage = pictureBox1.Image;
                Bitmap originalBitmap = (Bitmap)originalImage;
                int wx, wy;
                int sx, sy;
                sx = Convert.ToInt32(xSlicesNum.Value) + 1;
                sy = Convert.ToInt32(ySlicesNum.Value) + 1;

                if (sx > 0)
                    wx = originalImage.Width / sx;
                else
                    wx = originalImage.Width;

                if (sy > 0)
                    wy = originalImage.Height / sy;
                else
                    wy = originalImage.Height;

                int count = 0;
                for (int x = 0; x < sx; x++)
                {
                    for (int y = 0; y < sy; y++)
                    {
                        Rectangle srcRect = new Rectangle(x * wx, y * wy, wx, wy);
                        Bitmap cropped = (Bitmap)originalBitmap.Clone(srcRect, originalImage.PixelFormat);
                       
                        cropped.Save(fbd.SelectedPath + "\\" + filenameTxt.Text + count + "." + comboBox1.Text, format);
                        count++;
                    }
                }

                if (MessageBox.Show("Process Completed on " + fbd.SelectedPath + ". Open Directory?", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    ProcessStartInfo runExplorer = new ProcessStartInfo();
                    runExplorer.FileName = "explorer.exe";
                    runExplorer.Arguments = fbd.SelectedPath;
                    Process.Start(runExplorer);
                }
            }
            else
            {
                MessageBox.Show("Load image first.");
            }
        }

        private void xSlicesNum_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void ySlicesNum_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void filenameTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
