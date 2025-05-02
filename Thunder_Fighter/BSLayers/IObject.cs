using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    interface IObject
    {
        bool isCollide(IObject obj);
        int getX();
        int getY();
        int getW();
        int getH();
    }
}
