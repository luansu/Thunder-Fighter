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
        public static int mHealth;
        public static int mStamina;
        public static int health;
        public static int stamina;
        public bool isProtected = false;
        public Engine engine;
        public Sprite baseSprite;
        public Sprite shieldSprite;
        public static int status = 0; // 0: 100%, 1: 75%, 2: 50%, 3: 25%, 4: 0%
        public StatusBar statusBar = new StatusBar();

        public Fighter(string name, int health, int stamina)
        {
            this.name = name;
            Fighter.mHealth = health;
            Fighter.mStamina = stamina;
            Fighter.health = 99;
            Fighter.stamina = stamina;
        }

        public void update() 
        {
            engine.update();

            float percentH = (float) Fighter.health / Fighter.mHealth;
            if(percentH > 0.75)
            {
                Fighter.status = 0;
            }
            else if (percentH > 0.5)
            {
                Fighter.status = 1;
            }
            else if (percentH > 0.25)
            {
                Fighter.status = 2;
            }
            else if (percentH > 0)
            {
                Fighter.status = 3;
            }
            else
            {
                Fighter.status = 4;
            }
            statusBar.update();
        }

        public void paint(ref Graphics g)
        {
            engine.paint(ref g);
            if(Fighter.status < 4)
            {
                baseSprite.index = status;
                baseSprite.Draw(ref g, Fighter.x, Fighter.y, Fighter.w, Fighter.h);
            }
            if (!isProtected)
            {
                if (Form1.frameCount % 2 == 0) shieldSprite.index++;
                shieldSprite.Draw(ref g, Fighter.x, Fighter.y, Fighter.w, Fighter.h);
            }
            statusBar.paint(ref g);
        }
        public void move(int dx, int dy)
        {
            Fighter.x += dx;
            Fighter.y += dy;
        }

        public void getHit()
        {
            Fighter.health -= 100;
        }

        public bool isGetHit()
        {
            return false;
        }
        public bool isCollide(IObject obj)
        {
            return !(Fighter.x + Fighter.w < obj.getX() || Fighter.x > obj.getX() + obj.getW() ||
             Fighter.y + Fighter.h < obj.getY() || Fighter.y > obj.getY() + obj.getH());
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
