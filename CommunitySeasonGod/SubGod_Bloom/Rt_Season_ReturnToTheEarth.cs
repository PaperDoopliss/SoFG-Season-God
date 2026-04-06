using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_ReturnToTheEarth : Ritual
    {
        public static double presenceToAdd = 50;

        public Rt_Season_ReturnToTheEarth(Location location) : base(location)
        {
        }

        public override string getName()
        {
            return "Return to the Earth";
        }

        public override string getDesc()
        {
            return "Kills this person and grants their location " + presenceToAdd + "% Fey Presence";
        }

        public override string getRestriction()
        {
            return "";
        }

        public override double getComplexity()
        {
            return 1;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Base", 1));
            return 1;
        }

        public override bool valid()
        {
            return true;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override Sprite getSprite()
        {
            return EventManager.getImg("ComSeasonGod.challenge_return_to_earth.png");
        }

        public override string getCastFlavour()
        {
            return "All things change, and changing to and from a conscious being is part of that. They will live on in some form forever, but it will not be them in any true sense.";
        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            double utility = base.getUtility(ua, msgs);

            msgs?.Add(new ReasonMsg("Impulse to Dissolve",500));
            utility += 500;

            return utility;
        }

        public override void complete(UA u)
        {
            base.complete(u);

            u.die(map, u.getName() + " returned to the earth");

            foreach (Property pr in u.location.properties)
            {
                if (pr is Pr_FeyPresence presence)
                {
                    presence.charge += presenceToAdd;
                    if (presence.charge > 300)
                        presence.charge = 300;
                    return;
                }
            }

            Pr_FeyPresence newPresence = new Pr_FeyPresence(u.location);
            newPresence.charge = presenceToAdd;
            u.location.properties.Add(newPresence);
        }
    }
}
