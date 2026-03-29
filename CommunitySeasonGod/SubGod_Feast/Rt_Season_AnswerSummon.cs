using Assets.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_AnswerSummon : Ritual
    {

        public int turnTouched;

        public Rt_Season_AnswerSummon(Location location) : base(location)
        {
            turnTouched = map.turn;
        }

        public override string getName()
        {
            return "Answer Summon";
        }

        public override string getDesc()
        {
            return "Gains Feyblood, increasing their stats and making them more indulgent and aggressive.";
        }

        public override string getRestriction()
        {
            return "";
        }

        public override double getComplexity()
        {
            return 3;
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
            return EventManager.getImg("ComSeasonGod.power_pale_knights_summon.png");
        }

        public override string getCastFlavour()
        {
            return "The sounds of wildlife fade away as they approach the meeting point, as if those in tune with the natural world knew enough to stay away. There is barely enough time to glimpse a pale figure before they lose consciousness.";
        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            double utility = base.getUtility(ua, msgs);

            msgs?.Add(new ReasonMsg("Growing Call", (map.turn - turnTouched) * 5));
            utility += (map.turn - turnTouched) * 5;

            return utility;
        }

        public override void complete(UA u)
        {
            base.complete(u);

            u.rituals.Remove(this);

            if (u.person != null)
            {
                for (int i = 0; i < u.person.traits.Count; i++)
                {
                    if (u.person.traits[i] is T_Season_PaleKnightsTouch)
                        u.person.traits.RemoveAt(i);
                }
                u.person.receiveTrait(new T_Season_Feyblood(u.person));

            }
        }

    }
}
