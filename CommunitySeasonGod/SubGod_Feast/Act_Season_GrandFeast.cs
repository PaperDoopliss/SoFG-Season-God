using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Act_Season_GrandFeast : Assets.Code.Action
    {
        public static int cost = 100;

        public Act_Season_GrandFeast(Location loc) : base(loc)
        {
        }

        public override string getName()
        {
            return "Grand Feast";
        }

        public override string getShortDesc()
        {
            return "Spends " + cost + " gold to bolster their Feyblood with an ostantatious feast of magical creatures, permanently increasing a random stat by 1, with a very low chance of increasing that stat by 3 instead. Feyblood and this action cannot increase any stat by more than 3 in total.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.nurture;
        }

        public override int getTurnsRequired()
        {
            return 5;
        }

        public override bool valid(Person ruler, SettlementHuman settlementHuman)
        {
            return ruler.gold >= cost;
        }

        public override double getUtility(SettlementHuman hum, Person ruler, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(hum, ruler, reasons);

            reasons?.Add(new ReasonMsg("Base Motivation", 80));
            utility += 80;

            double unrest = 0;

            foreach (Property pr in hum.location.properties)
            {
                if (pr is Pr_Unrest)
                    unrest += pr.charge * 0.5;
            }

            unrest = Math.Round(unrest);
            if (unrest > 0)
            {
                reasons?.Add(new ReasonMsg("Unrest", -unrest));
                utility -= unrest;
            }

            if (ruler != null)
            {

                double modFromFeyblood = 0;
                foreach (Trait t in ruler.traits)
                {
                    if (t is T_Season_Feyblood feyblood)
                    {
                        if (feyblood.buffMight >= 3)
                            modFromFeyblood -= 10;
                        if (feyblood.buffLore >= 3)
                            modFromFeyblood -= 10;
                        if (feyblood.buffIntrigue >= 3)
                            modFromFeyblood -= 10;
                        if (feyblood.buffCommand >= 3)
                            modFromFeyblood -= 10;
                    }
                }

                if (modFromFeyblood < 0)
                {
                    reasons?.Add(new ReasonMsg("Maxed Out Stats", modFromFeyblood));
                    utility += modFromFeyblood;
                }
            }

            return utility;
        }

        public override void complete()
        {
            base.complete();

            if (location.settlement is SettlementHuman sh)
            {
                if (sh.ruler != null)
                {
                    sh.ruler.addGold(-cost);

                    foreach (Trait t in sh.ruler.traits)
                    {
                        if (t is T_Season_Feyblood feyblood)
                        {
                            List<int> nonMaxedStats = new List<int>();
                            if (feyblood.buffMight < 3)
                                nonMaxedStats.Add(0);
                            if (feyblood.buffLore < 3)
                                nonMaxedStats.Add(1);
                            if (feyblood.buffIntrigue < 3)
                                nonMaxedStats.Add(2);
                            if (feyblood.buffCommand < 3)
                                nonMaxedStats.Add(3);

                            if (nonMaxedStats.Count > 0)
                            {
                                int amountToBuff = 1;
                                if (Eleven.random.Next(10) == 0)
                                    amountToBuff = 3;


                                int statToBuff = nonMaxedStats[Eleven.random.Next(nonMaxedStats.Count)];
                                if (statToBuff == 0)
                                    feyblood.buffMight = Math.Min(3, feyblood.buffMight + amountToBuff);
                                else if (statToBuff == 1)
                                    feyblood.buffLore = Math.Min(3, feyblood.buffLore + amountToBuff);
                                else if (statToBuff == 2)
                                    feyblood.buffIntrigue = Math.Min(3, feyblood.buffIntrigue + amountToBuff);
                                else
                                    feyblood.buffCommand = Math.Min(3, feyblood.buffCommand + amountToBuff);
                            }

                        }
                    }
                }
            }
        }

    }
}
