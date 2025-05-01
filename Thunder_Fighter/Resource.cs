using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter
{
    class Resource
    {
         public static string ASSETSPATH = Path.GetFullPath("../../../assets");
        public static List<string> ENGINE_DART = new List<string>()
         {
             Path.Combine(ASSETSPATH, "player/Weapons/dart_0.gif"),
             Path.Combine(ASSETSPATH, "player/Weapons/dart_1.gif"),
             Path.Combine(ASSETSPATH, "player/Weapons/dart_2.gif"),
             Path.Combine(ASSETSPATH, "player/Weapons/dart_3.gif"),
         };
        public static List<string> ENGINE = new List<string>()
         {
             Path.Combine(ASSETSPATH, "player/Weapons/Auto Canon.gif"),
             Path.Combine(ASSETSPATH, "player/Weapons/Big Space Gun.gif"),
             Path.Combine(ASSETSPATH, "player/Weapons/Rockets.gif"),
             Path.Combine(ASSETSPATH, "player/Weapons/Zapper.gif"),
         };
    }
}
