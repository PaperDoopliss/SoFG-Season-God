using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Stasis : Power
    {
        public P_Stasis(Map map)
            : base(map)
        {

        }

        public override string getName()
        {
            return "Stasis";
        }

        public override string getDesc()
        {
            return $"Increases the time until the next season transition by 50% of the season length ({Math.Max(10, Kernel_Season.opt_seasonLength / 2)}), minimum 10 turns.";
        }

        public override string getFlavour()
        {
            return "The weather lingers, an ususual stability in the perpetually whirling seasons. Time seems to slow, to linger in an eternal moment. The seasons will change late this year.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.Icon_Stasis.jpg");
        }

        public override int getCost()
        {
            return 3;
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
        }
    }
}
