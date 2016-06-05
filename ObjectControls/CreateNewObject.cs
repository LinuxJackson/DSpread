using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libObjProperties;

namespace ObjectControls
{
    static public class CreateNewObject
    {
        static public DSObject Create(List<DSObject> supporter, libFlags.TeamSign.Teams team)
        {
            double Armor = 0;
            double ATK = 0;

            foreach (DSObject ds in supporter)
            {
                Armor = Armor + ds.ObjectProperties.Armor * 0.2;
                ds.ObjectProperties.Armor = ds.ObjectProperties.Armor * 0.75;
                ATK = ATK + ds.ObjectProperties.Atk * 0.2;
                ds.ObjectProperties.Atk = ds.ObjectProperties.Atk * 0.75;
            }

            return new DSObject(ATK, Armor, team);
        }
    }
}
