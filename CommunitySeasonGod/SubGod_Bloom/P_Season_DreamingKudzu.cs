using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_DreamingKudzu : P_Season
    {
        public static double startingKudzu = 100;
        public static double presenceCost = 300;
        public static double range = 4;

        public P_Season_DreamingKudzu(Map map) : base(map) { }

        public override string getName()
        {
            return "Dreaming Kudzu";
        }

        public override string getDesc()
        {
            return "Spends " + presenceCost + "% Fey Presence to create 100% Dreaming Kudzu at a location. Dreaming Kudzu spreads similarly to a plague, slightly increasing Fey Presence and Madness and slightly damaging <b>habitability</b>. Heroes and rulers who are aware or who dislike Madness will be heavily distracted fighting the kudzu, while other heroes and rulers will be helpless against it.";
        }

        public override string getFlavour()
        {
            return "Some of the Niece's gifts to humanity grow faster than they have any right, and the overgrowth is not taken as seriously as it should be.";
        }

        public override string getRestrictionText()
        {
            return "Must target a land location with at least " + presenceCost + " Fey Presence within " + range + " steps of Fey Crops that does not have Purged Kudzu present";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_dreaming_kudzu.png");
        }

        public override bool validTarget(Location loc)
        {
            if (loc.isOcean)
                return false;

            double presence = 0;
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_FeyPresence)
                {
                    presence += pr.charge;
                }
                else if (pr is Pr_Season_PurgedKudzu)
                    return false;
            }

            if (presence >= presenceCost)
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

            return false;
        }

        public override int getCost()
        {
            return 6;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            double presenceSpent = 0;
            Pr_Season_DreamingKudzu kudzu = null;

            for (int i = 0; i < loc.properties.Count; i++)
            {
                if (loc.properties[i] is Pr_FeyPresence)
                {
                    if (loc.properties[i].charge >= presenceCost - presenceSpent)
                    {
                        loc.properties[i].charge -= presenceCost - presenceSpent;
                        presenceSpent = presenceCost;
                    }
                    else
                    {
                        presenceSpent += loc.properties[i].charge;
                        loc.properties.RemoveAt(i);
                        i--;
                    }
                }
                else if (loc.properties[i] is Pr_Season_DreamingKudzu foundKudzu)
                    kudzu = foundKudzu;
            }

            if (kudzu != null)
            {
                kudzu.charge += startingKudzu;
            }
            else
            {
                kudzu = new Pr_Season_DreamingKudzu(loc);
                kudzu.charge = startingKudzu;
                loc.properties.Add(kudzu);
                kudzu.turnTick();
            }
        }

    }
}
