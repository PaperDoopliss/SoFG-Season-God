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
    public class UAEN_Season_HarvestRevelerTierI : UAEN
    {
        public Rt_Season_RevelerRevelingTierI rt_revel;
        public UAEN_Season_HarvestRevelerTierI(Location loc, Society sg, Person p) : base(loc, sg, p)
        {
            p.shadow = 1.0;
            p.alert_aware = true;
            p.alert_maxShadow = true;
            p.alert_halfShadow = true;
            rt_revel = new Rt_Season_RevelerRevelingTierI(loc, this);
            rituals.Add(rt_revel);
            p.receiveTrait(new T_UAEN_Reveler());
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
            return "Jolly Reveler";
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
            list.Add(new AIChallenge(typeof(Rt_Season_RevelerRevelingTierI), 0.0, new List<AIChallenge.ChallengeTags>
            {
            }, safeMove: false, supportSubtypes: true));


            AgentAI.ControlParameters revelerParams = new AgentAI.ControlParameters(true);
            revelerParams.considerAllRituals = true;
            revelerParams.valueTimeCost = true; //This will automatically make them more inclined to do it in closer locations than further ones

            List<AIChallenge> list2 = list;
            list2[0].delegates_Utility.Add(delegate_Utility_Rt_Season_RevelerReveling);
            list2[0].delegates_Valid.Add(delegate_Valid_Rt_Season_RevelerReveling);
            list2[0].delegates_ValidFor.Add(delegate_ValidFor_Rt_Season_RevelerReveling);
            ModCore.Get().GetAgentAI().RegisterAgentType(typeof(UAEN_Season_HarvestRevelerTierI), revelerParams);
            ModCore.Get().GetAgentAI().AddChallengesToAgentType(typeof(UAEN_Season_HarvestRevelerTierI), list2);

        }

        private static double delegate_Utility_Rt_Season_RevelerReveling(AgentAI.ChallengeData challengeData, UA ua, double utility, List<ReasonMsg> reasonMsgs)
        {
            //Return their motivation number for this challenge - the higher it is, the more they'll do it here instead of somewhere else
            //Use challengeData.location to get the location they'll be considering doing it in

            double num = 0;
            double numUnrest = 0;
            double numDevestation = 0;
            bool hasEternalRevelry = false;

            foreach (Property prop in challengeData.location.properties)
            {
                if (prop != null)
                {
                    if (prop is Pr_FeyPresence presence)
                    {
                        num = 100 - presence.charge;
                    }

                    if (prop is Pr_Unrest unrest)
                    {
                        numUnrest += unrest.charge;
                    }

                    if (prop is Pr_Devastation devastation)
                    {
                        numDevestation += devastation.charge;
                    }

                    if (prop is Pr_Season_RevelerHarvest revelry)
                    {
                        hasEternalRevelry = true;
                    }
                }
            }


            if(num > 0)
            {
                reasonMsgs?.Add(new ReasonMsg("Fey Presence Under 100% charge", num));
                utility += num;
            }

            if (numUnrest > 0)
            {
                reasonMsgs?.Add(new ReasonMsg("Unrest", numUnrest));
                utility += numUnrest;
            }

            if (numDevestation > 0)
            {
                reasonMsgs?.Add(new ReasonMsg("Devastation", numDevestation));
                utility += numDevestation;
            }

            if(hasEternalRevelry)
            {
                num = 30;
                reasonMsgs?.Add(new ReasonMsg("Eternal Revelries", num));
                utility += num;
            }

            return utility;
        }

        private static bool delegate_Valid_Rt_Season_RevelerReveling(AgentAI.ChallengeData challengeData)
        {
            //Return true if the ritual can be performed in challengeData.location (probably if there's unrest or such there), otherwise return false
            bool canBeValid = false;
            bool butFeyPresenceatMax = false;

            SettlementHuman human = challengeData.location.settlement as SettlementHuman;
            if(human != null)
            {
                foreach (Property prop in challengeData.location.properties)
                {
                    if (prop != null)
                    {
                        if (prop is Pr_Unrest unrest)
                        {
                            if (unrest.charge >= 1)
                            {
                                canBeValid = true;
                            }
                        }

                        if (prop is Pr_Devastation devastation)
                        {
                            if (devastation.charge >= 1)
                            {
                                canBeValid = true;
                            }
                        }

                        if (prop is Pr_FeyPresence fey)
                        {
                            if (fey.charge >= 100)
                            {
                                butFeyPresenceatMax = true;
                            }
                        }
                    }
                }

                if (butFeyPresenceatMax)
                {
                    canBeValid = false;
                }
            }


            return canBeValid;
        }

        private static bool delegate_ValidFor_Rt_Season_RevelerReveling(AgentAI.ChallengeData challengeData, UA ua)
        {
            return true;
        }
    }
}
