using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Thunder_Fighter.BSLayers
{
    class SmallEnemy : Enemy
    {
        public SmallEnemy(int x, int y)
            : base(x, y, 100, 100, 10, LoadSmallEnemySprite()) { }

        public override int GetEnemyType() => 0;

        private static Sprite LoadSmallEnemySprite()
        {
            string path = Path.Combine(Form1.assetPath, "enemy1/base/Kla'ed - Scout - Engine.gif");
            Sprite sprite = new Sprite();
            sprite.LoadGif(path);
            return sprite;
        }

        public override void Die()
        {
            base.Die();
            string explosionPath = Path.Combine(Form1.assetPath, "enemy1/destruction/Kla'ed - Scout - Destruction.gif");
            explosionSprite = new Sprite();
            explosionSprite.LoadGif(explosionPath);
        }
    }
}
