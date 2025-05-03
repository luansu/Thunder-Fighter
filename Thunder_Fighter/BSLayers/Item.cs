using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    class Item : IObject
    {
        public int type;
        public int x;
        public int y;
        public int w = 64;
        public int h = 64;
        public string name;
        public int health;
        public int stamina;
        public float damage;

        private float velocityX = 2.0f;
        private float velocityY = 2.0f;

        public Sprite itemSprite;
        public int lifetime = 0;
        public int maxLifetime = 9000;

        public Item()
        {
            
        }

        public Item(string name, int type, int x, int y)
        {
            this.name = name;
            this.type = type;
            this.x = x;
            this.y = y;
            this.loadItem();
        }

        //0-3: weapon item, 4-n: orther item
        public void loadItem()
        {
            this.itemSprite = new Sprite();
            if (this.type < 4)
            {
                this.itemSprite.LoadGif(Resource.WEAPON_ITEMS[this.type]);
            }
            else
            {
                this.itemSprite.LoadGif(Resource.ATTRIBUTE_ITEMS[this.type - 4]);
            }
            this.lifetime = this.maxLifetime;
        }

        public void update()
        {
            this.lifetime--;
            this.floating();
        }

        public void paint(ref Graphics g)
        {
            if (this.lifetime > 0)
            {
                if(Form1.frameCount % 2 == 0) this.itemSprite.index++;
                this.itemSprite.Draw(ref g, this.x, this.y, this.w, this.h);
            }
        }

        private void floating()
        {
            if (this.lifetime > 0)
            {
                this.x += (int)this.velocityX;
                this.y += (int)this.velocityY;

                if (this.x <= 0 || this.x + this.w >= Form1.width)
                {
                    this.velocityX *= -1;
                    this.x = Math.Max(0, Math.Min(this.x, Form1.width - this.w));
                }

                if (this.y <= 0 || this.y + this.h >= Form1.height)
                {
                    this.velocityY *= -1;
                    this.y = Math.Max(0, Math.Min(this.y, Form1.height - this.h));
                }
            }
        }

        public bool isCollide(IObject obj)
        {
            return !(this.x + this.w < obj.getX() || this.x > obj.getX() + obj.getW() ||
             this.y + this.h < obj.getY() || this.y > obj.getY() + obj.getH());
        }
        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return y;
        }
        public int getW()
        {
            return w;
        }
        public int getH()
        {
            return h;
        }
    }
}
