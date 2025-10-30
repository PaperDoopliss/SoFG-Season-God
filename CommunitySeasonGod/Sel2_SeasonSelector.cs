using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class Sel2_SeasonSelector : SelectClickReceiver
    {
        public God_Season God;

        List<SubGod> Targets;

        public int Cost;

        public Sel2_SeasonSelector(God_Season god, List<SubGod> targets, int cost)
        {
            God = god;
            Cost = cost;
            Targets = targets;
        }

        public void cancelled()
        {
            God.map.overmind.power += Cost;
        }

        public void selectableClicked(string text, int index)
        {
            if (index == 0)
            {
                SubGod subGod = God.SelectRandomSelectableSubGod();
                if (subGod == null)
                {
                    Console.WriteLine("ComSeasonGod: Unable to switch to random selectable sub-god: No new selectable sub-god available.");
                    return;
                }

                God.ChangeSubGod(subGod, false);
                return;
            }

            God.ChangeSubGod(Targets[index - 1], false);
        }
    }
}
