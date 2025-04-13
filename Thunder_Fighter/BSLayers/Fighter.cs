using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    class Fighter : IObject
    {
        public int type;
        public string name;
        public int x;
        public int y;
        public int w;
        public int h;
        public int health;
        public int stamina;
        public Dart dart;
        public Engine engine;
        public Sprite baseSprite;
        public Sprite shieldSprite;

        public Fighter(string name, int health, int stamina)
        {
            this.name = name;
            this.health = health;
            this.stamina = stamina;
        }

        public void update(ref Graphics g) 
        { 

        }

        public void paint(ref Graphics g)
        {
            baseSprite.Draw(ref g, this.x, this.y, this.w, this.h);
            shieldSprite.Draw(ref g, this.x, this.y, this.w, this.h);
        }
        public void move(int dx, int dy)
        {
            this.x += dx;
            this.y += dy;
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
