using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_CycleOfDecay : P_Season
    {
        public P_Season_CycleOfDecay(Map map) : base(map) { }

        public override string getName() => "Cycle of Decay";

        public override string getDesc()
        {
            return $"Cast on a location to convert its <b>death</b> modifier into <b>fey presence</b>.";
        }

        public override string getFlavour()
        {
            return "...";
        }

        public override string getRestrictionText()
        {
            return $"Target location must have a <b>death</b> modifier.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.death;
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            Pr_Death death = loc.properties.OfType<Pr_Death>().FirstOrDefault();
            if (death != null && death.charge > 0) return true;
            return false;
        }

        public override int getCost() => 1;

        public override void cast(Location loc)
        {
            base.cast(loc);
            Pr_FeyPresence fey = loc.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            Pr_Death death = loc.properties.OfType<Pr_Death>().FirstOrDefault();

            if (death == null) return;

            if (fey == null)
            {
                fey.charge = death.charge;
                loc.properties.Add(fey);
            }
            else if (fey.charge < 50) {
                fey.charge += death.charge;
            }

            loc.properties.RemoveAll(pr => pr is Pr_Death);
        }

    }
}
