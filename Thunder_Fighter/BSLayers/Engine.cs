using System;
using System.Collections.Generic;
using System.Linq;
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
        public Dart dart;
        public EngineEff eff;
        public Sprite sprite;

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
