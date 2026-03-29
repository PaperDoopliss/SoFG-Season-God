using Assets.Code;
using CommunitySeasonGod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_PaleKnightsTouch : Trait
    {
        public Act_Season_AnswerSummon act_answer;
        public Rt_Season_AnswerSummon rt_answer;

        public T_Season_PaleKnightsTouch(Person p) : base()
        {
            act_answer = new Act_Season_AnswerSummon(p.getLocation());
            rt_answer = new Rt_Season_AnswerSummon(p.getLocation());
        }

        public override string getName()
        {
            return "Pale Knight's Touch";
        }

        public override string getDesc()
        {
            return "This person has drawn the Pale Knight's attention, and will feel a growing compulsion to approach them and be infused with Feyblood.";
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);

            if (person.unit != null)
            {
                person.unit.rituals.Add(rt_answer);
            }
        }

        public override List<Assets.Code.Action> getActions()
        {
            List<Assets.Code.Action> result = base.getActions();
            result.Add(act_answer);
            return result;
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            if (p.unit != null)
            {
                if (p.unit.rituals.Contains(rt_answer) == false)
                    p.unit.rituals.Add(rt_answer);
            }

            bool alreadyHasFeyblood = false;
            foreach (Trait t in p.traits)
            {
                if (t is T_Season_Feyblood)
                {
                    alreadyHasFeyblood = true;
                    break;
                }
            }
            if (alreadyHasFeyblood)
                p.traits.Remove(this);


        }

    }
}
