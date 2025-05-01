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
        public int health;
        public int stamina;
        public bool isProtected = false;
        public Engine engine;
        public Sprite baseSprite;
        public Sprite shieldSprite;

        public Fighter(string name, int health, int stamina)
        {
            this.name = name;
            this.health = health;
            this.stamina = stamina;
        }

        public void update() 
        {
            engine.update();
        }

        public void paint(ref Graphics g)
        {
            engine.paint(ref g);
            baseSprite.Draw(ref g, Fighter.x, Fighter.y, Fighter.w, Fighter.h);
            if (isProtected)
            {
                if (Form1.frameCount % 2 == 0) shieldSprite.index++;
                shieldSprite.Draw(ref g, Fighter.x, Fighter.y, Fighter.w, Fighter.h);
            }
        }
        public void move(int dx, int dy)
        {
            Fighter.x += dx;
            Fighter.y += dy;
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
