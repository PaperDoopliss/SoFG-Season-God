using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Act_Season_Tournament : Assets.Code.Action
    {
        public static int cost = 100;
        public static double boost = 50;

        public Act_Season_Tournament(Location loc) : base(loc)
        {
        }

        public override string getName()
        {
            return "Tournament";
        }

        public override string getShortDesc()
        {
            return "Spends " + cost + " gold to host a great tournament, whipping the soldiery into a frenzy and increasing Military Fervour in the location by " + boost + "%. This action can only be performed in a settlement that has an army.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_pale_knights_summon.png");
        }

        public override int[] getPositiveTags()
        {
            return new int[] { Tags.COMBAT };
        }

        public override int getTurnsRequired()
        {
            return 5;
        }

        public override bool valid(Person ruler, SettlementHuman settlementHuman)
        {
            return ruler.gold >= cost && settlementHuman.supportedMilitary != null;
        }

        public override double getUtility(SettlementHuman hum, Person ruler, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(hum, ruler, reasons);

            reasons?.Add(new ReasonMsg("Base Motivation", 75));
            utility += 75;

            double unrest = 0;
            double fervor = 0;

            foreach (Property pr in hum.location.properties)
            {
                if (pr is Pr_Unrest)
                    unrest += pr.charge * 0.5;
                else if (pr is Pr_MilitaryFervor)
                    fervor += pr.charge * 0.5;
            }

            unrest = Math.Round(unrest);
            if (unrest > 0)
            {
                reasons?.Add(new ReasonMsg("Unrest", -unrest));
                utility -= unrest;
            }

            fervor = Math.Round(fervor);
            if (fervor > 0)
            {
                reasons?.Add(new ReasonMsg("Existing Military Fervour", -fervor));
                utility -= fervor;
            }

            return utility;
        }

        public override void complete()
        {
            base.complete();

            if (location.settlement is SettlementHuman sh && sh.ruler != null)
            {
                sh.ruler.addGold(-cost);
                Property.addToPropertySingleShot("Tournament", Property.standardProperties.MILITARY_FERVOUR, boost, location);
            }
        }

    }
}
