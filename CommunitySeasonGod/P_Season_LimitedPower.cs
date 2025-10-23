using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{

    //A power that automatically removes itself once it has run out of charges.
    public class P_Season_LimitedPower : P_Season_SubGodPower
    {

        public int charges = 0;

        public P_Season_LimitedPower(Map map, int charges = 1) : base(map) { this.charges = charges; }

        public override void cast(Location loc)
        {
            base.cast(loc);
            charges--;
            if (charges <= 0)
            {
                for (int i = 0; i < map.overmind.god.powers.Count; i++)
                {
                    if (map.overmind.god.powers[i] == this)
                    {
                        map.overmind.god.powers.RemoveAt(i);
                        map.overmind.god.powerLevelReqs.RemoveAt(i);
                    }
                }
            }
        }

        public override void cast(Unit unit)
        {
            base.cast(unit);
            charges--;
            if (charges <= 0)
            {
                for (int i = 0; i < map.overmind.god.powers.Count; i++)
                {
                    if (map.overmind.god.powers[i] == this)
                    {
                        map.overmind.god.powers.RemoveAt(i);
                        map.overmind.god.powerLevelReqs.RemoveAt(i);
                    }
                }
            }
        }


    }
}
