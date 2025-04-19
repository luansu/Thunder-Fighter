using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Thunder_Fighter.BSLayers
{
    abstract class Enemy : IObject
    {
        public int x, y, w, h;
        public int health;
        public Sprite sprite;
        public int dx = 2;
        public int dy = 2;
        public int minY = 0;
        public int maxY = 200;
        public int movementMode = 0;

        public bool isDead = false;
        public bool isExploding = false;
        public int explosionFrame = 0;
        public Sprite explosionSprite;

        public Enemy(int x, int y, int w, int h, int health, Sprite sprite)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.health = health;
            this.sprite = sprite;
        }

        public abstract int GetEnemyType(); // 0: small, 1: big, 2: boss

        public virtual void TakeDamage(float dmg)
        {
            health -= (int)dmg;
            if (health <= 0 && !isDead)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            isDead = true;
            isExploding = true;
            explosionFrame = 0;
            // explosionSprite will be loaded in subclass
        }

        public virtual void Update()
        {
            if (isExploding)
            {
                explosionFrame++;
                if (explosionSprite != null && explosionFrame >= explosionSprite.bitmaps.Count)
                {
                    isExploding = false;
                }
                return;
            }

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

        public virtual void Paint(ref Graphics g)
        {
            if (isExploding && explosionSprite != null)
            {
                explosionSprite.index = explosionFrame;
                explosionSprite.Draw(ref g, x, y, w, h);
            }
            else
            {
                sprite.Draw(ref g, x, y, w, h);
            }
        }

        public int getX() => x;
        public int getY() => y;
        public int getW() => w;
        public int getH() => h;
    }
}
