using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libObjProperties;

namespace ObjectControls
{
    static public class AvoidAttack
    {
        static public void Avoid(List<DSObject> objects)
        {
            foreach(DSObject ds in objects)
            {
                if (ds.ObjectProperties.AvoidingAttackAvailable)
                    ds.ObjectProperties.AvoidingAttack = true;
            }
        }
    }
}
