using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    class T_Supplicant_PatriarchsWarmth : Trait
    {

        public static float heatPercentagePerTurn = 0.25f;

        public T_Supplicant_PatriarchsWarmth() : base()
        {
        }

        public override string getName()
        {
            return "Patriarch's Warmth";
        }

        public override string getDesc()
        {
            return "The temperature at this person's location increases by " + heatPercentagePerTurn + "% per turn, and their menace increases by the same amount.";
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);
            if (p.unit != null)
            {
                foreach (Hex hex in p.unit.location.territory)
                {
                    hex.transientTempDelta += heatPercentagePerTurn / 100f;
                }

                p.unit.addMenace(heatPercentagePerTurn);
            }

        }

    }
}