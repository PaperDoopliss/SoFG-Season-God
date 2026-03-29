using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_BeckonKnights : Ritual
    {

        public UM_Season_FeyKnights knights;

        public Rt_Season_BeckonKnights(Location location, UM_Season_FeyKnights knights) : base(location)
        {
            this.knights = knights;
        }

        public override string getName()
        {
            return "Beckon Knights";
        }

        public override string getDesc()
        {
            return "Changes the Fey Knights' target to the present location. They will steadily march toward it, burning down any non-Dark Empire settlements in the way.";
        }

        public override string getRestriction()
        {
            return "Must be performed at a location with at least " + UM_Season_FeyKnights.presenceTargetCutoff + "% Fey Presence, and the Fey Knights must be on the map";
        }

        public override double getComplexity()
        {
            return 15;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.COMMAND;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Command", Math.Max(1,unit.getStatCommand())));
            return Math.Max(1, unit.getStatCommand());
        }


        public override bool validFor(UA ua)
        {

            foreach (Property pr in ua.location.properties)
            {
                if (pr is Pr_FeyPresence)
                {
                    if (pr.charge >= UM_Season_FeyKnights.presenceTargetCutoff)
                    {
                        if (knights?.location.units.Contains(knights) == true)
                            return true;
                    }


                    return false;
                }
            }

            return false;
        }

        public override Sprite getSprite()
        {
            return EventManager.getImg("ComSeasonGod.unit_pale_army.png");
        }

        public override string getCastFlavour()
        {
            return "During the time of feasts, even the Pale Knight must offer a tempting enough morsel to direct the fey elsewhere.";
        }

        public override void complete(UA u)
        {
            base.complete(u);

            if (knights != null)
                knights.target = u.location;
        }


    }
}
