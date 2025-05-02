using System;
using System.Drawing;
using System.IO;

namespace Thunder_Fighter.BSLayers
{
    class Dart : IObject
    {
        public int type;
        public int x;
        public int y;
        public int w;
        public int h;
        public string name;
        public float speed;
        public float damage;
        public Sprite dartSprite;
        public int coolDown = 60;

        public bool isFire = false;

        private int lifetime = 0;
        private int maxLifetime = 90;
        private int animationCounter = 0;

        // Dart của người chơi
        public Dart(string name, float speed, float damage, int type)
        {
            this.name = name;
            this.speed = speed;
            this.damage = damage;
            this.type = type;
            this.loadDart();
        }

        // Dart thay thế EnemyBullet
        public Dart(int startX, int startY, int enemyType)
        {
            this.type = enemyType;
            this.x = startX;
            this.y = startY;

            LoadBulletSprite(enemyType);
            SetBulletStats(enemyType);
        }

        private void loadDart()
        {
            if (this.type == 0) { this.coolDown = 60; this.speed = 20; }
            else if (this.type == 1) { this.coolDown = 120; this.speed = 10; }
            else if (this.type == 2) { this.coolDown = 20; this.speed = 25; }
            else if (this.type == 3) { this.coolDown = 0; }

            dartSprite = new Sprite();
            dartSprite.LoadGif(Resource.ENGINE_DART[this.type]);
            this.w = Fighter.w * 2 / 3;
            this.h = Fighter.h * 2 / 3;
            this.x = Fighter.x + Fighter.w / 2 - this.w / 2;
            this.y = Fighter.y;
        }

        private void LoadBulletSprite(int type)
        {
            string path = type switch
            {
                0 => Path.Combine(Form1.assetPath, "enemy1/attack/Kla'ed - Torpedo.gif"),
                1 => Path.Combine(Form1.assetPath, "enemy1/attack/Kla'ed - Ray.gif"),
                2 => Path.Combine(Form1.assetPath, "enemy1/attack/Kla'ed - Wave.gif"),
                _ => null
            };

            dartSprite = new Sprite();
            dartSprite.LoadGif(path);
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
                case 1:
                    speed = 0;
                    damage = 30;
                    w = 60;
                    h = 300;
                    break;
                case 2:
                    speed = 3;
                    damage = 20;
                    w = 120;
                    h = 240;
                    break;
            }
        }

        public void update()
        {
            if (type == 1)
            {
                lifetime++;
                if (lifetime > maxLifetime)
                    y = 9999;
            }
            else
            {
                y += (int)speed; // Enemy đạn bay xuống
            }

            if (isFire)
                y -= (int)speed; // Dart (người chơi) bay lên
        }

        public void Paint(ref Graphics g)
        {
            if (Form1.frameCount % 2 == 0) dartSprite.index++;
            dartSprite.Draw(ref g, x, y, w, h);
        }

        public bool isCollide(IObject obj)
        {
            return !(x + w < obj.getX() || x > obj.getX() + obj.getW() ||
                     y + h < obj.getY() || y > obj.getY() + obj.getH());
        }

        public int getX() => x;
        public int getY() => y;
        public int getW() => w;
        public int getH() => h;
    }
}
