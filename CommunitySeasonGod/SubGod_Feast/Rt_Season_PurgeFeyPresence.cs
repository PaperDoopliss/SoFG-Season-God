using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_PurgeFeyPresence : Ritual
    {
        public static double presenceToRemove = 50;


        public Rt_Season_PurgeFeyPresence(Location loc)
            : base(loc)
        {

        }

        public override string getName()
        {
            return "Purge Fey Presence";
        }

        public override string getDesc()
        {
            return "Reduces Fey Presence in this location by " + presenceToRemove + "%";
        }

        public override string getRestriction()
        {
            return "Can only be performed by a member of the Alliance with the Servant's Spectacles and less than 50% <b>shadow</b>";
        }

        public override string getCastFlavour()
        {
            return "The spectacles were guarded jealously to prevent exactly this happening.";
        }

        public override double getProfile()
        {
            return 0;
        }

        public override Sprite getSprite()
        {
            return EventManager.getImg("ComSeasonGod.Icon_FeyPresence_Background.png");
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.LORE;
        }

        public override bool validFor(UA ua)
        {
            if (ua.person != null && ua.person.society?.isAlliance == true)
            {
                foreach (Item item in ua.person.items)
                {
                    if (item is I_Season_ServantsSpectacles)
                    {

                        foreach (Property pr in ua.location.properties)
                        {
                            if (pr is Pr_FeyPresence)
                                return true;
                        }
                        return false;
                    }

                }
            }
            return true;
        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            double result = base.getUtility(ua, msgs);

            foreach (Property pr in ua.location.properties)
            {
                if (pr is Pr_FeyPresence)
                {

                    result += pr.charge * 1.5;
                    msgs?.Add(new ReasonMsg("Fey Presence", pr.charge * 1.5));

                }
            }

            return result;
        }

        public override int isGoodTernary()
        {
            return 1;
        }

        public override int getCompletionMenace()
        {
            return 0;
        }

        public override int getCompletionProfile()
        {
            return 5;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Lore", Math.Max(1, unit.getStatLore())));
            return Math.Max(1, unit.getStatLore());
        }

        public override double getComplexity()
        {
            return 30;
        }

        public override void complete(UA u)
        {

            Pr_FeyPresence presence = null;
            foreach (Property pr in u.location.properties)
            {
                presence = pr as Pr_FeyPresence;
                if (presence != null)
                {
                    presence.charge -= presenceToRemove;
                    break;
                }
            }

            if (presence != null && presence.charge <= 0)
            {
                u.location.properties.Remove(presence);
            }
        }

        public override int[] buildPositiveTags()
        {
            return new int[0];
        }

        public override int[] buildNegativeTags()
        {
            return new int[1] {Tags.SHADOW};
        }


    }
}
