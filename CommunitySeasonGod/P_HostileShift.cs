using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_HostileShift : Power
    {
        public P_HostileShift(Map map)
            : base(map)
        {
            
        }

        public override string getName()
        {
            return "Hostile Shift";
        }

        public override string getDesc()
        {
            return "Brings up a list of seasons which you can immediately switch to, allowing early transition to a season of your choice. You do not receive the bonus powers or effects that come with a natural season transition. This power costs 3 when cast of a location with fey presence between 100.0 (inclusively) and 200.0 (exclusively), 2 when cast on a location with fey presence between 200.0 (inclusively) and 300.0 (exclusively), and 1 when cast on a location with 300.0 fey presence.";
        }

        public override string getFlavour()
        {
            return "The sky darkens, violent in its suddenness. The wind howls across the landscape, stripping loose tiles from roofs. The changing of seasons has come unexpectedly, suddenly, and violently, casting the world anew.";
        }

        public override string getRestrictionText()
        {
            return "Target location must have a fey presence modifier with charge of at least 100.0. Casting cost is greater the lower the charge (displayed on target location).";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.Icon_HostileShift.jpg");
        }

        public override int getCost()
        {
            return 1;
        }

        public virtual int GetCost(Location loc)
        {
            Pr_FeyPresence feyPresence = (Pr_FeyPresence)loc.properties.FirstOrDefault(pr => pr is Pr_FeyPresence);
            if (feyPresence == null || feyPresence.charge < 100.0)
            {
                return -1;
            }

            if (feyPresence.charge < 200.0)
            {
                return getCost() + 2;
            }

            if (feyPresence.charge < 300.0)
            {
                return getCost() + 1;
            }

            return getCost();
        }


        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            Pr_FeyPresence feyPresence = (Pr_FeyPresence)loc.properties.FirstOrDefault(pr => pr is Pr_FeyPresence);
            if (feyPresence == null || feyPresence.charge < 100.0)
            {
                return false;
            }

            if (feyPresence.charge < 200.0)
            {
                if (map.overmind.power >= 3)
                {
                    return true;
                }
                return false;
            }

            if (feyPresence.charge < 300.0)
            {
                if (map.overmind.power >= 2)
                {
                    return true;
                }
                return false;
            }

            return true;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            if (!(map.overmind.god is God_Season seasonGod))
            {
                map.overmind.power += getCost();
                return;
            }

            List<SubGod> subGods = seasonGod.GetSelectableSubGods();
            if (subGods.Count == 0)
            {
                map.overmind.power += getCost();
                return;
            }

            int additionalCost = GetCost(loc) - getCost();

            if (additionalCost > 0)
            {
                map.overmind.power -= additionalCost;
            }
            Pr_FeyPresence feyPresence = (Pr_FeyPresence)loc.properties.FirstOrDefault(pr => pr is Pr_FeyPresence);
            loc.properties.Remove(feyPresence);

            Sel2_SeasonSelector selector = new Sel2_SeasonSelector(seasonGod, subGods, getCost() + additionalCost);
            List<string> targetLabels = new List<string> { "Random" };
            if (Kernel_Season.opt_deckMode)
            {
                targetLabels.Add("Random (Shuffle Deck)");
            }

            targetLabels.AddRange(subGods.Select<SubGod, string>(sg => sg.GetName() + " (" + sg.GetKeywords() + ")"));
            map.world.ui.addBlocker(map.world.prefabStore.getScrollSetText(targetLabels, false, selector, "Choose New Season", "Select the season to immediately transition to.").gameObject);
        }
    }
}
