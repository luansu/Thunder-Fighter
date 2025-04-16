using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    class Enemy : IObject
    {
        public int type; // 0: nhỏ, 1: lớn, 2: boss
        public int x, y, w, h;
        public int health;
        public Sprite sprite;
        public int dx = 2; // hướng ngang
        public int dy = 2; // hướng dọc
        public int minY = 0; // phạm vi di chuyển Y tối đa (giới hạn phía trên)
        public int maxY = 200;
        public int movementMode = 0; // 0: zigzag, 1: horizontal

        public Enemy(int type, int x, int y, int w, int h, int health, Sprite sprite)
        {
            this.type = type;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.health = health;
            this.sprite = sprite;
        }

        public void update()
        {
            x += dx;
            if (movementMode == 0)
                y += dy;

            if (x <= 0 || x + w >= 360 * 3 / 2)
                dx = -dx;

            if (movementMode == 0)
            {
                if (y <= minY || y + h >= maxY)
                    dy = -dy;
            }
        }

        public void paint(ref Graphics g)
        {
            sprite.Draw(ref g, x, y, w, h);
        }

        public int getX() => x;
        public int getY() => y;
        public int getW() => w;
        public int getH() => h;
    }
}
