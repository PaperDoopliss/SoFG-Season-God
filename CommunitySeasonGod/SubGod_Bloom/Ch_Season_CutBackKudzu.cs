using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Ch_Season_CutBackKudzu : Challenge
    {

        public Pr_Season_DreamingKudzu kudzu;
        public double kudzuToReduce = 50;

        public Ch_Season_CutBackKudzu(Location location, Pr_Season_DreamingKudzu kudzu) : base(location) 
        { 
            this.kudzu = kudzu;
        }

        public override string getName()
        {
            return "Cut Back Kudzu";
        }

        public override double getProfile()
        {
            return kudzu.charge / 20;
        }

        public override double getMenace()
        {
            return kudzu.charge;
        }

        public override int getCompletionProfile()
        {
            return 3;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.MIGHT;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Might", Math.Max(1, unit.getStatMight())));
            return Math.Max(1, unit.getStatMight());
        }

        public override double getComplexity()
        {
            return 25;
        }

        public override bool validFor(UA ua)
        {
            if (ua.isCommandable())
                return true;

            if (ua.person != null)
            {
                if (ua.person.awareness >= 1)
                    return true;
                if (ua.person.getTagRanking(Tags.MADNESS) <= -1)
                    return true;
            }

            return false;
        }

        public override Sprite getSprite()
        {
            return EventManager.getImg("ComSeasonGod.property_dreaming_kudzu.png");
        }

        public override int isGoodTernary()
        {
            return 0;
        }

        public override string getCastFlavour()
        {
            return "The kudzu is not difficult to cut, tear and burn, but there is so much of it. It is a battle of endurance against a life form that does not sleep and can spread anywhere.";
        }

        public override string getDesc()
        {
            return "Aware heroes and those who dislike Madness can reduce the level of Dreaming Kudzu at this location by " + kudzuToReduce + "%.";
        }

        public override void complete(UA u)
        {
            if (kudzu.charge <= kudzuToReduce)
            {
                kudzu.location.properties.Remove(kudzu);
                kudzu.location.properties.Add(new Pr_Season_PurgedKudzu(location));
            }
            else
            {
                kudzu.influences.Add(new ReasonMsg("Razing Army", -kudzuToReduce));
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
            return new int[2]
            {
            Tags.MADNESS,
            map.soc_dark.index + 20000
            };
        }



    }
}
