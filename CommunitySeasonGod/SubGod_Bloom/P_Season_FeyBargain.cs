using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_FeyBargain : P_Season
    {

        public P_Season_FeyBargain(Map map) : base(map)
        {
        }

        public override string getName()
        {
            return "Fey Bargain";
        }

        public override string getDesc()
        {
            return "Immediately transforms a human outpost into a full-fledged settlement. That settlement starts infiltrated and 100% enshadowed, and the ruler gains extreme liking for The Dark.";
        }

        public override string getFlavour()
        {
            return "Founding a new settlement is gueling work, and the risk of failure is ever-present. If a shortcut is offered, the settlers will remember.";
        }

        public override string getRestrictionText()
        {
            return "Must target a human outpost";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_fey_bargain.png");
        }

        public override bool validTarget(Location loc)
        {

            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_HumanOutpost)
                    return true;
            }

            return false;
        }

        public override int getCost()
        {
            return 3;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            for (int i = 0; i < loc.properties.Count; i++)
            {
                if (loc.properties[i] is Pr_HumanOutpost)
                {
                    loc.properties[i].charge = 10000;
                    loc.properties[i].turnTick();
                }
            }

            if (loc.settlement != null)
            {
                loc.settlement.shadow = 1;
                foreach (Subsettlement sub in loc.settlement.subs)
                {
                    if (sub.canBeInfiltrated())
                        sub.infiltrated = true;
                }
            }


            Pr_Season_FeyCrops crops = new Pr_Season_FeyCrops(loc);
            loc.properties.Add(crops);
            if (map.overmind.god is God_Season season)
            {
                foreach (SubGod subGod in season.SubGods)
                {
                    if (subGod is SubGod_Bloom bloom)
                        bloom.crops.Add(crops);
                }
            }

            loc.properties.Add(new Pr_Season_BloomingFields(loc));
            loc.properties.Add(new Pr_Season_GratefulSettlers(loc));

        }



    }
}
