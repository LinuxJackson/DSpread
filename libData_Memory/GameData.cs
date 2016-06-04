using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libObjProperties;
using libFlags;

namespace libData_Memory
{
    public static class GameData
    {
        public static List<DSObject> arrEnemyObjects = new List<DSObject>();
        public static List<DSObject> arrUserObjects = new List<DSObject>();
        public static int UserObjectCount = 5;
        public static int EnemyObjectCount = 5;
        public static List<DSObject> arrSelectedUserObjects = new List<DSObject>();
        public static List<DSObject> arrSelectedEnemiesObjects = new List<DSObject>();
        public static bool IsUserOverride = false;
        public static bool IsEnemyOverride = false;
    }
}
