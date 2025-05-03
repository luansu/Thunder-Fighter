using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    class GameText
    {
        public string text;
        public int x;
        public int y;

        private int lifetime = 0;

        public GameText()
        {
             
        }

        public void update()
        {

        }

        public void paint(ref Graphics g)
        {
            Font font = new Font("Arial", 20, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.White);
            g.DrawString(this.text, font, brush, this.x, this.y);
            lifetime++;
            if (lifetime > 100)
            {
                this.x = -9999;
                this.y = -9999;
            }
        }

        public static void show(ref Graphics g, string text, int x, int y, int size)
        {
            Font font = new Font(Form1.fontFamily, size);
            Brush brush = new SolidBrush(Color.White);
            g.DrawString(text, font, brush, x, y);
        }
    }
}
