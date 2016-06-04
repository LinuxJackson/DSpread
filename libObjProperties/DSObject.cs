using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using libFlags;

namespace libObjProperties
{
    public class DSObject
    {
        public DSObject(TeamSign.Teams team)
        {
            this.team = team;
            ObjectProperties.Bdr.Tag = this;
        }
        public DSObject(double ATK, double Armor, TeamSign.Teams team)
        {
            objectProperties.Atk = ATK;
            objectProperties.Armor = Armor;
            this.team = team;
            objectProperties.Bdr.Tag = this;
        }

        private Skills objectProperties = new Skills();
        public Skills ObjectProperties
        {
            get { return objectProperties; }
            set { objectProperties = value; }
        }

        private TeamSign.Teams team;

        public TeamSign.Teams Team
        {
            get { return team; }
            set { team = value; }
        }

    }
}
