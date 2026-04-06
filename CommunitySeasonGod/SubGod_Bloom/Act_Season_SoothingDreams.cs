using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Act_Season_SoothingDreams : Assets.Code.Action
    {
        public Pr_Season_DreamingKudzu kudzu;
        public static double kudzuCost = 50;
        public static double unrestToReduce = 50;
        public static double madnessToAdd = 10;

        public Act_Season_SoothingDreams(Location loc, Pr_Season_DreamingKudzu kudzu) : base(loc)
        {
            this.kudzu = kudzu;
        }

        public override string getName()
        {
            return "Soothing Dreams";
        }

        public override string getShortDesc()
        {
            return kudzuCost + "% Dreaming Kudzu is harvested and burned to drown out thoughts of rebellion, reducing Unrest by " + unrestToReduce + "% and increasing Madness by " + madnessToAdd + ".";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_dreaming_kudzu.png");
        }

        public override int getTurnsRequired()
        {
            return 3;
        }

        public override bool valid(Person ruler, SettlementHuman settlementHuman)
        {
            double unrest = 0;
            foreach (Property pr in settlementHuman.location.properties)
            {
                if (pr is Pr_Unrest)
                    unrest += pr.charge;
                else if (pr is Pr_Season_KudzuCrisis crisis)
                {
                    if (crisis.isCapital && crisis.exploitationOutlawed)
                        return false;
                    else if (crisis.isCapital == false && crisis.parent != null && crisis.parent.exploitationOutlawed)
                        return false;
                }
            }

            if (unrest > 0 && kudzu.charge >= kudzuCost)
                return true;

            return false;
        }

        public override double getUtility(SettlementHuman hum, Person ruler, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(hum, ruler, reasons);

            utility -= 25;
            reasons?.Add(new ReasonMsg("Base Reluctance", -25));

            double unrest = 0;
            double madness = 0;
            foreach (Property pr in hum.location.properties)
            {
                if (pr is Pr_Unrest)
                    unrest += pr.charge;
                else if (pr is Pr_Madness)
                    madness += pr.charge;
            }

            utility += unrest;
            reasons?.Add(new ReasonMsg("Level of Unrest", unrest));

            if (madness > 0)
            {
                utility -= madness;
                reasons?.Add(new ReasonMsg("Existing Madness", -madness));
            }

            return utility;
        }

        public override int[] getPositiveTags()
        {
            return new int[2] { Tags.MADNESS, map.soc_dark.index + 20000 };
        }

        public override int[] getNegativeTags()
        {
            return new int[1] { Tags.DISCORD };
        }

        public override void complete()
        {
            base.complete();

            Property.addToProperty("Burned Kudzu", Property.standardProperties.MADNESS, madnessToAdd, location);
            Property.addToProperty("Burned Kudzu", Property.standardProperties.UNREST, -unrestToReduce, location);
            kudzu.influences.Add(new ReasonMsg("Burned to Suppress Dissent", -kudzuCost));

            Pr_Season_KudzuCrisis crisis = null;
            foreach (Property pr in location.properties)
            {
                if (pr is Pr_Season_KudzuCrisis foundCrisis)
                {
                    crisis = foundCrisis;
                    break;
                }

            }

            if (crisis != null && location.soc != null)
            {
                if (crisis.isCapital == false && crisis.parent != null)
                    crisis.parent.exploitIntensity += (20f / (double)location.soc.lastTurnLocs.Count);
            }

        }

    }
}
