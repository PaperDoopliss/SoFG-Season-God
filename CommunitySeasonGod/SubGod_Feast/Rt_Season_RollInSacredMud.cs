using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_RollInSacredMud : Ritual
    {
        public Rt_Season_RollInSacredMud(Location loc) : base(loc) { }

        public override string getName()
        {
            return "Roll in Sacred Mud";
        }

        public override string getDesc()
        {
            return "Destroys all Fey Presence at this location to create a Golem Army, behaving like an orc army with <b>hp</b> equal to the Fey Presence spent.";
        }

        public override string getCastFlavour()
        {
            return "The creature's misbegotten power draws him to sculpt the otherworldly presence here into a facsimile of life, animated only by a mindless bond with the tribe that spawned it.";
        }

        public override string getRestriction()
        {
            return "Can only be performed in a location with Fey Presence";
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
            return EventManager.getImg("ComSeasonGod.unit_golem_army.png");
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.LORE;
        }

        public override bool validFor(UA ua)
        {

            foreach (Property pr in ua.location.properties)
            {
                if (pr is Pr_FeyPresence)
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
            return 5;
        }

        public override int getCompletionProfile()
        {
            return 5;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Lore", unit.getStatLore()));
            return Math.Max(1, unit.getStatLore());
        }

        public override double getComplexity()
        {
            return 30;
        }

        public override void complete(UA u)
        {
            Location location = u.location;
            double num = 0.0;

            for (int i = 0; i < u.location.properties.Count; i++)
            {
                if (u.location.properties[i] is Pr_FeyPresence)
                {
                    num += u.location.properties[i].charge;
                    u.location.properties.RemoveAt(i);
                    i--;
                }
            }

            UM_Season_GolemArmy army = new UM_Season_GolemArmy(location, u.society, (int)num);
            u.location.units.Add(army);
            map.units.Add(army);
        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Performed automatically if there is Fey Presence at a location",-1000));
            return -1000;
        }

        public override int[] buildPositiveTags()
        {
            return new int[3]
            {
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
