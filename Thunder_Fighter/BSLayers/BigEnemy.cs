using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Thunder_Fighter.BSLayers
{
    class BigEnemy : Enemy
    {
        private bool isEntering = true;
        private int targetX;

        public BigEnemy(int startFromSide, int y)
            : base(0, y, 250, 250, 30, LoadBigEnemySprite())
        {
            // Xuất hiện từ trái hoặc phải
            if (startFromSide == 0)
                this.x = -w; // ngoài màn hình trái
            else
                this.x = 540 + w; // ngoài màn hình phải (form width giả định)

            // Mục tiêu là trung tâm màn hình
            targetX = 145; // bạn có thể chỉnh theo form thực tế
            dx = (startFromSide == 0) ? 5 : -5;
            dy = 0;
            movementMode = 1;
        }

        private static Sprite LoadBigEnemySprite()
        {
            string path = Path.Combine(Form1.assetPath, "enemy1/base/Kla'ed - Battlecruiser - Engine.gif");
            Sprite sprite = new Sprite();
            sprite.LoadGif(path);
            return sprite;
        }

        public override int GetEnemyType() => 1;

        public override void Update()
        {
            if (isDead && isExploding)
            {
                base.Update();
                return;
            }

            if (isEntering)
            {
                x += dx;

                if ((dx > 0 && x >= targetX) || (dx < 0 && x <= targetX))
                {
                    x = targetX;
                    isEntering = false;

                    // Bắt đầu di chuyển ngang sau khi vào vị trí
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

        public override void Die()
        {
            base.Die();
            string explosionPath = Path.Combine(Form1.assetPath, "enemy1/destruction/Kla'ed - Battlecruiser - Destruction.gif");
            explosionSprite = new Sprite();
            explosionSprite.LoadGif(explosionPath);
        }
    }
}
