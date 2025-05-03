using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    class ExplosionEff
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public int type; // 0: fighter_die, 1: enemy_die, 2: small_enemy_die, 3: fighter_get_hit, 4: small_enemy_get_hit, 5: boss_get_hit
        public Sprite eff;
        public bool isStart = true;

        public ExplosionEff(int type)
        {
            this.type = type;
            this.loadSprite();
        }

        public void loadSprite()
        {
            Sprite tempS = new Sprite();
            if (this.type == 0)
            {
                tempS.LoadGif(Resource.FIGHTER_DIE_EFF);
            }
            else if (this.type == 1)
            {
                tempS.LoadGif(Resource.ENEMY_DIE_EFF);
            }
            else if (this.type == 2)
            {
                tempS.LoadGif(Resource.SMALL_ENEMY_DIE_EFF);
            }
            else if (this.type == 3)
            {
                tempS.LoadGif(Resource.FIGHTER_GET_HIT_EFF);
            }
            else if (this.type == 4)
            {
                tempS.LoadGif(Resource.SMALL_ENEMY_GET_HIT_EFF);
            }
            else if (this.type == 5)
            {
                tempS.LoadGif(Resource.BOSS_GET_HIT_EFF);
            }
            this.eff = tempS;
        }

        public void update()
        {

        }

        public void paint(ref Graphics g)
        {
            if (this.isStart)
            {
                if (Form1.frameCount % 2 == 0) this.eff.index++;
                this.eff.Draw(ref g, Fighter.x, Fighter.y, Fighter.w, Fighter.h);
            }
        }
    }
}
