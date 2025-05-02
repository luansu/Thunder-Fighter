using System;
using System.Collections.Generic;
using System.Drawing;

namespace Thunder_Fighter.BSLayers
{
    abstract class Enemy : IObject
    {
        public int x, y, w, h;
        public int health;
        public Sprite sprite;
        public int dx = 2, dy = 2;
        public int minY = 0, maxY = 200;
        public int movementMode = 0;

        public bool isDead = false;
        public bool isExploding = false;
        public int explosionFrame = 0;
        public Sprite explosionSprite;

        public List<Dart> bullets = new List<Dart>();
        protected int fireCooldown = 0;
        protected int fireInterval = 60;
        protected int animationCounter = 0;

        public Enemy(int x, int y, int w, int h, int health, Sprite sprite)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.health = health;
            this.sprite = sprite;
        }

        public abstract int GetEnemyType();

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
        }

        public virtual void FireBullet()
        {
            if (fireCooldown == 0)
            {
                int bulletW = 20, bulletH = 40;
                int bulletX = x + (w - bulletW) / 2;
                int bulletY = y + h / 2 - bulletH / 2;

                bullets.Add(new Dart(bulletX, bulletY, GetEnemyType()));
                fireCooldown = fireInterval;
            }
        }

        public virtual void Update()
        {
            if (isExploding)
            {
                explosionFrame++;
                if (explosionSprite != null && explosionFrame >= explosionSprite.bitmaps.Count)
                    isExploding = false;
                return;
            }

            x += dx;
            if (movementMode == 0) y += dy;

            if (x <= 0 || x + w >= 360 * 3 / 2) dx = -dx;
            if (movementMode == 0 && (y <= minY || y + h >= maxY)) dy = -dy;

            if (fireCooldown > 0) fireCooldown--;
            FireBullet();

            foreach (var b in bullets)
                b.update();

            bullets.RemoveAll(b => b.y > 1000);
        }

        public virtual void Paint(ref Graphics g)
        {
            if (isExploding && explosionSprite != null)
            {
                explosionSprite.index = explosionFrame;
                explosionSprite.Draw(ref g, x, y, w, h);
            }
            else if (sprite != null && sprite.bitmaps.Count > 0)
            {
                animationCounter++;
                if (animationCounter % 2 == 0)
                {
                    sprite.index = (sprite.index + 1) % sprite.bitmaps.Count;
                }

                sprite.Draw(ref g, x, y, w, h);
            }

            foreach (var b in bullets)
                b.Paint(ref g);
        }

        public int getX() => x;
        public int getY() => y;
        public int getW() => w;
        public int getH() => h;
    }
}
