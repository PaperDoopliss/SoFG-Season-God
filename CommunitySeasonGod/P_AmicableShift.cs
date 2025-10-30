using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_AmicableShift : Power
    {
        [SerializeField]
        private int _maxDurationPenalty = 4;
        public int MaxDurationPenalty => _maxDurationPenalty;

        public P_AmicableShift(Map map)
            : base(map)
        {

        }

        public override string getName()
        {
            return "Amicable Shift";
        }

        public override string getDesc()
        {
            return "Brings up a list of seasons which you can immediately switch to, allowing early transition to a season of your choice. You do not receive the bonus powers or effects that come with a natural season transition. This power is more expensive the earlier in a season you use it.";
        }

        public override string getFlavour()
        {
            return "The seasons are fickle, shifting eternalloy in a complex dance of light, warmth, cold, darkness, birth, death, and rebirth. They shift now, smoothly transitioning from one to another as if it was according to the preordained clock of the world... But it is not.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.Icon_AmicableShift.jpg");
        }

        public override int getCost()
        {
            if (!(map.overmind.god is God_Season seasonGod))
            {
                return 1;
            }

            double seasonRemainingFraction = (double)seasonGod.TurnsRemainingInSeason / (double)Kernel_Season.opt_seasonLength;
            return (int)Math.Round(MaxDurationPenalty * seasonRemainingFraction); // Cost decreases linearly at duration fractions 0.875 (5 -> 4), 0.625 (4 -> 3), 0.375 (3 -> 2), and 0.125 (2 -> 1).
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override void cast(Location loc)
        {
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

            Sel2_SeasonSelector selector = new Sel2_SeasonSelector(seasonGod, subGods, getCost());
            List<string> targetLabels = new List<string> { "Random" };
            targetLabels.AddRange(subGods.Select<SubGod, string>(sg => sg.GetName() + " (" + sg.GetKeywords() + ")"));
            map.world.ui.addBlocker(map.world.prefabStore.getScrollSetText(targetLabels, false, selector, "Choose New Season", "Select the season to immediately transition to.").gameObject);
        }
    }
}
