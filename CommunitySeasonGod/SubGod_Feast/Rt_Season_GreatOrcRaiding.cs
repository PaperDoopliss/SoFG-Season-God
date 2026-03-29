using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_GreatOrcRaiding : Ritual
    {

        public static double devastationToAdd = 100;

        public Rt_Season_GreatOrcRaiding(Location loc)
            : base(loc)
        {
        }

        public override string getName()
        {
            return "Great Orc Raiding";
        }

        public override string getDesc()
        {
            return "Raids a nearby human settlement, taking " + (int)(100.0 * map.param.ch_orcRaidingGoldGain) + "% of the ruler's gold and inflicting " + devastationToAdd + "% Devastation. Adds " + map.param.ch_orcishRaidingMenaceGain + " menace to the orcish horde.";
        }

        public override string getCastFlavour()
        {
            return "Unruly and disorganised, the orcish raiders are motivated by bloodlust, greed, remembered grudges and the need for personal glory. As a military force their poor discipline is offset by their extreme personal strength and reckless bravery.";
        }

        public override string getRestriction()
        {
            return "";
        }

        public override double getProfile()
        {
            return map.param.ch_orcs_raiding_aiProfile;
        }

        public override double getMenace()
        {
            return map.param.ch_orcs_raiding_aiMenace;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.raid;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.COMMAND;
        }

        public override bool validFor(UA ua)
        {
            if (ua.location.soc is Society && ua.location.settlement is SettlementHuman settlementHuman && settlementHuman.ruler != null)
            {
                return true;
            }

            return false;
        }

        public override bool valid()
        {
            return true;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override int getCompletionMenace()
        {
            return map.param.ch_orcs_raiding_parameterValue2;
        }

        public override int getCompletionProfile()
        {
            return map.param.ch_orcs_raiding_parameterValue3;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            int num = 0;
            num += unit.getStatMight();
            msgs?.Add(new ReasonMsg("Stat: Might", unit.getStatMight()));
            num += unit.getStatCommand();
            msgs?.Add(new ReasonMsg("Stat: Command", unit.getStatCommand()));
            if (num < 1)
            {
                num++;
                msgs?.Add(new ReasonMsg("Base", 1.0));
            }

            return num;
        }

        public override double getComplexity()
        {
            return map.param.ch_orcs_raiding_complexity;
        }

        public override void complete(UA u)
        {
            Location location = u.location;
            double num = 0.0;
            if (u.location.settlement is SettlementHuman sh && sh.ruler != null)
                num = sh.ruler.gold;

            if (location != null)
            {
                int num2 = (int)((double)location.person().gold * map.param.ch_orcRaidingGoldGain);
                base.msgString = "The orcs raid the " + location.getName() + ", taking " + num2 + " gold, which has been added directly to " + u.getName() + "'s own reserves. The orcs have gained " + map.param.ch_orcishRaidingMenaceGain + " menace as a society as a result of this action.";
                u.person.addGold(num2);
                location.person().addGold(-num2);

                Property.addToPropertySingleShot("Great Orc Raiding", Property.standardProperties.DEVASTATION, devastationToAdd, location);

                if (u.society is SG_Orc sG_Orc)
                {
                    sG_Orc.menace += map.param.ch_orcishRaidingMenaceGain;
                }
            }
        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            double utility = base.getUtility(ua, msgs);
            return utility + (double)map.param.ch_orcs_raiding_parameterValue5;
        }

        public override int[] buildPositiveTags()
        {
            return new int[4]
            {
            Tags.GOLD,
            Tags.COMBAT,
            Tags.DANGER,
            Tags.CRUEL
            };
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }



    }
}
