using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_Fertilization : P_Season
    {
        public static int range = 4;
        public static double maxDevastation = 100;

        public P_Season_Fertilization(Map map) : base(map)
        {
        }

        public override string getName()
        {
            return "Fertilization";
        }

        public override string getDesc()
        {
            return "Replaces up to " + maxDevastation + "% of Devastation in a location with Fey Presence. If a full 100% Devastation is reduced and the ruler is unaware, they gain liking for The Dark.";
        }

        public override string getFlavour()
        {
            return "Every destroyed building and ruined crop can be naturally fed on and repurposed. The fey simply do this much faster.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location with Devastation within " + range + " steps of Fey Crops";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_fertilization.png");
        }

        public override bool validTarget(Location loc)
        {

            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_Devastation)
                {
                    if (map.overmind.god is God_Season season && season.ActiveSubGod is SubGod_Bloom bloom)
                    {
                        foreach (Property pr2 in bloom.crops)
                        {
                            if (map.getStepDist(pr2.location, loc) <= range)
                            {
                                return true;
                            }
                        }

                        return false;
                    }
                    return true;

                }
            }

            return false;
        }

        public override int getCost()
        {
            return 1;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            double devastationRemoved = 0;
            for (int i = 0; i < loc.properties.Count; i++)
            {
                if (loc.properties[i] is Pr_Devastation)
                {
                    if (loc.properties[i].charge < maxDevastation - devastationRemoved)
                    {
                        devastationRemoved += loc.properties[i].charge;
                        loc.properties.RemoveAt(i);
                    }
                    else
                    {
                        loc.properties[i].charge -= maxDevastation - devastationRemoved;
                        devastationRemoved = maxDevastation;
                        break;
                    }
                }
            }

            bool foundFeyPresence = false;
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_FeyPresence)
                {
                    foundFeyPresence = true;
                    pr.charge += devastationRemoved;
                }
            }
            if (!foundFeyPresence)
            {
                Pr_FeyPresence presence = new Pr_FeyPresence(loc);
                presence.charge = devastationRemoved;
                loc.properties.Add(presence);
            }

            if (devastationRemoved >= maxDevastation)
            {
                if (loc.settlement is SettlementHuman sh)
                {
                    if (sh.ruler != null && sh.ruler.awareness < 1)
                        sh.ruler.increasePreference(map.soc_dark.index + 20000);
                }
            }


        }



    }
}
