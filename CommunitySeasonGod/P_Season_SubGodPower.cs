using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{


    public class P_Season_SubGodPower : Power
    {
        public P_Season_SubGodPower(Map map) : base(map)
        {
        }

        public override void cast(Location loc)
        {
            base.cast(loc);
            for (int i = 0; i < map.overmind.god.powers.Count; i++)
            {
                if (map.overmind.god.powers[i] is P_Season_PickAgain)
                {
                    map.overmind.god.powers.RemoveAt(i);
                    map.overmind.god.powerLevelReqs.RemoveAt(i);
                    i--;
                }
            }
            
        }

        public override void cast(Unit unit)
        {
            base.cast(unit);
            for (int i = 0; i < map.overmind.god.powers.Count; i++)
            {
                if (map.overmind.god.powers[i] is P_Season_PickAgain)
                {
                    map.overmind.god.powers.RemoveAt(i);
                    map.overmind.god.powerLevelReqs.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}
