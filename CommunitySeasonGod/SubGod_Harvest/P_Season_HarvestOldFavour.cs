using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public  class P_Season_HarvestOldFavour : P_Season
    {
        public P_Season_HarvestOldFavour(Map map) : base(map) { }

        public override string getName()
        {
            return "Old Favour";
        }

        public override string getDesc()
        {
            return $"Increases the time until the next season transition by 50% of the season length ({Math.Max(10, Kernel_Season.opt_seasonLength / 2)}), minimum 10 turns.";
        }

        public override string getFlavour()
        {
            return "The Uncle of the Harvest has always been willing to help the other members of the family. As such, he has accrued a number of favours from every other member of the family. Now he calls one in.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.Icon_Stasis.jpg");
        }

        public override int getCost()
        {
            return 0;
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            if (!(map.overmind.god is God_Season seasonGod) || !seasonGod.TryApplyStasis())
            {
                map.overmind.power += getCost();
            }

            for (int i = 0; i < map.overmind.god.powers.Count; i++)
            {
                if (map.overmind.god.powers[i] is P_Season_HarvestOldFavour)
                {
                    map.overmind.god.powers.RemoveAt(i);
                    map.overmind.god.powerLevelReqs.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
