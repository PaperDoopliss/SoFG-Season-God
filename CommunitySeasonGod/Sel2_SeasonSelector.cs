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
            if (!God.NextShiftIsNatural)
            {
                if (Cost > 0)
                {
                    God.map.overmind.power += Cost;
                    God.ShiftPowerCost = -1;
                }

                return;
            }

            if (God.ActiveSubGod == null)
            {
                God.ChangeSubGodRandom(Array.Empty<SubGod>());
                return;
            }

            God.ChangeSubGodRandom(new SubGod[1] { God.ActiveSubGod });
        }

        public void selectableClicked(string text, int index)
        {
            if (index == 0) // Index 1 = random
            {
                if (God.ActiveSubGod == null)
                {
                    God.ChangeSubGodRandom(Array.Empty<SubGod>());
                    return;
                }

                God.ChangeSubGodRandom(new SubGod[1] { God.ActiveSubGod });
                return;
            }

            index--; // Offset the index to align the label indexes with the sub-god indexes, ignoring the random option
            God.ChangeSubGod(Targets[index]);
        }
    }
}
