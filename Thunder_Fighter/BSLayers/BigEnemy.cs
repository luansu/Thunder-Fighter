using System;
using System.IO;

namespace Thunder_Fighter.BSLayers
{
    class BigEnemy : Enemy
    {
        private bool isEntering = true;
        private int targetX;

        private Dart beam;
        private int beamTimer = 0;
        private bool isBeamOn = false;

        private const int beamOnDuration = 30;   // 0.5s @ 60fps
        private const int beamOffDuration = 18;  // 0.3s @ 60fps

        public BigEnemy(int startFromSide, int y)
            : base(0, y, 250, 250, 30, LoadBigEnemySprite())
        {
            if (startFromSide == 0)
                this.x = -w;
            else
                this.x = 540 + w;

            targetX = 145;
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
                    dx = 2;
                    dy = 0;
                    movementMode = 1;
                }
            }
            else
            {
                // ✅ Xử lý beam TRƯỚC khi gọi base.Update()
                beamTimer++;

                if (isBeamOn)
                {
                    if (beam != null)
                    {
                        beam.x = x + (w - beam.w) / 2;
                        beam.y = y + h - 40;
                    }

                    if (beamTimer > beamOnDuration)
                    {
                        beam = null;
                        isBeamOn = false;
                        beamTimer = 0;
                    }
                }
                else
                {
                    if (beamTimer > beamOffDuration)
                    {
                        int beamW = 80;
                        int beamH = 900;
                        int beamX = x + (w - beamW) / 2;
                        int beamY = y + h - 40;

                        beam = new Dart(beamX, beamY, 1);
                        beam.w = beamW;
                        beam.h = beamH;
                        beam.speed = 0;

                        isBeamOn = true;
                        beamTimer = 0;
                    }
                }

                base.Update(); // Gọi sau khi xử lý beam để tránh bug
            }
        }

        // ✅ Chặn FireBullet gốc của Enemy (không cho bắn đạn thường)
        public override void FireBullet()
        {
            // BigEnemy không dùng FireBullet mặc định
        }

        public override void Paint(ref Graphics g)
        {
            if (beam != null)
                beam.paint(ref g); // beam vẽ trước enemy

            base.Paint(ref g); // enemy vẽ sau
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
