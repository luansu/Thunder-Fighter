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
        public static string SHIP_BODY = Path.Combine(ASSETSPATH, "player/Main Ship_full.gif");
        public static string SHIELD = Path.Combine(ASSETSPATH, "player/Main Ship - Shields - Invincibility Shield.gif");
        public static string SHIELD_F = Path.Combine(ASSETSPATH, "player/Shield");
        public static string BG = Path.Combine(ASSETSPATH, "background/bg1 - Copy.png");
        public static string HEAHLTH = Path.Combine(ASSETSPATH, "player/Status/health.gif");
        public static string STAMINA = Path.Combine(ASSETSPATH, "player/Status/stamina.gif");

        public static string FIGHTER_DIE_EFF = Path.Combine(ASSETSPATH, "eff/fighter_die.gif");
        public static string ENEMY_DIE_EFF = Path.Combine(ASSETSPATH, "eff/fighter_die.gif");
        public static string SMALL_ENEMY_DIE_EFF = Path.Combine(ASSETSPATH, "eff/fighter_die.gif");
        public static string FIGHTER_GET_HIT_EFF = Path.Combine(ASSETSPATH, "eff/fighter_die.gif");
        public static string SMALL_ENEMY_GET_HIT_EFF = Path.Combine(ASSETSPATH, "eff/fighter_die.gif");
        public static string BOSS_GET_HIT_EFF = Path.Combine(ASSETSPATH, "eff/fighter_die.gif");
    }
}
