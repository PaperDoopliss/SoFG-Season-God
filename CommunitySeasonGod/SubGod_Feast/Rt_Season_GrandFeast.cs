using Assets.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_GrandFeast : Ritual
    {

        public static int cost = 100;

        public Rt_Season_GrandFeast(Location location) : base(location)
        {

        }

        public override string getName()
        {
            return "Grand Feast";
        }

        public override string getDesc()
        {
            return "Spends " + cost + " gold to bolster their Feyblood with an ostantatious feast of magical creatures, permanently increasing a random stat by 1, with a very low chance of increasing that stat by 3 instead. Feyblood and this action cannot increase any stat by more than 3 in total.";
        }

        public override string getRestriction()
        {
            return "Must be performed in a populated settlement by a person with at least " + cost + " <b>golb</b>";
        }

        public override double getComplexity()
        {
            return 3;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.nurture;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Base", 1));
            return 1;
        }

        public override bool valid()
        {
            return true;
        }

        public override bool validFor(UA ua)
        {
            if (ua.person?.gold >= cost)
            {
                if (ua.location.settlement is SettlementHuman)
                    return true;
            }
            return false;
        }

        public override string getCastFlavour()
        {
            return "The mystic beasts assembled for the feast spend their last moments in agony, the better to seal in the energies that animate them. Each dish is prepared with a mixed sophistication and brutality that leaves no room to hide from what they are doing.";
        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            double utility = base.getUtility(ua, msgs);

            msgs?.Add(new ReasonMsg("Base Motivation", 100));
            utility += 100;

            if (ua.person != null)
            {

                double modFromFeyblood = 0;
                foreach (Trait t in ua.person.traits)
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
                    msgs?.Add(new ReasonMsg("Maxed Out Stats", modFromFeyblood));
                    utility += modFromFeyblood;
                }
            }

            return utility;
        }

        public override void complete(UA u)
        {
            base.complete(u);

            if (u.person != null)
            {
                u.person.addGold(-cost);

                foreach (Trait t in u.person.traits)
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
                            {
                                int finalChange = Math.Min(3 - feyblood.buffMight, amountToBuff);
                                feyblood.buffMight += finalChange;
                                if (finalChange > 1 && u.isCommandable())
                                    map.addUnifiedMessage(u.person, null, "Feyblood Empowered", "As " + u.person.getName() + " completes their feast, the mystic energies they gather resonate with their blood unpredictably. They find themselves helplessly propelled to greater power and greater hunger.\n\n" + u.person.getName() + " gains +" + finalChange + " Might.", "FEYBLOOD EMPOWERED");
                            }
                            else if (statToBuff == 1)
                            {
                                int finalChange = Math.Min(3 - feyblood.buffLore, amountToBuff);
                                feyblood.buffLore += finalChange;
                                if (finalChange > 1 && u.isCommandable())
                                    map.addUnifiedMessage(u.person, null, "Feyblood Empowered", "As " + u.person.getName() + " completes their feast, the mystic energies they gather resonate with their blood unpredictably. They find themselves helplessly propelled to greater power and greater hunger.\n\n" + u.person.getName() + " gains +" + finalChange + " Lore.", "FEYBLOOD EMPOWERED");

                            }
                            else if (statToBuff == 2)
                            {
                                int finalChange = Math.Min(3 - feyblood.buffIntrigue, amountToBuff);
                                feyblood.buffIntrigue += finalChange;
                                if (finalChange > 1 && u.isCommandable())
                                    map.addUnifiedMessage(u.person, null, "Feyblood Empowered", "As " + u.person.getName() + " completes their feast, the mystic energies they gather resonate with their blood unpredictably. They find themselves helplessly propelled to greater power and greater hunger.\n\n" + u.person.getName() + " gains +" + finalChange + " Intrigue.", "FEYBLOOD EMPOWERED");

                            }
                            else
                            {
                                int finalChange = Math.Min(3 - feyblood.buffCommand, amountToBuff);
                                feyblood.buffCommand += finalChange;
                                if (finalChange > 1 && u.isCommandable())
                                    map.addUnifiedMessage(u.person, null, "Feyblood Empowered", "As " + u.person.getName() + " completes their feast, the mystic energies they gather resonate with their blood unpredictably. They find themselves helplessly propelled to greater power and greater hunger.\n\n" + u.person.getName() + " gains +" + finalChange + " Command.", "FEYBLOOD EMPOWERED");
                            }
                        }

                    }
                }
            }
        }

    }
}
