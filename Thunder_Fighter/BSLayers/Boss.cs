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
                int bulletW = 100;
                int bulletH = 200;

                int centerX = x + w / 2;
                int bulletY = y + h - 130;

                var bullet1 = new EnemyBullet(centerX - bulletW - 10, bulletY, 2);
                bullet1.w = bulletW;
                bullet1.h = bulletH;

                var bullet2 = new EnemyBullet(centerX - bulletW / 2, bulletY, 2);
                bullet2.w = bulletW;
                bullet2.h = bulletH;

                var bullet3 = new EnemyBullet(centerX + bulletW + 10 - bulletW, bulletY, 2);
                bullet3.w = bulletW;
                bullet3.h = bulletH;

                bullets.Add(bullet1);
                bullets.Add(bullet2);
                bullets.Add(bullet3);

                fireCooldown = fireInterval;
            }
        }

        public override void Paint(ref Graphics g)
        {
            // Vẽ các viên đạn trước (nằm dưới Boss)
            foreach (var bullet in bullets)
            {
                bullet.Paint(ref g);
            }

            // Vẽ Boss sau (nằm trên đạn)
            base.Paint(ref g);
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
