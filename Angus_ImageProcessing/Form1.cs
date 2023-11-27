using System.Collections.Generic;
using WebCamLib;
using HNUDIP;
using AForge.Video;
using AForge.Video.DirectShow;


namespace Angus_ImageProcessing
{
    public partial class Form1 : Form
    {
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;

        Bitmap loadedImg, processedImg, bgImg;
        bool webcam_on = false;

        Device[] devices = DeviceManager.GetAllDevices();
        Device webcam = DeviceManager.GetDevice(0);
        webCamMode mode = webCamMode.COPY;
        System.Windows.Forms.Timer t;


        enum webCamMode
        {
            COPY,
            GREYSCALE,
            INVERSION,
            HISTOGRAM,
            SEPIA,
            SUBTRACT
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "webcam: off";
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                comboBox1.Items.Add(filterInfo.Name);
            }
            comboBox1.SelectedIndex = 0;
            videoCaptureDevice = new VideoCaptureDevice();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loadedImg = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loadedImg;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = webCamMode.COPY;
            if (webcam_on)
            {
                return;
            }
            processedImg = new Bitmap(loadedImg.Width, loadedImg.Height);
            for (int x = 0; x < loadedImg.Width; x++)
            {
                for (int y = 0; y < loadedImg.Height; y++)
                {
                    Color pixel = loadedImg.GetPixel(x, y);
                    processedImg.SetPixel(x, y, pixel);
                }
            }
            pictureBox2.Image = processedImg;
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = webCamMode.GREYSCALE;
            if (webcam_on)
            {
                return;
            }
            processedImg = new Bitmap(loadedImg.Width, loadedImg.Height);
            for (int x = 0; x < loadedImg.Width; x++)
            {
                for (int y = 0; y < loadedImg.Height; y++)
                {
                    Color pixel = loadedImg.GetPixel(x, y);
                    int greyValue = (pixel.R + pixel.G + pixel.B) / 3;
                    Color greyShade = Color.FromArgb(greyValue, greyValue, greyValue);
                    processedImg.SetPixel(x, y, greyShade);
                }
            }
            pictureBox2.Image = processedImg;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = webCamMode.INVERSION;
            if (webcam_on)
            {
                return;
            }
            processedImg = new Bitmap(loadedImg.Width, loadedImg.Height);
            for (int x = 0; x < loadedImg.Width; x++)
            {
                for (int y = 0; y < loadedImg.Height; y++)
                {
                    Color pixel = loadedImg.GetPixel(x, y);
                    processedImg.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                }
            }
            pictureBox2.Image = processedImg;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = webCamMode.HISTOGRAM;
            if (webcam_on)
            {
                return;
            }
            processedImg = new Bitmap(loadedImg.Width, loadedImg.Height);
            for (int x = 0; x < loadedImg.Width; x++)
            {
                for (int y = 0; y < loadedImg.Height; y++)
                {
                    Color pixel = loadedImg.GetPixel(x, y);
                    int greyValue = (pixel.R + pixel.G + pixel.B) / 3;
                    Color greyShade = Color.FromArgb(greyValue, greyValue, greyValue);
                    processedImg.SetPixel(x, y, greyShade);
                }
            }
            Color sample;
            int[] hist = new int[256];
            for (int x = 0; x < loadedImg.Width; x++)
            {
                for (int y = 0; y < loadedImg.Height; y++)
                {
                    sample = processedImg.GetPixel(x, y);
                    hist[sample.R]++;
                }
            }
            Bitmap hgraph = new Bitmap(256, 800);
            int h = hgraph.Height - 1;
            for (int x = 0; x < 256; x++)
            {
                int limit = (int)Math.Min(799, hist[x] / 5);
                limit = h - limit;
                int y;
                for (y = h; y >= limit; y--)
                {
                    hgraph.SetPixel(x, y, Color.Black);
                }
                for (; y >= 0; y--)
                {
                    hgraph.SetPixel(x, y, Color.White);
                }
            }
            pictureBox2.Image = hgraph;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = webCamMode.SEPIA;
            if (webcam_on)
            {
                return;
            }
            processedImg = new Bitmap(loadedImg.Width, loadedImg.Height);
            for (int x = 0; x < loadedImg.Width; x++)
            {
                for (int y = 0; y < loadedImg.Height; y++)
                {
                    Color pixel = loadedImg.GetPixel(x, y);
                    int outRed = (int)Math.Min(255, (pixel.R * 0.393) + (pixel.G * 0.769) + (pixel.B * 0.189));
                    int outGreen = (int)Math.Min(255, (pixel.R * .349) + (pixel.G * .686) + (pixel.B * .168));
                    int outBlue = (int)Math.Min(255, (pixel.R * .272) + (pixel.G * .534) + (pixel.B * .131));
                    processedImg.SetPixel(x, y, Color.FromArgb(outRed, outGreen, outBlue));
                }
            }
            pictureBox2.Image = processedImg;
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            processedImg.Save(saveFileDialog1.FileName);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bgImg = new Bitmap(openFileDialog2.FileName);
            pictureBox3.Image = bgImg;
        }

        private void subtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = webCamMode.SUBTRACT;
            if (webcam_on)
            {
                return;
            }
            Color mygreen = Color.FromArgb(0, 255, 0);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            processedImg = new Bitmap(loadedImg.Width, loadedImg.Height);
            for (int x = 0; x < loadedImg.Width; x++)
            {
                for (int y = 0; y < loadedImg.Height; y++)
                {
                    Color pixel = loadedImg.GetPixel(x, y);
                    Color backpixel = bgImg.GetPixel(x, y);
                    int greyValue = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractValue = Math.Abs(greyValue - greygreen);
                    if (subtractValue > threshold)
                    {
                        processedImg.SetPixel(x, y, pixel);
                    }
                    else
                    {
                        processedImg.SetPixel(x, y, backpixel);
                    }
                }
            }
            pictureBox2.Image = processedImg;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            webcam_on = !webcam_on;
            button1.Enabled = !webcam_on;
            comboBox1.Enabled = button1.Enabled;
            if (webcam_on)
            {
                label1.Text = "webcam: on";
                videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[comboBox1.SelectedIndex].MonikerString);
                videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
                videoCaptureDevice.Start();

            }
            else
            {
                if (videoCaptureDevice.IsRunning)
                {
                    try
                    {
                        videoCaptureDevice.SignalToStop();
                    }
                    catch (Exception)
                    {

                        
                    }
                    
                }
                label1.Text = "webcam: off";
            }



        }
        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap b = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = (Bitmap)b.Clone();
            switch (mode)
            {
                case webCamMode.COPY:
                    pictureBox2.Image = b;
                    break;
                case webCamMode.GREYSCALE:
                    ImageProcess2.BitmapFilter.GrayScale(b);
                    pictureBox2.Image = b;
                    break;
                case webCamMode.INVERSION:
                    ImageProcess2.BitmapFilter.Invert(b);
                    pictureBox2.Image = b;
                    break;
                case webCamMode.HISTOGRAM:
                    ImageProcess.Histogram(ref b, ref processedImg);
                    pictureBox2.Image = processedImg;
                    break;
                case webCamMode.SUBTRACT:
                    ImageProcess2.BitmapFilter.Subtract(b, bgImg, Color.Green, 100);
                    pictureBox2.Image = b;
                    break;
                case webCamMode.SEPIA:
                    processedImg = new Bitmap(b.Width, b.Height);
                    for (int x = 0; x < b.Width; x++)
                    {
                        for (int y = 0; y < b.Height; y++)
                        {
                            Color pixel = b.GetPixel(x, y);
                            int outRed = (int)Math.Min(255, (pixel.R * 0.393) + (pixel.G * 0.769) + (pixel.B * 0.189));
                            int outGreen = (int)Math.Min(255, (pixel.R * .349) + (pixel.G * .686) + (pixel.B * .168));
                            int outBlue = (int)Math.Min(255, (pixel.R * .272) + (pixel.G * .534) + (pixel.B * .131));
                            processedImg.SetPixel(x, y, Color.FromArgb(outRed, outGreen, outBlue));
                        }
                    }
                    pictureBox2.Image = processedImg;
                  
                    break;
            }
        }
    }
}