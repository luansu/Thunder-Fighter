using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Thunder_Fighter.BSLayers
{
    class Sprite
    {
        public string folder;
        public List<string> filenames;
        public List<Bitmap> bitmaps;
        public int index = 0;
        public Sprite()
        {
        }
        public Sprite(string folder)
        {
            this.folder = folder;
            this.filenames = LoadFiles();
            this.bitmaps = filenames.Select(path => new Bitmap(path)).ToList();
        }
        public Sprite(List<string> filenames)
        {
            this.filenames = filenames;
            this.bitmaps = filenames.Select(path => new Bitmap(path)).ToList();
        }

        private List<string> LoadFiles()
        {
            List<string> files = new List<string>();
            string[] fileEntries = Directory.GetFiles(folder);
            foreach (string fileName in fileEntries)
            {
                if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    files.Add(fileName);
                }
            }
            return files;
        }
        public void LoadGif(string gifPath)
        {
            bitmaps = new List<Bitmap>();

            if (!File.Exists(gifPath))
            {
                MessageBox.Show("Unable to load gif at: " + gifPath);
                return;
            }

            try
            {
                Image gifImg = Image.FromFile(gifPath);
                FrameDimension dimension = new FrameDimension(gifImg.FrameDimensionsList[0]);
                int frameCount = gifImg.GetFrameCount(dimension);

                for (int i = 0; i < frameCount; i++)
                {
                    gifImg.SelectActiveFrame(dimension, i);
                    Bitmap frame = new Bitmap(gifImg.Width, gifImg.Height);
                    using (Graphics g = Graphics.FromImage(frame))
                    {
                        g.DrawImage(gifImg, Point.Empty);
                    }
                    bitmaps.Add(frame);
                }

                // Reset index and filenames
                filenames = new List<string> { gifPath };
                index = 0;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error loading GIF: " + ex.Message);
            }
        }

        public void Draw(ref Graphics g, int x, int y, int w, int h)
        {
            if (this.index < 0 || this.index >= bitmaps.Count)
            {
                this.index = 0;
                return;
            }

            try
            {
                g.DrawImage(bitmaps[this.index], new Rectangle(x, y, w, h));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error drawing sprite: " + ex.Message);
            }
        }
    }
}
