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
            if (Kernel_Season.opt_deckMode)
            {
                if (index < 2) // Index 0 & Index 1 = random
                {
                    if (index == 1) // Index 1 == random with shuffle.
                    {
                        God.ShuffleSubGodDeck();
                    }

                    SubGod subGod = God.SelectRandomSelectableSubGod();
                    if (subGod == null)
                    {
                        Console.WriteLine("ComSeasonGod: Unable to switch to random selectable sub-god: No new selectable sub-god available.");
                        return;
                    }

                    God.ChangeSubGod(subGod, false);
                    return;
                }

                index -= 2; // Offset the index to align the label indexes with the sub-god indexes, ignoring the random options
            }
            else
            {
                if (index == 0) // Index 1 = random
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

                index--; // Offset the index to align the label indexes with the sub-god indexes, ignoring the random option
            }

            God.ChangeSubGod(Targets[index], false);
        }
    }
}
