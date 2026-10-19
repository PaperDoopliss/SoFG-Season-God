using Assets.Code;
using CommunityLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class UAEN_Season_HarvestRevelerTierIII : UAEN
    {
        public Rt_Season_RevelerRevelingTierIII rt_revel;
        public UAEN_Season_HarvestRevelerTierIII(Location loc, Society sg, Person p) : base(loc, sg, p)
        {
            p.shadow = 1.0;
            p.alert_aware = true;
            p.alert_maxShadow = true;
            p.alert_halfShadow = true;
            rt_revel = new Rt_Season_RevelerRevelingTierIII(loc, this);
            rituals.Add(rt_revel);
            p.receiveTrait(new T_UAEN_Reveler_Lord());
            p.receiveTrait(new T_UAEN_Reveler_Fey());
            p.stat_might = 4;
            p.stat_intrigue = 1;
            p.stat_lore = 3;
            p.stat_command = 1;
            p.species = map.species_monster;
            p.hasSoul = false;
        }

        public override bool definesName()
        {
            return true;
        }

        public override string getName()
        {
            return "Reveler Lord";
        }

        public override bool isCommandable()
        {
            return false;
        }

        public override Sprite getPortraitBackground()
        {
            return map.world.iconStore.standardBack;
        }

        public override Sprite getPortraitForeground()
        {
            return map.world.textureStore.evil_ghast;
        }

        public override void turnTickAI()
        {
            Kernel_Season.ComLibKernel.GetAgentAI().turnTickAI(this);

        }

        public static void populateReveler()
        {

            List<AIChallenge> list = new List<AIChallenge>();
            list.Add(new AIChallenge(typeof(Rt_Season_RevelerRevelingTierIII), 0.0, new List<AIChallenge.ChallengeTags>
            {
            }, safeMove: false, supportSubtypes: true));


            AgentAI.ControlParameters revelerParams = new AgentAI.ControlParameters(true);
            revelerParams.considerAllRituals = true;
            revelerParams.valueTimeCost = true; //This will automatically make them more inclined to do it in closer locations than further ones

            List<AIChallenge> list2 = list;
            list2[0].delegates_Utility.Add(delegate_Utility_Rt_Season_RevelerRevelingIII);
            list2[0].delegates_Valid.Add(delegate_Valid_Rt_Season_RevelerRevelingIII);
            list2[0].delegates_ValidFor.Add(delegate_ValidFor_Rt_Season_RevelerRevelingIII);
            ModCore.Get().GetAgentAI().RegisterAgentType(typeof(UAEN_Season_HarvestRevelerTierIII), revelerParams);
            ModCore.Get().GetAgentAI().AddChallengesToAgentType(typeof(UAEN_Season_HarvestRevelerTierIII), list2);

        }

        private static double delegate_Utility_Rt_Season_RevelerRevelingIII(AgentAI.ChallengeData challengeData, UA ua, double utility, List<ReasonMsg> reasonMsgs)
        {
            //Return their motivation number for this challenge - the higher it is, the more they'll do it here instead of somewhere else
            //Use challengeData.location to get the location they'll be considering doing it in

            double num = 0;
            double numUnrest = 0;
            double numDevestation = 0;

            Person ruler = challengeData.location.person();
            if (ruler != null && !ruler.isDead)
            {
                if (ruler.awareness > 0)
                {
                    num = ruler.awareness * 100;

                    reasonMsgs?.Add(new ReasonMsg("Ruler Awareness", num));
                    utility += num;
                }

                double num2 = challengeData.location.getShadow() * 100;
                num = 100 - num2;

                reasonMsgs?.Add(new ReasonMsg("Light Within " + challengeData.location.getName(), num));
                utility += num;
            }

            foreach (Property prop in challengeData.location.properties)
            {
                if (prop != null)
                {
                    if (prop is Pr_Unrest unrest)
                    {
                        numUnrest += unrest.charge;
                    }

                    if (prop is Pr_LingeringResentment devastation)
                    {
                        numDevestation += devastation.charge;
                    }
                }
            }

            if (numUnrest > 0)
            {
                reasonMsgs?.Add(new ReasonMsg("Unrest", numUnrest));
                utility += numUnrest;
            }

            if (numDevestation > 0)
            {
                reasonMsgs?.Add(new ReasonMsg("Lingering Resentment", numDevestation));
                utility += numDevestation;
            }

            return utility;
        }

        private static bool delegate_Valid_Rt_Season_RevelerRevelingIII(AgentAI.ChallengeData challengeData)
        {
            //Return true if the ritual can be performed in challengeData.location (probably if there's unrest or such there), otherwise return false

            SettlementHuman human = challengeData.location.settlement as SettlementHuman;
            if (human != null)
            {
                bool hasFeyPresence = false;
                bool hasEternalFestival = false;

                foreach (Property prop in challengeData.location.properties)
                {
                    if (prop != null)
                    {
                        if (prop is Pr_FeyPresence presence)
                        {
                            if (presence.charge >= 100)
                            {
                                hasFeyPresence = true;
                            }
                        }

                        if (prop is Pr_Season_RevelerHarvest)
                        {
                            if (prop.charge >= 1)
                            {
                                hasEternalFestival = true;
                            }
                        }
                    }
                }

                if (hasEternalFestival)
                {
                    return false;
                }

                if (hasFeyPresence)
                {
                    return true;
                }
            }


            return false;
        }

        private static bool delegate_ValidFor_Rt_Season_RevelerRevelingIII(AgentAI.ChallengeData challengeData, UA ua)
        {
            return true;
        }
    }
}
