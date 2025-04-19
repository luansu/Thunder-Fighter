using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Thunder_Fighter.BSLayers
{
    class BossEnemy : Enemy
    {
        private bool isEntering = true;
        private int targetY = 40;

        public BossEnemy(int x, int y)
            : base(x, y, 350, 350, 500, LoadBossSprite()) // máu boss cao hơn
        {
            dx = 2;
            dy = 3;
            movementMode = 0; // di chuyển cả X & Y khi cần
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

            // Giai đoạn boss đang bay xuống
            if (isEntering)
            {
                y += dy;

                if (y >= targetY)
                {
                    y = targetY;
                    isEntering = false;

                    // Bắt đầu di chuyển như BigEnemy
                    dx = 2;
                    dy = 0;
                    movementMode = 1;
                }
            }
            else
            {
                base.Update(); // di chuyển ngang qua lại như thường
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

