using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Code.Challenge;

namespace CommunitySeasonGod
{
    public class Rt_Season_BountyEnshadow : Ritual //I copied a ritual a while ago but have not converted it into what this ritual needs to do
    {
        public UA caster;

        public Rt_Season_BountyEnshadow(Location loc, UA parent)
            : base(loc)
        {
            caster = parent;
        }

        public override string getName()
        {
            return "Bountiful Shadows";
        }

        public override string getDesc()
        {
            return "Spends Harvest's Bounty to enshadow any location.";
        }

        public override string getCastFlavour()
        {
            return "No peoples will reject the gifts of the harvest, especially those so blessed by The Uncle.";
        }

        public override string getRestriction()
        {
            if (caster.getGeomancyMastery() < 3)
            {
                return "Requires " + getReq(caster.getGeomancyMastery()) + " Arcane Knowledge";
            }

            return "Maximum level reached";
        }

        public override double getProfile()
        {
            return 0.0;
        }

        public override double getMenace()
        {
            return 25.0;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.LORE;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Lore", Math.Max(1, unit.getStatLore())));
            return Math.Max(1, unit.getStatLore());
        }

        public override double getComplexity()
        {
            return 40.0;
        }

        public override int getCompletionMenace()
        {
            return 0;
        }

        public override int getCompletionProfile()
        {
            return 0;
        }

        public override bool validFor(UA ua)
        {
            foreach (Trait trait in ua.person.traits)
            {
                if (trait is T_MysticBounty bounty)
                {
                    if (bounty.amount > 75.0)
                        return true;
                }
            }
            return false;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.geomanticLocus;
        }

        public override int isGoodTernary()
        {
            return 0;
        }

        public int getReq(int level)
        {
            int[] array = new int[4] { 2, 3, 5, 100 };
            return array[level];
        }

        public override void complete(UA ua)
        {
            T_ArcaneKnowledge t_ArcaneKnowledge = null;
            foreach (Trait trait in ua.person.traits)
            {
                if (trait is T_ArcaneKnowledge t_ArcaneKnowledge2)
                {
                    t_ArcaneKnowledge = t_ArcaneKnowledge2;
                }
            }

            if (t_ArcaneKnowledge == null || t_ArcaneKnowledge.level == 0)
            {
                return;
            }

            T_MasteryGeomancy t_MasteryGeomancy = null;
            int level = 0;
            foreach (Trait trait2 in ua.person.traits)
            {
                if (trait2 is T_MasteryGeomancy t_MasteryGeomancy2)
                {
                    if (t_MasteryGeomancy2.level >= t_MasteryGeomancy2.getMaxLevel())
                    {
                        return;
                    }

                    t_MasteryGeomancy = t_MasteryGeomancy2;
                    level = t_MasteryGeomancy2.level;
                }
            }

            if (t_MasteryGeomancy == null)
            {
                t_MasteryGeomancy = new T_MasteryGeomancy();
                t_MasteryGeomancy.level = 0;
                ua.person.receiveTrait(t_MasteryGeomancy);
            }

            int req = getReq(level);
            t_ArcaneKnowledge.level -= req;
            t_MasteryGeomancy.level++;
            if (t_ArcaneKnowledge.level < 0)
            {
                t_ArcaneKnowledge.level = 0;
            }

            if (map.opt_allowMagicalArmsRace && ua.isCommandable() && map.overmind.magicalArmsRace < (double)t_MasteryGeomancy.level)
            {
                map.overmind.magicalArmsRace = t_MasteryGeomancy.level;
            }

            map.addUnifiedMessage(ua, null, "Geomancy Mastery", ua.getName() + " has advanced their mastery of Geomancy to level " + t_MasteryGeomancy.level, UnifiedMessage.messageType.MAGICAL_MASTERY);
        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            double utility = base.getUtility(ua, msgs);
            if (ua.isCommandable())
            {
                return utility;
            }

            if (ua.getGeomancyMastery() == 3)
            {
                return 0.0;
            }

            double num = map.param.ch_studyMagicAversion;
            utility += num;
            msgs?.Add(new ReasonMsg("Magic is dangerous", num));
            if (ua is UAG_Mage)
            {
                double num2 = map.overmind.magicalArmsRace - (double)ua.getGeomancyMastery() * 0.75;
                if (num2 > 0.0)
                {
                    num = num2 * map.overmind.magicalArmsRace * (double)map.param.ch_studyMagicFromArmsRace;
                    utility += num;
                    msgs?.Add(new ReasonMsg("Magical Arms Race", num));
                }
            }

            return utility;
        }

        public override bool valid()
        {
            return true;
        }

        public override int[] buildPositiveTags()
        {
            return new int[0];
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }
    }
}
