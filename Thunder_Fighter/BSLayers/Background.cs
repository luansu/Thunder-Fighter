using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    class GBackground
    {
        public Image bg1 = Image.FromFile(Resource.BG);
        public Image bg2 = Image.FromFile(Resource.BG);

        public int scrollSpeed = 1;
        public int bgY1 = 0;
        public int bgY2 = 0;

        public GBackground()
        {
            this.bgY1 = 0;
            this.bgY2 = - bg2.Height;
        }

        public void update(int width, int height)
        {
            this.bgY1 += this.scrollSpeed;
            this.bgY2 += this.scrollSpeed;

            // Nếu ảnh đã chạy khỏi màn hình thì đưa nó lên đầu
            if (this.bgY1 >=  height)
                this.bgY1 = this.bgY2 - this.bg2.Height;

            if (this.bgY2 >= height)
                this.bgY2 = this.bgY1 - this.bg1.Height;
        }

        public void paint(ref Graphics g, int width, int height)
        {
            g.DrawImage(bg1, new Rectangle(0, this.bgY1, this.bg1.Width, this.bg1.Height));
            g.DrawImage(bg2, new Rectangle(0, this.bgY2, this.bg2.Width, this.bg2.Height));
        }
    }
}
