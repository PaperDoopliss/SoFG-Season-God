using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Ch_Season_SeekArtifactGood : Challenge
    {
        public Pr_Season_HiddenArtifact property;

        public Ch_Season_SeekArtifactGood(Location loc, Pr_Season_HiddenArtifact property) : base(loc) { this.property = property; }

        public override string getName()
        {
            return "Seek " + property.getName();
        }

        public override string getDesc()
        {
            return "Grants the first person to complete this challenge a " + property.getName();
        }

        public override string getCastFlavour()
        {
            return "Sleepless nights and forced marches have brought them to a concealed altar, mercilessly cracked open before any competition can catch up.";
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.OTHER;
        }

        public override double getProfile()
        {
            return 50;
        }

        public override double getMenace()
        {
            return 0;
        }

        public override Sprite getSprite()
        {
            return property.item.getIconFore();
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            double result = base.getUtility(ua, msgs);

            if (property.item is I_Season_CourtDuelingBlade)
            {
                if (ua.person != null)
                {
                    result += (30 * ua.person.getStatMight());
                    msgs?.Add(new ReasonMsg("Wants More Might", 30 * ua.person.getStatMight()));
                }
            }
            else if (property.item is I_Season_ServantsSpectacles)
            {
                if (ua.person != null)
                {
                    result += (30 * ua.person.getStatLore());
                    msgs?.Add(new ReasonMsg("Wants More Lore", 30 * ua.person.getStatLore()));
                }
            }
            else
            {
                result += 90;
                msgs?.Add(new ReasonMsg("Valuable Artifact", 90));
            }
            return result;
        }

        public override bool valid()
        {
            return true;
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
            double value = unit.getStatIntrigue() + unit.getStatMight() + unit.getStatLore() + unit.getStatCommand();

            if (msgs != null)
            {
                msgs.Add(new ReasonMsg("Stat: Intrigue", Math.Max(1, unit.getStatIntrigue())));
                msgs.Add(new ReasonMsg("Stat: Lore", Math.Max(1, unit.getStatLore())));
                msgs.Add(new ReasonMsg("Stat: Command", Math.Max(1, unit.getStatCommand())));
                msgs.Add(new ReasonMsg("Stat: Might", Math.Max(1, unit.getStatMight())));
            }

            return value;
        }


        public override double getComplexity()
        {
            return 100;
        }

        public override void complete(UA u)
        {

            if (u.person != null)
            {
                if (u.isCommandable() == false)
                    u.person.gainItem(property.item, true);
                else
                    u.person.gainItem(property.item);
            }

            location.properties.Remove(property);
        }

        public override int[] buildPositiveTags()
        {
            return new int[0] ;
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }



    }
}
