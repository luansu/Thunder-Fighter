using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    class Dart : IObject
    {
        public int type;
        public int x;
        public int y;
        public int w;
        public int h;
        public string name;
        public float speed;
        public float damage;
        public Sprite dartSprite;
        public int coolDown = 60;

        public bool isFire = false;
        public Dart(string name, float speed, float damage, int type)
        {
            this.name = name;
            this.speed = speed;
            this.damage = damage;
            this.type = type;
            this.loadDart();
        }

        private void loadDart()
        {
            if (this.type == 0)
            {
                this.coolDown = 60;
                this.speed = 20;
            }
            else if(this.type == 1)
            {
                this.coolDown = 120;
                this.speed = 10;
                this.w = Fighter.w * 2 / 3;
                this.h = Fighter.h * 2 / 3;
                this.x = Fighter.x + Fighter.w / 2 - this.w / 2;
                this.y = Fighter.y;
            }
            else if (this.type == 2)
            {
                this.coolDown = 20;
                this.speed = 25;
            }
            else if (this.type == 3)
            {
                this.coolDown = 0;
            }
            dartSprite = new Sprite();
            dartSprite.LoadGif(Resource.ENGINE_DART[this.type]);
            this.w = Fighter.w * 2 / 3;
            this.h = Fighter.h * 2 / 3;
            this.x = Fighter.x + Fighter.w / 2 - this.w / 2;
            this.y = Fighter.y;

        }

        public void update()
        {
            if (this.isFire)
            {
                this.y -= (int)this.speed;
            }
        }

        public void paint(ref Graphics g)
        {
            if (Form1.frameCount % 2 == 0) this.dartSprite.index++;
            if (this.isFire)
            {
                dartSprite.Draw(ref g, this.x, this.y, this.w, this.h);
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
