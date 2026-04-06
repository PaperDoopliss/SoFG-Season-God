using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Act_Season_PsychicCoercion : Assets.Code.Action
    {
        public Pr_Season_DreamingKudzu kudzu;
        public static double kudzuCost = 50;
        public static double instabilityToReduce = 50;
        public static int numTargets = 3;
        public static int sanityDamage = 3;

        public Act_Season_PsychicCoercion(Location loc, Pr_Season_DreamingKudzu kudzu) : base(loc)
        {
            this.kudzu = kudzu;
        }

        public override string getName()
        {
            return "Psychic Coercion";
        }

        public override string getShortDesc()
        {
            return kudzuCost + "% Dreaming Kudzu is harvested and burned to inflict nightmares on rebellious vassals, reducing Political Instability by " + instabilityToReduce + ". Up to " + numTargets + " rulers with Political Agitation at their location will lose " + sanityDamage + " <b>sanity</b> and gain liking for the sovereign.";
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
            double instability = 0;
            foreach (Property pr in settlementHuman.location.properties)
            {
                if (pr is Pr_PoliticalInstability)
                    instability += pr.charge;
                else if (pr is Pr_Season_KudzuCrisis crisis)
                {
                    if (crisis.isCapital && crisis.exploitationOutlawed)
                        return false;
                    else if (crisis.isCapital == false && crisis.parent != null && crisis.parent.exploitationOutlawed)
                        return false;
                }
            }

            if (instability > 0 && kudzu.charge >= kudzuCost)
                return true;

            return false;
        }

        public override double getUtility(SettlementHuman hum, Person ruler, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(hum, ruler, reasons);

            utility -= 25;
            reasons?.Add(new ReasonMsg("Base Reluctance", -25));

            double instability = 0;
            foreach (Property pr in hum.location.properties)
            {
                if (pr is Pr_PoliticalInstability)
                    instability += pr.charge;
            }

            utility += instability;
            reasons?.Add(new ReasonMsg("Level of Instability", instability));

            return utility;
        }

        public override int[] getPositiveTags()
        {
            return new int[3] { Tags.MADNESS, map.soc_dark.index + 20000, Tags.COOPERATION };
        }

        public override int[] getNegativeTags()
        {
            return new int[1] { Tags.DISCORD };
        }

        public override void complete()
        {
            base.complete();

            double instabilityLost = 0;
            for (int i = 0; i < location.properties.Count; i++)
            {
                if (location.properties[i] is Pr_PoliticalInstability)
                {
                    if (location.properties[i].charge <= instabilityToReduce - instabilityLost)
                    {
                        instabilityLost += location.properties[i].charge;
                        location.properties.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        location.properties[i].charge -= instabilityToReduce - instabilityLost;
                        instabilityLost = instabilityToReduce;
                        break;
                    }
                }
            }


            Property.addToProperty("Burned Kudzu", Property.standardProperties.POLITICAL_INSTABILITY, -instabilityToReduce, location);
            kudzu.influences.Add(new ReasonMsg("Burned to Suppress Vassals", -kudzuCost));

            Person ruler = null;
            if (location.settlement is SettlementHuman sh2 && sh2.ruler != null)
                ruler = sh2.ruler;


            if (location.soc != null)
            {

                List<Person> victims = new List<Person>();
                List<Person> confirmedVictims = new List<Person>();

                foreach (Location l in location.soc.lastTurnLocs)
                {
                    if (l == location || l.soc != location.soc)
                        continue;
                    foreach (Property pr in l.properties)
                    {
                        if (pr is Pr_PoliticalAgitation)
                        {
                            if (l.settlement is SettlementHuman sh && sh.ruler != null)
                            {
                                if (ruler != sh.ruler)
                                {
                                    victims.Add(sh.ruler);
                                }
                            }

                            continue;
                        }
                    }
                }

                if (ruler != null) 
                { 
                
                    for (int i = 0; i < numTargets; i++)
                    {
                        if (victims.Count == 0)
                            break;
                    
                        Person target = victims[Eleven.random.Next(victims.Count)];
                        target.sanity -= sanityDamage;
                        target.increasePreference(ruler.index + 10000);
                        confirmedVictims.Add(target);
                        victims.Remove(target);
                    }

                    if (confirmedVictims.Count > 0)
                    {
                        string victimNames = confirmedVictims[0].getName();
                        for (int i = 1; i < confirmedVictims.Count; i++)
                        {
                            if (i == confirmedVictims.Count - 1)
                                victimNames += " and " + confirmedVictims[i];
                            else
                                victimNames += ", " + confirmedVictims[i] + ",";
                        }
                        if (confirmedVictims.Count == 1)
                            victimNames += " has lost " + sanityDamage + " <b>sanity</b> and gained liking for " + ruler.getName() + ".";
                        else
                            victimNames += " have lost " + sanityDamage + " <b>sanity</b> and gained liking for " + ruler.getName() + ".";

                        map.addUnifiedMessage(ruler, confirmedVictims[0], "Psychic Coercion", ruler.getName() + " has exploited the Dreaming Kudzu to manipulate the minds of their vassals. " + location.getName() + " has lost " + instabilityLost + "% Political Instability, and " + victimNames, "PSYCHIC COERCION");
                    }
                }

            }
        }

    }
}
