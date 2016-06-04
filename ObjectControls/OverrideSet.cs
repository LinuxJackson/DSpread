using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libData_Memory;
using libObjProperties;

namespace ObjectControls
{
    static public class OverrideSet
    {
        static public void SetOn(libFlags.TeamSign.Teams overrideTeam)
        {
            if (overrideTeam == libFlags.TeamSign.Teams.User)
            {
                foreach(DSObject ds in GameData.arrEnemyObjects)
                {
                    ds.ObjectProperties.Atk = ds.ObjectProperties.Atk * 1.2;
                    ds.ObjectProperties.Armor = ds.ObjectProperties.Armor * 1.2;
                }
            }
            else
            {
                foreach (DSObject ds in GameData.arrUserObjects)
                {
                    ds.ObjectProperties.Atk = ds.ObjectProperties.Atk * 1.2;
                    ds.ObjectProperties.Armor = ds.ObjectProperties.Armor * 1.2;
                }
            }
        }

        static public void SetOff(libFlags.TeamSign.Teams overrideTeam)
        {
            if (overrideTeam == libFlags.TeamSign.Teams.User)
            {
                foreach (DSObject ds in GameData.arrEnemyObjects)
                {
                    ds.ObjectProperties.Atk = ds.ObjectProperties.Atk / 1.2;
                    ds.ObjectProperties.Armor = ds.ObjectProperties.Armor / 1.2;
                }
            }
            else
            {
                foreach (DSObject ds in GameData.arrUserObjects)
                {
                    ds.ObjectProperties.Atk = ds.ObjectProperties.Atk / 1.2;
                    ds.ObjectProperties.Armor = ds.ObjectProperties.Armor / 1.2;
                }
            }
        }
    }
}
