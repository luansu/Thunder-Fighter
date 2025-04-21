using System;
using System.Drawing;
using System.IO;

namespace Thunder_Fighter.BSLayers
{
    class EnemyBullet : IObject
    {
        public int x, y;
        public int w = 20, h = 40;
        public float speed;
        public float damage;
        public Sprite sprite;
        public int enemyType;
        private int lifetime = 0;
        private int maxLifetime = 90;
        private int animationCounter = 0;

        public EnemyBullet(int startX, int startY, int enemyType)
        {
            this.x = startX;
            this.y = startY;
            this.enemyType = enemyType;

            LoadBulletSprite(enemyType);
            SetBulletStats(enemyType);
        }

        private void LoadBulletSprite(int type)
        {
            string path = type switch
            {
                0 => Path.Combine(Form1.assetPath, "enemy1\\attack\\Kla'ed - Torpedo.gif"),
                1 => Path.Combine(Form1.assetPath, "enemy1\\attack\\Kla'ed - Ray.gif"),
                2 => Path.Combine(Form1.assetPath, "enemy1\\attack\\Kla'ed - Wave.gif"),
                _ => null
            };

            sprite = new Sprite();
            sprite.LoadGif(path);
        }

        private void SetBulletStats(int type)
        {
            switch (type)
            {
                case 0:
                    speed = 6;
                    damage = 5;
                    w = 20;
                    h = 40;
                    break;
                case 1: // BigEnemy beam
                    speed = 0;
                    damage = 30;
                    w = 60;
                    h = 300;
                    break;
                case 2:
                    speed = 3;
                    damage = 20;
                    w = 20;
                    h = 40;
                    break;
            }
        }

        public void Update()
        {
            if (enemyType == 1) // beam
            {
                lifetime++;
                if (lifetime > maxLifetime)
                    y = 9999;
            }
            else
            {
                y += (int)speed;
            }
        }

        public void Paint(ref Graphics g)
        {
            if (sprite != null)
            {
                animationCounter++;
                if (animationCounter % 2 == 0) // làm chậm animation
                {
                    sprite.index = (sprite.index + 1) % sprite.bitmaps.Count;
                }

                sprite.Draw(ref g, x, y, w, h);
            }
            else
            {
                g.FillRectangle(Brushes.Red, x, y, w, h);
            }
        }

        public int getX() => x;
        public int getY() => y;
        public int getW() => w;
        public int getH() => h;
    }
}
