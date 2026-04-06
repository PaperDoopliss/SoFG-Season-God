using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_DreamingKudzu : Property
    {

        public static double standardIncrease = 2;
        public static double enshadowedBoostAtHalf = 1;
        public static double enshadowedBoostAtFull = 1;

        public static double presencePerCharge = 0.01;
        public static double habitabilityMultPerHundred = 0.01;

        public static double madnessOnCrisis = 150;
        public static double presenceOnCrisis = 150;

        public bool popped = false;

        public Act_Season_SoothingDreams act_dreams;
        public Act_Season_PsychicCoercion act_coercion;
        public Ch_Season_CutBackKudzu ch_cut;

        public Pr_Season_DreamingKudzu(Location loc) : base(loc) 
        {
            act_dreams = new Act_Season_SoothingDreams(loc,this);
            act_coercion = new Act_Season_PsychicCoercion(loc,this);
            ch_cut = new Ch_Season_CutBackKudzu(loc, this);
        }

        public override string getName()
        {
            return "Dreaming Kudzu";
        }

        public override string getDesc()
        {
            return "Psychoactive vines spread rapidly, bestowing strange dreams and smothering native plant life. Every 100% Dreaming Kudzu reduces local <b>habitability</b> by " + habitabilityMultPerHundred * 100 + "%, and increases Fey Presence by " + presencePerCharge * 100 + "%. Dreaming Kudzu will naturally grow by " + standardIncrease + "% per turn, increasing to " + standardIncrease + enshadowedBoostAtFull + + enshadowedBoostAtHalf + "% if fully enshadowed, and will spread to neighbouring locations if it reaches " + map.param.prop_plagueSpreadReq + "% (" + map.param.prop_plagueSpreadReqQuarantine + "% if quarantined). The first time this property hits 300%, the location gains " + madnessOnCrisis + "% Madness and " + presenceOnCrisis + "% Fey Presence. Heroes and rulers who are Aware or dislike Madness gain tools to cut back the kudzu.";
        }

        public override Sprite getSprite(World world)
        {
            return EventManager.getImg("ComSeasonGod.power_dreaming_kudzu.png");
        }

        public override bool removedOnRuin()
        {
            return false;
        }

        public override bool survivesRuin()
        {
            return true;
        }

        public override bool hasHexView()
        {
            return true;
        }
        public override Sprite hexViewSprite()
        {
            return EventManager.getImg("ComSeasonGod.property_dreaming_kudzu_hex.png");
        }

        public override List<Challenge> getChallenges()
        {
            List<Challenge> result = new List<Challenge>();
            result.Add(ch_cut);
            return result;
        }

        public override List<Assets.Code.Action> getActions()
        {
            List<Assets.Code.Action> result = new List<Assets.Code.Action>();

            result.Add(act_dreams);
            if (location.soc != null && location.soc.getCapitalHex() == location.hex)
                result.Add(act_coercion);

            return result;
        }

        //Remove it manually in TurnTick instead so you can make sure the Purged Kudzu modifier is spawning
        public override bool deleteOnZero()
        {
            return false;
        }

        public override void turnTick()
        {
            base.turnTick();

            if (charge <= 0)
            {
                location.properties.Remove(this);
                location.properties.Add(new Pr_Season_PurgedKudzu(location));
                return;
            }

            influences.Add(new ReasonMsg("Natural Growth", standardIncrease));
            if (location.getShadow() >= 1)
                influences.Add(new ReasonMsg("Growth from Shadow", enshadowedBoostAtHalf + enshadowedBoostAtFull));
            else if (location.getShadow() >= 0.5)
                influences.Add(new ReasonMsg("Growth from Shadow", enshadowedBoostAtHalf));


            bool hasCrisisProperty = false;
            Pr_FeyPresence presence = null;
            bool hasQuarantine = false;
            foreach (Property pr in location.properties)
            {
                if (pr is Pr_Season_KudzuCrisis)
                {
                    hasCrisisProperty = true;
                }
                else if (pr is Pr_FeyPresence foundPresence)
                {
                    presence = foundPresence;
                    pr.influences.Add(new ReasonMsg("Dreaming Kudzu", charge * presencePerCharge));
                }
                else if (pr is Pr_Quarantine)
                    hasQuarantine = true;
            }

            if (!hasCrisisProperty && location.settlement is SettlementHuman)
            {
                if (location.soc != null && location.soc.getCapitalHex() == location.hex)
                {
                    location.properties.Add(new Pr_Season_KudzuCrisis(true, location));
                }
                else
                {
                    location.properties.Add(new Pr_Season_KudzuCrisis(false, location));
                }

            }



            if (charge > map.param.prop_plagueSpreadReq && (hasQuarantine == false || charge > map.param.prop_plagueSpreadReqQuarantine))
            {
                foreach (Location l in location.getNeighbours())
                {
                    if (l.isOcean)
                        continue;

                    bool foundKudzu = false;
                    foreach (Property pr in l.properties)
                    {
                        if (pr is Pr_Season_DreamingKudzu)
                        {
                            pr.influences.Add(new ReasonMsg("Spread from Neighbour", 1));
                            foundKudzu = true;
                            break;
                        }
                    }

                    if (!foundKudzu)
                    {
                        Pr_Season_DreamingKudzu kudzu = new Pr_Season_DreamingKudzu(l);
                        kudzu.charge = 1;
                        l.properties.Add(kudzu);
                    }
                }
            }

            if (charge >= 300)
            {
                charge = 300;

                if (!popped)
                {
                    popped = true;
                    if (presence != null)
                        presence.charge += presenceOnCrisis;
                    else
                    {
                        presence = new Pr_FeyPresence(location);
                        presence.charge = presenceOnCrisis;
                        location.properties.Add(presence);
                    }

                    Property.addToPropertySingleShot("Dreaming Kudzu", standardProperties.MADNESS, madnessOnCrisis, location);

                    if (!EventManager.events.TryGetValue("ComSeasonGod.dreaming_kudzu", out EventManager.ActiveEvent ae))
                    {
                        Console.WriteLine("ComSeasonGod: Unable to find Dreaming Kudzu event (\"ComSeasonGod.dreaming_kudzu\").");
                        map.addMessage("ERROR: Unable to find Dreaming Kudzu event (\"ComSeasonGod.dreaming_kudzu\").", 1.0, false);
                        return;
                    }
                    map.world.prefabStore.popEvent(ae.data, EventContext.withLocation(map, location), null, false);
                }
            }
        }


    }
}
