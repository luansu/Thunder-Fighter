using System;
using System.IO;

namespace Thunder_Fighter.BSLayers
{
    class BossEnemy : Enemy
    {
        private bool isEntering = true;
        private int targetY = 40;

        public BossEnemy(int x, int y)
            : base(x, y, 350, 350, 500, LoadBossSprite())
        {
            dx = 2;
            dy = 3;
            movementMode = 0;
        }

        private static Sprite LoadBossSprite()
        {
            string path = Path.Combine(Form1.assetPath, "enemy1/base/Kla'ed - Dreadnought - Engine.gif");
            Sprite sprite = new Sprite();
            sprite.LoadGif(path);
            return sprite;
        }

        public override int GetEnemyType() => 2;

        public override void Update()
        {
            if (isDead && isExploding)
            {
                base.Update();
                return;
            }

            if (isEntering)
            {
                y += dy;
                if (y >= targetY)
                {
                    y = targetY;
                    isEntering = false;
                    dx = 2;
                    dy = 0;
                    movementMode = 1;
                }
            }
            else
            {
                base.Update();
            }
        }

        public override void FireBullet()
        {
            if (fireCooldown == 0)
            {
                int bulletW = 20;
                int bulletH = 40;
                int centerX = x + w / 2;
                int bulletY = y + h / 2 - bulletH / 2;

                bullets.Add(new EnemyBullet(centerX - 40, bulletY, 2));
                bullets.Add(new EnemyBullet(centerX - bulletW / 2, bulletY, 2));
                bullets.Add(new EnemyBullet(centerX + 20, bulletY, 2));

                fireCooldown = fireInterval;
            }
        }

        public override void Die()
        {
            base.Die();
            string explosionPath = Path.Combine(Form1.assetPath, "enemy1/destruction/Kla'ed - Dreadnought - Destruction.gif");
            explosionSprite = new Sprite();
            explosionSprite.LoadGif(explosionPath);
        }
    }
}
