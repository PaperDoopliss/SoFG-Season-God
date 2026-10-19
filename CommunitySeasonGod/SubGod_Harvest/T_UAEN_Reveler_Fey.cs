using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_UAEN_Reveler_Fey : Trait
    {
        public override string getName()
        {
            return "Manic Energy";
        }

        public override string getDesc()
        {
            return "The fey nature of this reveler naturally draws upon the emotions of those around them. If they are killed in a settlement, they will immediately generate Unrest.";
        }

        public override int getMaxLevel()
        {
            return 300;
        }

        public override bool preventDeathFromAge()
        {
            return true;
        }

        public override int[] getTags()
        {
            return new int[0];
        }

        public override void onDeath(Unit unit, Person killer)
        {
            if (unit != null && unit.location != null)
            {
                SettlementHuman human = unit.location.settlement as SettlementHuman;
                if (human != null)
                {
                    Property.addToProperty("Sudden end to Revelry", Property.standardProperties.UNREST, 50, unit.location);
                }
            }
            base.onDeath(unit, killer);
        }
    }
}
