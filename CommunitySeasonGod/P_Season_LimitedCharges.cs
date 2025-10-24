using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{

    //A power that automatically removes itself once it has run out of charges.
    public class P_Season_LimitedCharges : P_Season
    {
        [SerializeField]
        protected int _maxCharges = 1;

        public int Charges = 1;

        public P_Season_LimitedCharges(Map map, int maxCharges = 1)
            : base(map)
        {
            _maxCharges = maxCharges;
            Charges = maxCharges;
        }

        public virtual void ResetCharges()
        {
            Charges = _maxCharges;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            Charges--;
            if (Charges <= 0)
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

            Charges--;
            if (Charges <= 0)
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
