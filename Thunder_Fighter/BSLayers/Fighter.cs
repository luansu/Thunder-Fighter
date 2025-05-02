using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    class Fighter : IObject
    {
        public int type;
        public string name;
        public static int x;
        public static int y;
        public static int w;
        public static int h;
        public int mHealth;
        public int mStamina;
        public int health;
        public int stamina;
        public bool isProtected = false;
        public Engine engine;
        public Sprite baseSprite;
        public Sprite shieldSprite;
        public int status = 0; // 0: 100%, 1: 75%, 2: 50%, 3: 25%, 4: 0%

        public Fighter(string name, int health, int stamina)
        {
            this.name = name;
            this.mHealth = health;
            this.mStamina = stamina;
            this.health = 99;
            this.stamina = stamina;
        }

        public void update() 
        {
            engine.update();

            float percentH = (float) this.health / this.mHealth;
            if(percentH > 0.75)
            {
                this.status = 0;
            }
            else if (percentH > 0.5)
            {
                this.status = 1;
            }
            else if (percentH > 0.25)
            {
                this.status = 2;
            }
            else if (percentH > 0)
            {
                this.status = 3;
            }
            else
            {
                this.status = 4;
            }
        }

        public void paint(ref Graphics g)
        {
            engine.paint(ref g);
            if(this.status < 4)
            {
                baseSprite.index = status;
                baseSprite.Draw(ref g, Fighter.x, Fighter.y, Fighter.w, Fighter.h);
            }
            if (isProtected)
            {
                if (Form1.frameCount % 2 == 0) shieldSprite.index++;
                shieldSprite.Draw(ref g, Fighter.x, Fighter.y, Fighter.w, Fighter.h);
            }
        }

        public void TakeDamage(float dmg)
        {
            this.health -= (int)dmg;
            if (this.health < 0) this.health = 0;
        }

        public void move(int dx, int dy)
        {
            Fighter.x += dx;
            Fighter.y += dy;
        }

        public void getHit()
        {
            this.health -= 100;
        }

        public bool isGetHit()
        {
            return false;
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
