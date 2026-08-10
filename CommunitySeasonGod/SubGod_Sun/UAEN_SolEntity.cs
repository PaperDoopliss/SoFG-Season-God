using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;
using CommunityLib;

namespace CommunitySeasonGod
{
    class UAEN_SolEntity : UAEN
    {
        public List<Location> completedLocations = new List<Location>();
        public static float temperatureToAdd = 0.005f;

        public UAEN_SolEntity(Location loc, Society sg, Map map)
            : base(loc, sg)
        {
            base.person.stat_might = 5;
            base.person.stat_command = 1;
            base.person.stat_intrigue = 1;
            base.person.stat_lore = 4;

            maxHp = 7;
            hp = 7;

            base.person.species = map.species_monster;

            bool hasSolEntityTrait = false;
            foreach (Trait t in person.traits)
            {
                if (t is T_SolEntity_SunCloak)
                    hasSolEntityTrait = true;
            }
            if (!hasSolEntityTrait)
            {
                T_SolEntity_SunCloak cloak = new T_SolEntity_SunCloak();
                cloak.isInherent = true;
                person.receiveTrait(cloak);

            }

            rituals.Add(new Rt_Season_SunGodWorship(loc));

        }

        public override string getName()
        {
            if (base.person.overrideName != null && base.person.overrideName.Length != 0)
            {
                return base.person.overrideName;
            }

            return "Solar Entity";
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
            return EventManager.getImg("ComSeasonGod.unit_solar_entity.png");
        }

        public override void turnTickAI()
        {
            Kernel_Season.ComLibKernel.GetAgentAI().turnTickAI(this);
        }

        public override void turnTick(Map map)
        {
            base.turnTick(map);

            addMenace(0.25);
            foreach (Hex hex in location.territory)
            {
                hex.transientTempDelta += temperatureToAdd;
            }
        }


        public static void populateSolEntity()
        {

            List<AIChallenge> list = new List<AIChallenge>();
            list.Add(new AIChallenge(typeof(Rt_Season_SunGodWorship), 0.0, new List<AIChallenge.ChallengeTags>
            {
            }, safeMove: false, supportSubtypes: true));


            AgentAI.ControlParameters solEntityParams = new AgentAI.ControlParameters(true);
            solEntityParams.considerAllRituals = true;

            List<AIChallenge> list2 = list;
            list2[0].delegates_Utility.Add(delegate_Utility_Rt_Season_SunGodWorship);
            list2[0].delegates_Valid.Add(delegate_Valid_Rt_Season_SunGodWorship);
            list2[0].delegates_ValidFor.Add(delegate_ValidFor_Rt_Season_SunGodWorship);
            ModCore.Get().GetAgentAI().RegisterAgentType(typeof(UAEN_SolEntity), solEntityParams);
            ModCore.Get().GetAgentAI().AddChallengesToAgentType(typeof(UAEN_SolEntity), list2);

        }

        private static double delegate_Utility_Rt_Season_SunGodWorship(AgentAI.ChallengeData challengeData, UA ua, double utility, List<ReasonMsg> reasonMsgs)
        {
            utility = 100;
            reasonMsgs?.Add(new ReasonMsg("Base", 100));

            int distance = ua.map.getStepDist(ua.location, challengeData.location);
            if (distance > 0)
            {
                utility -= distance * 5;
                reasonMsgs?.Add(new ReasonMsg("Distance", distance * -5));
            }

            if (ua is UAEN_SolEntity entity)
            {
                if (entity.completedLocations.Contains(challengeData.location))
                {
                    utility -= 80;
                    reasonMsgs?.Add(new ReasonMsg("Already Prayed Here", -80));
                }
            }

            return utility;

        }

        private static bool delegate_Valid_Rt_Season_SunGodWorship(AgentAI.ChallengeData challengeData)
        {
            if (challengeData.location.settlement is Set_ElvenCity)
            {
                return true;
            }

            if (challengeData.location.settlement != null)
            {
                foreach (Subsettlement sub in challengeData.location.settlement.subs)
                {
                    if (sub is Sub_Farms)
                        return true;
                }
            }

            if (challengeData.location.hex.isForest)
                return true;
            

            return false;
        }

        private static bool delegate_ValidFor_Rt_Season_SunGodWorship(AgentAI.ChallengeData challengeData, UA ua)
        {
            return true;
        }


    }
}
