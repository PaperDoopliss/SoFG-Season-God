using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_BestowFeyblood : Ritual
    {

        public Rt_Season_BestowFeyblood(Location location) : base(location)
        {
        }

        public override string getName()
        {
            return "Bestow Feyblood";
        }

        public override string getDesc()
        {
            return "Grants Feyblood to any other heroes, acolytes or agents in the location, increasing their stats and aggression.";
        }

        public override string getRestriction()
        {
            return "";
        }

        public override double getComplexity()
        {
            return 12;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.INTRIGUE;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Intrigue", Math.Max(1, unit.getStatIntrigue())));
            return Math.Max(1, unit.getStatIntrigue());
        }


        public override bool validFor(UA ua)
        {
            return true;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.shadowMarket;
        }

        public override string getCastFlavour()
        {
            return "With the Pale Knight at the height of his power, there is less need to tempt and tease to inflict his cravings on the world.";
        }

        public override void complete(UA u)
        {
            base.complete(u);

            foreach (Unit target in u.location.units)
            {
                if (target is UA ua && ua.person != null)
                {
                    bool alreadyHasFeyblood = false;

                    foreach (Trait t in ua.person.traits)
                    {
                        if (t is T_Season_Feyblood)
                        {
                            alreadyHasFeyblood = true;
                            break;
                        }
                    }

                    if (!alreadyHasFeyblood)
                    {
                        ua.person.receiveTrait(new T_Season_Feyblood(ua.person, u.person));
                    }
                }
            }
        }


    }
}
