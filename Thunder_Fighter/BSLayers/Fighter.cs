using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    class Fighter : IObject
    {
        public int type;
        public string name;
        public static int x;
        public static int y;
        public static int w;
        public static int h;
        public static int mHealth;
        public static int mStamina;
        public static int health;
        public static int stamina;
        public bool isProtected = false;
        public int protectTime = 0;
        public bool isGetHit = false;
        public Engine engine;
        public Sprite baseSprite;
        public Sprite shieldSprite;
        public static int status = 0; // 0: 100%, 1: 75%, 2: 50%, 3: 25%, 4: 0%
        public StatusBar statusBar = new StatusBar();
        ExplosionEff dieEff;
        List<ExplosionEff> getHitEffs;
        public Fighter(string name, int health, int stamina)
        {
            this.name = name;
            Fighter.mHealth = health;
            Fighter.mStamina = stamina;
            Fighter.health = 99;
            Fighter.stamina = stamina;
        }

        public void loadEff()
        {
            this.dieEff = new ExplosionEff(0);
            this.getHitEffs = new List<ExplosionEff>();
        }

        public void update() 
        {
            engine.update();
            this.protectTime -= 1;
            if(this.protectTime <= 0)
            {
                this.isProtected = false;
                this.protectTime = 0;
            }

            float percentH = (float) Fighter.health / Fighter.mHealth;
            if(percentH > 0.75)
            {
                Fighter.status = 0;
            }
            else if (percentH > 0.5)
            {
                Fighter.status = 1;
            }
            else if (percentH > 0.25)
            {
                Fighter.status = 2;
            }
            else if (percentH > 0)
            {
                Fighter.status = 3;
            }
            else
            {
                Fighter.status = 4;
                if(this.dieEff is not null)
                {
                    this.dieEff.isStart = true;
                    this.dieEff.update();
                }
            }

            if (this.isGetHit)
            {
                this.getHitEffs.Add(new ExplosionEff(3));
                this.isGetHit = false;
            }
            
            statusBar.update();
        }

        public void paint(ref Graphics g)
        {
            if(Fighter.status != 4)
            {
                engine.paint(ref g);
            }
            if(Fighter.status < 4)
            {
                baseSprite.index = status;
                baseSprite.Draw(ref g, Fighter.x, Fighter.y, Fighter.w, Fighter.h);
            }
            else if (Fighter.status == 4 && this.dieEff is not null)
            {
               this.dieEff.paint(ref g);
            }
            if (this.isProtected && Fighter.status != 4)
            {
                if (Form1.frameCount % 2 == 0) shieldSprite.index++;
                shieldSprite.Draw(ref g, Fighter.x, Fighter.y, Fighter.w, Fighter.h);
            }
            if (this.getHitEffs.Count > 0)
            {
                //foreach (ExplosionEff eff in this.getHitEffs)
                //{
                //    if (Form1.frameCount % 2 == 0) eff.eff.index++;
                //    bool isEnd = eff.eff.index == eff.eff.bitmaps.Count - 1;
                //    eff.paint(ref g); 
                //    if (isEnd)
                //    {
                //        this.getHitEffs.Remove(eff);
                //    }
                //}

                for (int i = this.getHitEffs.Count - 1; i >= 0; i--)
                {
                    ExplosionEff eff = this.getHitEffs[i];
                    if (Form1.frameCount % 2 == 0) eff.eff.index++;
                    bool isEnd = eff.eff.index == eff.eff.bitmaps.Count - 1;
                    eff.paint(ref g);
                    if (isEnd)
                    {
                        this.getHitEffs.RemoveAt(i);
                    }
                }
            }
            statusBar.paint(ref g);
        }

        public void move(int dx, int dy)
        {
            if(Fighter.status!= 4)
            {
                Fighter.x += dx;
                Fighter.y += dy;
            }
        }

        public void getShield(int time = 9000)
        {
            if (Fighter.status != 4)
            {
                this.isProtected = true;
                this.protectTime = time;
            }
        }

        public void changeWeapon(int type)
        {
            if (Fighter.status != 4)
            {
                this.engine = new Engine(type);
            }
        }

        public void moveTo(int x, int y)
        {
            if (Fighter.status != 4)
            {
                Fighter.x = x;
                Fighter.y = y;
            }
        }

        public void getHit(Dart b)
        {
            if(Fighter.status != 4)
            {
                if (!this.isProtected)
                {
                    Fighter.health -= (int)b.damage;
                }
                b.y = 9999;
                this.isGetHit = true;
            }

        }
        public bool isCollide(IObject obj)
        {
            return !(Fighter.x + Fighter.w < obj.getX() || Fighter.x > obj.getX() + obj.getW() ||
             Fighter.y + Fighter.h < obj.getY() || Fighter.y > obj.getY() + obj.getH());
        }
        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return y;
        }
        public int getW()
        {
            return w;
        }
        public int getH()
        {
            return h;
        }
    }
}
