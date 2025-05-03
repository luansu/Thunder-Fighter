using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    class Engine : IObject
    {
        public int type;
        public int x;
        public int y;
        public int w;
        public int h;
        public Dart dartTemplate;
        public List<Dart> fireDarts = new List<Dart>();
        public EngineEff eff;
        public Sprite eSprite;

        public bool isFire = false;
        public bool isCooling = false;
        public List<int> dart_3_fire_phase = new List<int>() {2, 4, 6, 8, 10, 12};
        public List<int> dart_3_cooling_phase = new List<int>() { 3, 5, 7, 9, 11, 13 };

        public Engine(int type)
        {
            this.type = type;
            this.loadEngine(type);
        }
        
        public void loadEngine(int type)
        {
            if (type == 0)
            {
                dartTemplate = new Dart("Canon", 5, 5, 0);
            }
            else if (type == 1)
            {
                dartTemplate = new Dart("Energy Ball", 5, 5, 1);
            }
            else if (type == 2)
            {
                dartTemplate = new Dart("Rocket", 5, 5, 2);
            }
            else if (type == 3)
            {
                dartTemplate = new Dart("Laser", 5, 5, 3);
            }
            eSprite = new Sprite();
            eSprite.LoadGif(Resource.ENGINE[type]);
            this.x = Fighter.x;
            this.y = Fighter.y;
            this.w = Fighter.w;
            this.h = Fighter.h;
        }   
        public void update()
        {
            this.x = Fighter.x;
            this.y = Fighter.y;
            foreach(Dart dart in fireDarts)
            {
                dart.update();
            }
            if(this.isFire && !this.isCooling)
            {
                // type 0 sẽ bắn cùng lúc 2 viên đạn
                if(dartTemplate.type == 0)
                {
                    Dart lDart = new Dart(dartTemplate.name, dartTemplate.speed, dartTemplate.damage, dartTemplate.type);
                    lDart.isFire = true;
                    fireDarts.Add(lDart);
                    Dart rDart = new Dart(dartTemplate.name, dartTemplate.speed, dartTemplate.damage, dartTemplate.type);
                    rDart.isFire = true;
                    fireDarts.Add(rDart);
                    lDart.x = Fighter.x - 1;
                    rDart.x = Fighter.x + this.w / 3 + 1;

                }
                // type 1 sẽ bắn ra 1 viên đạn/ lần
                else if (dartTemplate.type == 1)
                {
                    Dart newDart = new Dart(dartTemplate.name, dartTemplate.speed, dartTemplate.damage, dartTemplate.type);
                    newDart.isFire = true;
                    fireDarts.Add(newDart);
                }
                // type 2 sẽ bắn từng viên nhưng vị trí sẽ khác nhau
                else if (dartTemplate.type == 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Dart newDart = new Dart(dartTemplate.name, dartTemplate.speed, dartTemplate.damage, dartTemplate.type);
                        //newDart.x = Fighter.x - 12 + i*6;
                        //newDart.y = this.y;
                        newDart.isFire = false;
                        fireDarts.Add(newDart);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        Dart newDart = new Dart(dartTemplate.name, dartTemplate.speed, dartTemplate.damage, dartTemplate.type);
                        //newDart.x = Fighter.x + 40 + i * 6;
                        //newDart.y = this.y;
                        newDart.isFire = false;
                        fireDarts.Add(newDart);
                    }
                }
                // type 3 sẽ bắn ra đạn dài, chạn vào vật thể sẽ bị chặn lại
                else if (dartTemplate.type == 3)
                {

                }
                this.isFire = false;
                this.isCooling = true;
            }
        }

        public void paint(ref Graphics g)
        {
            if (Form1.frameCount % 2 == 0) eSprite.index++;

            if (eSprite.index == 0)
            {
                this.isCooling = false;
            }

            if(this.type == 2)
            {
                if (eSprite.index == 0)
                {
                    this.isFire = true;
                }
            }
            else
            {
                if (eSprite.index == eSprite.bitmaps.Count - 1)
                {
                    this.isFire = true;
                }
            }

            if (this.type == 2 && this.fireDarts.Count >= 6)
            {
                int dartIndex = this.fireDarts.Count - 6;
                List<int> arr = new List<int>() { 2, 3, 1, 4, 0, 5 };

                for (int i = 0; i < 6; i++)
                {
                    if (this.dart_3_fire_phase[i] == eSprite.index)
                    {
                        int index = dartIndex + arr[i];
                        int offsetX = (arr[i] >= 3) ? +22 + arr[i] * 6 : -10 + arr[i] * 6;
                        fireDarts[index].x = Fighter.x + offsetX;
                        fireDarts[index].y = this.y;
                        fireDarts[index].isFire = true;
                    }
                }
            }

            foreach (Dart dart in fireDarts)
            {
                dart.paint(ref g);
            }
            eSprite.Draw(ref g, this.x, this.y, this.w, this.h);

        }
        public bool isCollide(IObject obj)
        {
            return !(this.x + this.w < obj.getX() || this.x > obj.getX() + obj.getW() ||
             this.y + this.h < obj.getY() || this.y > obj.getY() + obj.getH());
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
