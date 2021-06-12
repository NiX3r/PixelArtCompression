using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelArtCompression
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                writeLog("Loaded picture from: " + openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String outputFile = openFileDialog1.FileName;
            outputFile = outputFile.Replace(".jpg", "");
            outputFile = outputFile.Replace(".png", "");
            outputFile += ".bmp";
            Bitmap output = CompressionBitmap.Compress((Bitmap)pictureBox1.Image, (int)numericUpDown1.Value, outputFile);
            if (output == pictureBox1.Image)
                writeLog("Compression failed!");
            else
                writeLog("Compression done and saved: " + outputFile);
            pictureBox2.Image = output;
        }

        public void writeLog(String value)
        {
            richTextBox1.Text += "[" + DateTime.Now + "] » " + value + "\n";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                pictureBox1.ImageLocation = textBox1.Text;
                openFileDialog1.FileName = "output";
            }
        }
    }
}
