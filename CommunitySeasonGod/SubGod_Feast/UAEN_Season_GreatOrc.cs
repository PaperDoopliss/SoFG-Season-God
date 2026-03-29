using Assets.Code;
using CommunityLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace CommunitySeasonGod
{
    public class UAEN_Season_GreatOrc : UAEN_OrcUpstart
    {
        public UAEN_Season_GreatOrc(Location loc, SocialGroup sg, Person p) : base(loc, sg, p)
        {

            bool hasGreatOrcTrait = false;

            foreach (Trait t in person.traits)
            {
                if (t is T_Season_GreatOrc)
                    hasGreatOrcTrait = true;
            }
            if (!hasGreatOrcTrait)
                person.receiveTrait(new T_Season_GreatOrc());

            rituals.Add(new Rt_Season_GreatOrcRaiding(loc));
        }

        public override string getName()
        {
            return "Great Orc";
        }

        public override Sprite getPortraitForeground()
        {
            return EventManager.getImg("ComSeasonGod.unit_great_orc.png");
        }

        public override void turnTickAI()
        {
            Kernel_Season.ComLibKernel.GetAgentAI().turnTickAI(this);

        }

        public static void populateGreatOrc()
        {

            List<AIChallenge> list = new List<AIChallenge>();
            list.Add(new AIChallenge(typeof(Rt_Season_GreatOrcRaiding), 0.0, new List<AIChallenge.ChallengeTags>
        {
            /*AIChallenge.ChallengeTags.BaseValid,
            AIChallenge.ChallengeTags.BaseValidFor,*/
        }, safeMove: false, supportSubtypes: true));
            list.Add(new AIChallenge(typeof(Ch_RecruitMinion), 0.0, new List<AIChallenge.ChallengeTags>
        {
            AIChallenge.ChallengeTags.RequiresOwnSociety,
            AIChallenge.ChallengeTags.RecruitsMinion
        }, safeMove: false, supportSubtypes: true));
            list.Add(new AIChallenge(typeof(Ch_Rest_InOrcCamp), 0.0, new List<AIChallenge.ChallengeTags>
        {
            AIChallenge.ChallengeTags.RequiresOwnSociety,
            AIChallenge.ChallengeTags.HealOrc,
            AIChallenge.ChallengeTags.Rest
        }, safeMove: false, supportSubtypes: true));

            AgentAI.ControlParameters orcParams = new AgentAI.ControlParameters(true);
            orcParams.considerAllRituals = true;

            List<AIChallenge> list2 = list;
            list2[0].delegates_Utility.Add(delegate_Utility_Rt_Season_GreatOrcRaiding);
            list2[0].delegates_Valid.Add(delegate_Valid_Rt_Season_GreatOrcRaiding);
            list2[0].delegates_ValidFor.Add(delegate_ValidFor_Rt_Season_GreatOrcRaiding);
            list2[1].delegates_Valid.Add(delegate_Valid_Ch_RecruitMinion);
            list2[2].delegates_Utility.Add(delegate_Utility_Ch_Rest_InOrcCamp);
            ModCore.Get().GetAgentAI().RegisterAgentType(typeof(UAEN_Season_GreatOrc), /*new AgentAI.ControlParameters(isDark: true)*/orcParams);
            ModCore.Get().GetAgentAI().AddChallengesToAgentType(typeof(UAEN_Season_GreatOrc), list2);
            //if (ModCore.Get().GetAgentAI().TryGetAgentType(typeof(UAEN_Season_GreatOrc), out var aiData))
            //{
                //aiData?.aiChallenges_UniversalDelegates_ValidFor.Add(universalDelegate_ValidFor_Underground);
            //}



        }

        private static double delegate_Utility_Rt_Season_GreatOrcRaiding(AgentAI.ChallengeData challengeData, UA ua, double utility, List<ReasonMsg> reasonMsgs)
        {
            reasonMsgs?.Add(new ReasonMsg("Base", 50));
            reasonMsgs?.Add(new ReasonMsg("Distance from Home", ua.map.getStepDist(challengeData.location, ua.map.locations[ua.homeLocation]) * -1));

            double distanceReduction = ua.map.getStepDist(challengeData.location, ua.map.locations[ua.homeLocation]);
            return 50 - distanceReduction;

        }

        private static bool delegate_Valid_Rt_Season_GreatOrcRaiding(AgentAI.ChallengeData challengeData)
        {

            if (challengeData.location.settlement is SettlementHuman)
            {
                return true;
            }

            return false;
        }

        private static bool delegate_ValidFor_Rt_Season_GreatOrcRaiding(AgentAI.ChallengeData challengeData, UA ua)
        {
            return true;
        }

        private static bool delegate_Valid_Ch_RecruitMinion(AgentAI.ChallengeData challengeData)
        {
            return true;
        }

        private static double delegate_Utility_Ch_Rest_InOrcCamp(AgentAI.ChallengeData challengeData, UA ua, double utility, List<ReasonMsg> reasonMsgs)
        {
            utility -= (double)ua.map.param.ch_rest_parameterValue1;
            utility += 1.0;
            if (reasonMsgs != null)
            {
                ReasonMsg reasonMsg = reasonMsgs.FirstOrDefault((ReasonMsg m) => m.msg == "Base");
                if (reasonMsg != null)
                {
                    reasonMsg.value = 1.0;
                }
            }

            return utility;
        }


        /*private void populateDeepOne()
        {
            List<AIChallenge> list = new List<AIChallenge>();
            list.Add(new AIChallenge(typeof(Rt_DeepOneReproduce), 0.0, new List<AIChallenge.ChallengeTags> { AIChallenge.ChallengeTags.Aquaphibious }, safeMove: true, supportSubtypes: true));
            list.Add(new AIChallenge(typeof(Ch_DeepOnesHumanAppearance), 0.0, new List<AIChallenge.ChallengeTags> { AIChallenge.ChallengeTags.Aquaphibious }, safeMove: true, supportSubtypes: true));
            list.Add(new AIChallenge(typeof(Ch_ConcealDeepOnes), 0.0, new List<AIChallenge.ChallengeTags> { AIChallenge.ChallengeTags.Aquaphibious }, safeMove: true, supportSubtypes: true));
            List<AIChallenge> list2 = list;
            list2[0].delegates_Valid.Add(delegate_Valid_Rt_DeepOneReproduce);
            list2[0].delegates_Utility.Add(delegate_Utility_Rt_DeepOneReproduce);
            list2[1].delegates_Valid.Add(delegate_Valid_Ch_DeepOnesHumanAppearance);
            list2[1].delegates_Utility.Add(delegate_Utility_Ch_DeepOnesHumanAppearance);
            list2[2].delegates_Valid.Add(delegate_Valid_Ch_ConcealDeepOnes);
            list2[2].delegates_Utility.Add(delegate_Utility_Ch_ConcealDeepOnes);
            ModCore.Get().GetAgentAI().RegisterAgentType(typeof(UAEN_DeepOne), new AgentAI.ControlParameters(isDark: true));
            ModCore.Get().GetAgentAI().AddChallengesToAgentType(typeof(UAEN_DeepOne), list2);
            AITask aITask = new AITask(typeof(Task_ReturnToTheDeep), "Return to the Deep", map, delegate_Instantiate_ReturnDeep, AITask.TargetCategory.None, null, foregroundSprite: map.world.iconStore.hideInAbyss, colour: new Color(0.2f, 0.2f, 0.7f));
            aITask.delegates_Valid.Add(delegate_Validity_ReturnDeep);
            aITask.delegates_Utility.Add(delegate_Utility_ReturnDeep);
            ModCore.Get().GetAgentAI().AddTaskToAgentType(typeof(UAEN_DeepOne), aITask);
        }

        private bool delegate_Valid_Rt_DeepOneReproduce(AgentAI.ChallengeData challengeData)
        {
            SettlementHuman settlementHuman = challengeData.location.settlement as SettlementHuman;
            Society obj = challengeData.location.soc as Society;
            if (obj != null && obj.isOphanimControlled)
            {
                return false;
            }

            if (settlementHuman != null)
            {
                if (settlementHuman.ophanimTakeOver)
                {
                    return false;
                }

                if (ModCore.Get().data.tryGetModIntegrationData("AberrantMetal", out var intData) && intData.typeDict.TryGetValue("Factory", out var value) && (settlementHuman.GetType() == value || settlementHuman.GetType().IsSubclassOf(value)))
                {
                    return false;
                }

                if (!challengeData.location.properties.Any((Property pr) => pr is Pr_DeepOneCult))
                {
                    return true;
                }
            }

            return false;
        }

        private double delegate_Utility_Rt_DeepOneReproduce(AgentAI.ChallengeData challengeData, UA ua, double utility, List<ReasonMsg> reasonMsgs)
        {
            double num = 100.0;
            reasonMsgs?.Add(new ReasonMsg("Base", num));
            utility += num;
            return utility;
        }

        private bool delegate_Valid_Ch_DeepOnesHumanAppearance(AgentAI.ChallengeData challengeData)
        {
            Pr_DeepOneCult pr_DeepOneCult = (challengeData.challenge as Ch_DeepOnesHumanAppearance)?.deepOnes;
            if (pr_DeepOneCult != null && pr_DeepOneCult.menace > 25.0)
            {
                return true;
            }

            return false;
        }

        private double delegate_Utility_Ch_DeepOnesHumanAppearance(AgentAI.ChallengeData challengeData, UA ua, double utility, List<ReasonMsg> reasonMsgs)
        {
            Pr_DeepOneCult pr_DeepOneCult = (challengeData.challenge as Ch_DeepOnesHumanAppearance)?.deepOnes;
            if (pr_DeepOneCult != null && pr_DeepOneCult.menace > 25.0)
            {
                double num = pr_DeepOneCult.menace * 5.0;
                reasonMsgs?.Add(new ReasonMsg("Potential Menace Reduction", num));
                utility += num;
            }

            return utility;
        }

        private bool delegate_Valid_Ch_ConcealDeepOnes(AgentAI.ChallengeData challengeData)
        {
            Pr_DeepOneCult pr_DeepOneCult = (challengeData.challenge as Ch_ConcealDeepOnes)?.deepOnes;
            if (pr_DeepOneCult != null && pr_DeepOneCult.profile > 25.0)
            {
                return true;
            }

            return false;
        }

        private double delegate_Utility_Ch_ConcealDeepOnes(AgentAI.ChallengeData challengeData, UA ua, double utility, List<ReasonMsg> reasonMsgs)
        {
            Pr_DeepOneCult pr_DeepOneCult = (challengeData.challenge as Ch_ConcealDeepOnes)?.deepOnes;
            if (pr_DeepOneCult != null && pr_DeepOneCult.profile > 25.0)
            {
                double num = pr_DeepOneCult.profile * 5.0;
                reasonMsgs?.Add(new ReasonMsg("Potential Profile Reduction", num));
                utility += num;
            }

            return utility;
        }

        private Task delegate_Instantiate_ReturnDeep(UA ua, AITask.TargetCategory targetCategory, AgentAI.TaskData taskData)
        {
            return new Task_ReturnToTheDeep(-1);
        }

        private bool delegate_Validity_ReturnDeep(UA ua, AITask.TargetCategory targetCategory, AgentAI.TaskData taskData)
        {
            if (targetCategory == AITask.TargetCategory.None && ua.moveType == Unit.MoveType.NORMAL)
            {
                return true;
            }

            return false;
        }

        private double delegate_Utility_ReturnDeep(UA ua, AITask.TargetCategory targetCategory, AgentAI.TaskData taskData, List<ReasonMsg> reasonMsgs)
        {
            double num = 10000.0;
            reasonMsgs?.Add(new ReasonMsg("Must Return to the Deep", num));
            return num;
        }*/









        /*private void populateOrcUpstart()
        {
            List<AIChallenge> list = new List<AIChallenge>();
            list.Add(new AIChallenge(typeof(Ch_OrcRaiding), 0.0, new List<AIChallenge.ChallengeTags>
        {
            AIChallenge.ChallengeTags.BaseValid,
            AIChallenge.ChallengeTags.BaseValidFor,
            AIChallenge.ChallengeTags.RequiresOwnSociety
        }, safeMove: false, supportSubtypes: true));
            list.Add(new AIChallenge(typeof(Ch_RecruitMinion), 0.0, new List<AIChallenge.ChallengeTags>
        {
            AIChallenge.ChallengeTags.RequiresOwnSociety,
            AIChallenge.ChallengeTags.RecruitsMinion
        }, safeMove: false, supportSubtypes: true));
            list.Add(new AIChallenge(typeof(Ch_Rest_InOrcCamp), 0.0, new List<AIChallenge.ChallengeTags>
        {
            AIChallenge.ChallengeTags.RequiresOwnSociety,
            AIChallenge.ChallengeTags.HealOrc,
            AIChallenge.ChallengeTags.Rest
        }, safeMove: false, supportSubtypes: true));
            list.Add(new AIChallenge(typeof(Rti_Orc_CeaseWar), 0.0, new List<AIChallenge.ChallengeTags>
        {
            AIChallenge.ChallengeTags.BaseValid,
            AIChallenge.ChallengeTags.BaseValidFor,
            AIChallenge.ChallengeTags.PreferLocal
        }, safeMove: false, supportSubtypes: true));
            List<AIChallenge> list2 = list;
            list2[0].delegates_Utility.Add(delegate_Utility_Ch_OrcRaiding);
            list2[1].delegates_Valid.Add(delegate_Valid_Ch_RecruitMinion);
            list2[2].delegates_Utility.Add(delegate_Utility_Ch_Rest_InOrcCamp);
            list2[3].delegates_ValidFor.Add(delegate_ValidFor_Rti_Orcs_CeaseWar);
            list2[3].delegates_Utility.Add(delegate_Utility_Rti_Orc_CeaseWar);
            ModCore.Get().GetAgentAI().RegisterAgentType(typeof(UAEN_OrcUpstart), new AgentAI.ControlParameters(isDark: true));
            ModCore.Get().GetAgentAI().AddChallengesToAgentType(typeof(UAEN_OrcUpstart), list2);
            if (ModCore.Get().GetAgentAI().TryGetAgentType(typeof(UAEN_OrcUpstart), out var aiData))
            {
                aiData?.aiChallenges_UniversalDelegates_ValidFor.Add(universalDelegate_ValidFor_Underground);
            }
        }

        private bool universalDelegate_ValidFor_Underground(AgentAI.ChallengeData challengeData, UA ua)
        {
            if (challengeData.challenge.canBeSeenAcrossZLevels())
            {
                return true;
            }

            if (challengeData.location.hex.z == 1 && ua.society is SG_Orc sG_Orc && !sG_Orc.canGoUnderground())
            {
                return false;
            }

            return true;
        }

        private double delegate_Utility_Ch_OrcRaiding(AgentAI.ChallengeData challengeData, UA ua, double utility, List<ReasonMsg> reasonMsgs)
        {
            int num = 0;
            foreach (Location neighbour in challengeData.location.getNeighbours())
            {
                if (neighbour.settlement is SettlementHuman settlementHuman && settlementHuman.ruler != null && settlementHuman.ruler.gold > num)
                {
                    num = settlementHuman.ruler.gold;
                }
            }

            if (num > 0)
            {
                double num2 = (double)num * map.param.ch_orcRaidingGoldGain;
                reasonMsgs?.Add(new ReasonMsg("Potential Gold Gain", num2));
                utility += num2;
            }

            return utility;
        }

        private bool delegate_Valid_Ch_RecruitMinion(AgentAI.ChallengeData challengeData)
        {
            if (map.worldPanic < map.param.panic_forFundHeroes)
            {
                return false;
            }

            return true;
        }

        private double delegate_Utility_Ch_Rest_InOrcCamp(AgentAI.ChallengeData challengeData, UA ua, double utility, List<ReasonMsg> reasonMsgs)
        {
            utility -= (double)map.param.ch_rest_parameterValue1;
            utility += 1.0;
            if (reasonMsgs != null)
            {
                ReasonMsg reasonMsg = reasonMsgs.FirstOrDefault((ReasonMsg m) => m.msg == "Base");
                if (reasonMsg != null)
                {
                    reasonMsg.value = 1.0;
                }
            }

            return utility;
        }

        private bool delegate_ValidFor_Rti_Orcs_CeaseWar(AgentAI.ChallengeData challengeData, UA ua)
        {
            if (challengeData.location.soc == null || challengeData.location.soc == ua.society)
            {
                return true;
            }

            return false;
        }

        private double delegate_Utility_Rti_Orc_CeaseWar(AgentAI.ChallengeData challengeData, UA ua, double utility, List<ReasonMsg> reasonMsgs)
        {
            if (challengeData.challenge is Rti_Orc_CeaseWar rti_Orc_CeaseWar)
            {
                SG_Orc orcs = rti_Orc_CeaseWar.caster.orcs;
                int num = 0;
                int num2 = 0;
                double num3 = 0.0;
                double num4 = 0.0;
                double currentMilitary = orcs.currentMilitary;
                foreach (DipRel allRelation in orcs.getAllRelations())
                {
                    SocialGroup socialGroup = allRelation.other(orcs);
                    if (!socialGroup.isGone() && allRelation.state == DipRel.dipState.war)
                    {
                        num3 += socialGroup.currentMilitary;
                        num++;
                        if (allRelation.war.att == orcs)
                        {
                            num4 += socialGroup.currentMilitary;
                            num2++;
                        }
                    }
                }

                double num5 = 0.0;
                if (num2 == 0 || num4 == 0.0)
                {
                    num5 = -100.0;
                    reasonMsgs?.Add(new ReasonMsg("Would not effect outcome", num5));
                    utility += num5;
                    return utility;
                }

                if (currentMilitary - 10.0 >= num3)
                {
                    num5 = num3 - currentMilitary;
                    reasonMsgs?.Add(new ReasonMsg("Superior military", num5));
                    utility += num5;
                }
                else if (currentMilitary - 10.0 < num3)
                {
                    if (num2 >= num || num4 >= num3)
                    {
                        num5 = 2.0 * (num3 - currentMilitary);
                        reasonMsgs?.Add(new ReasonMsg("Inferior military", num5));
                        utility += num5;
                    }
                    else
                    {
                        num5 = num3 - currentMilitary;
                        reasonMsgs?.Add(new ReasonMsg("Inferior military", num5));
                        utility += num5;
                        if (num4 > 0.0)
                        {
                            num5 = num4;
                            reasonMsgs?.Add(new ReasonMsg("Removes " + num4 + " combatants from war", num5));
                        }
                    }
                }
            }

            return utility;
        }*/



    }
}
