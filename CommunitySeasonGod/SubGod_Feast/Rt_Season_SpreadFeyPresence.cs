using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_SpreadFeyPresence : Ritual
    {
        public static double presenceToAdd = 50;


        public Rt_Season_SpreadFeyPresence(Location loc)
            : base(loc)
        {
        }

        public override string getName()
        {
            return "Spread Fey Presence";
        }

        public override string getDesc()
        {
            return "Increases Fey Presence in this location by " + presenceToAdd + "%, to a maximum of 300%";
        }

        public override string getRestriction()
        {
            return "Can only be performed by an fey-blooded or 50%-enshadowed character with Servant's Spectacles";
        }

        public override string getCastFlavour()
        {
            return "The world is more vulnerable from the inside than the outside. Someone who understands the occult energies suffusing the world with the proper equipment can exploit vulnerabilities that would occupy a god for weeks.";
        }

        public override double getProfile()
        {
            return 0;
        }

        public override double getMenace()
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
            if (ua.person != null)
            {
                foreach (Item item in ua.person.items)
                {
                    if (item is I_Season_ServantsSpectacles)
                    {
                        if (ua.person.shadow >= 0.5)
                            return true;
                        foreach (Trait t in ua.person.traits)
                        {
                            if (t is T_Season_Feyblood)
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

                    result += 50 - Math.Abs(pr.charge - 50);
                    msgs?.Add(new ReasonMsg("Existing Fey Presence", 50 - Math.Abs(pr.charge - 50)));
                    
                }
            }

            return result;
        }

        public override bool valid()
        {
            return true;
        }

        public override int isGoodTernary()
        {
            return 0;
        }

        public override int getCompletionMenace()
        {
            return 15;
        }

        public override int getCompletionProfile()
        {
            return 3;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Lore", Math.Max(1, unit.getStatLore())));
            return Math.Max(1, unit.getStatLore());
        }

        public override double getComplexity()
        {
            return 50;
        }

        public override void complete(UA u)
        {
            foreach (Property pr in u.location.properties)
            {
                if (pr is Pr_FeyPresence presence)
                {
                    presence.charge = Math.Min(presence.charge + presenceToAdd, 300);
                    return;
                }
            }

            Pr_FeyPresence newPresence = new Pr_FeyPresence(u.location);
            newPresence.charge = presenceToAdd;
            u.location.properties.Add(newPresence);
        }

        public override int[] buildPositiveTags()
        {
            return new int[1] {Tags.SHADOW};
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }


    }
}
