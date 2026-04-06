using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_MeldIntoNature : Ritual
    {

        public static double reductionPerTurn = 1;

        public Rt_Season_MeldIntoNature(Location loc) : base(loc) { }

        public override double getMenace()
        {
            return 0.0;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.OTHER;
        }

        public override string getName()
        {
            return "Meld Into Nature";
        }

        public override string getDesc()
        {
            return "Reduces an agent's <b>menace</b> and <b>profile</b> by " + reductionPerTurn + " per turn until minimum is reached (hover over stat in left-hand panel to see an agent's minimum) and increases their <b>hp</b> by 1 per turn. No effect on first turn.";
        }

        public override string getCastFlavour()
        {
            return "Over time memories fade, and other issues rise to the forefront. Hiding amongst the shadows or amongst other cultists allows the trail to go cold, and the pursuing forces to be lost.";
        }

        public override string getRestriction()
        {
            return "Unit must have menace or profile above their minimum, or must be below full <b>hp</b>";
        }

        public override bool ignoreInterruptionWarning()
        {
            return true;
        }

        public override double getProfile()
        {
            return 0;
        }

        public override double getUtility(UA ua, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(ua, reasons);
            double num = ua.inner_profile - ua.inner_profileMin;
            double num2 = ua.inner_menace - ua.inner_menaceMin;
            utility += num;
            utility += num2;
            if (reasons != null)
            {
                reasons.Add(new ReasonMsg("Potential Profile Reduction", num));
                reasons.Add(new ReasonMsg("Potential Menace Reduction", num2));
            }

            return utility;
        }

        public override int getSimplificationLevel()
        {
            return 0;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            return 1;
        }

        public override double getComplexity()
        {
            return map.param.ch_laylow_complexity;
        }

        public override bool isIndefinite()
        {
            return true;
        }

        public override Sprite getSprite()
        {
            return EventManager.getImg("ComSeasonGod.challenge_hide_in_nature.png");
        }

        public override int isGoodTernary()
        {
            return 0;
        }

        public override bool validFor(UA ua)
        {
            return ua.inner_menace > ua.inner_menaceMin || ua.inner_profile > ua.inner_profileMin;
        }

        public override void complete(UA u)
        {
        }

        public override void turnTick(UA u)
        {
            base.turnTick(u);
            if (!(u.task is Task_PerformChallenge task_PerformChallenge) || task_PerformChallenge.turnsTaken < 1)
            {
                return;
            }

            u.hp = Math.Min(u.hp + 1, u.maxHp);
            bool flag = u.inner_profile <= u.inner_profileMin;
            bool flag2 = u.inner_menace <= u.inner_menaceMin;
            u.addProfile(-1.0 * getProgressPerTurn(u, null));
            u.addMenace(-1.0 * getProgressPerTurn(u, null));
            bool flag3 = u.inner_profile <= u.inner_profileMin;
            bool flag4 = u.inner_menace <= u.inner_menaceMin;
            bool flag5 = u.hp >= u.maxHp;

            if (flag5)
            {
                if (flag4 && flag3)
                {
                    u.task = null;
                    complete(u);
                    if (u.isCommandable())
                    {
                        map.addMessage(u.getName() + " completes: " + getName(), map.param.ch_laylow_parameterValue5, positive: true, u.location.hex);
                        popCompletionMessage(u);
                    }
                }
                else if (flag4 && !flag2)
                {
                    if (u.isCommandable())
                    {
                        map.addUnifiedMessage(u, null, "Laying Low", u.getName() + " is laying low and has reached their minimum menace value, but not their minimum profile", UnifiedMessage.messageType.LAY_LOW_PARTIALLY_COMPLETE);
                    }
                }
                else if (flag3 && !flag && u.isCommandable())
                {
                    map.addUnifiedMessage(u, null, "Laying Low", u.getName() + " is laying low and has reached their minimum profile value, but not their minimum menace", UnifiedMessage.messageType.LAY_LOW_PARTIALLY_COMPLETE);
                }
            }
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
            return new int[1] { Tags.DANGER };
        }

    }
}
