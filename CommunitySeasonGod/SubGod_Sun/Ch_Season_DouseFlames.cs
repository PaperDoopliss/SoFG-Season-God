using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Ch_Season_DouseFlames : Challenge
    {
        public Pr_Season_Wildfire wildfire = null;
        public static double dousePerTurn = -25;

        public Ch_Season_DouseFlames(Location loc, Pr_Season_Wildfire wildfire)
            : base(loc)
        {
            this.wildfire = wildfire;
        }

        public override string getName()
        {
            return "Douse Flames";
        }

        public override string getDesc()
        {
            return "Reduces the Wildfire at this location by " + -dousePerTurn + "% every turn";
        }

        public string getFlavour()
        {
            return "The flames spread and the masses defy.";
        }

        public override double getProfile()
        {
            if (wildfire.charge >= 200)
                return 70;
            else if (wildfire.charge >= 100)
                return 30;
            return 0;
        }

        public override double getMenace()
        {
            return 100;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.OTHER;
        }
        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Base", 1));
            return 1;
        }

        public override double getComplexity()
        {
            return 15;
        }

        public override int getCompletionMenace()
        {
            return 0;
        }

        public override int getCompletionProfile()
        {
            return 5;
        }

        public override int getInherentDanger()
        {
            return 2;
        }

        public override int isGoodTernary()
        {
            return 1;
        }

        public override Sprite getSprite()
        {
            return EventManager.getImg("ComSeasonGod.power_wildfire.png");
        }

        public override int[] buildPositiveTags()
        {
            return new int[2]
            {
            Tags.COOPERATION,
            Tags.DANGER
            };
        }
        public override int[] buildNegativeTags()
        {
            return new int[0];
        }

        public override bool allowMultipleUsers()
        {
            return false;
        }

        public override void turnTick(UA ua)
        {
            base.turnTick(ua);

            wildfire.influences.Add(new ReasonMsg(ua.getName() + "'s Firefighting", dousePerTurn));
        }

    }
}
