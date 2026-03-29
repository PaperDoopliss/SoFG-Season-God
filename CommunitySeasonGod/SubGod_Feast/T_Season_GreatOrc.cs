using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_GreatOrc : Trait
    {
        public static double amountRequired = 50;
        public Rt_Season_RollInSacredMud rt_roll = null;

        public override string getName()
        {
            return "Feastbound";
        }

        public override string getDesc()
        {
            return "If this person is in a location with at least " + amountRequired + "% Fey Presence, they will abort their previous task and perform the Roll in Sacred Mud challenge, which will turn that location's Fey Presence into a Golem Army with that much <b>hp</b>.";
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);

            checkRoll(person);
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);
            checkRoll(p);
        }

        public void checkRoll(Person p)
        {
            if (p.unit != null)
            {
                if (rt_roll == null || p.unit.rituals.Contains(rt_roll) == false)
                {
                    rt_roll = new Rt_Season_RollInSacredMud(p.getLocation());
                    p.unit.rituals.Add(rt_roll);
                }
                if (p.unit.task is Task_PerformChallenge ct && ct.challenge is Rt_Season_RollInSacredMud)
                    return;

                foreach (Property pr in p.unit.location.properties)
                {
                    if (pr is Pr_FeyPresence && pr.charge >= amountRequired)
                        p.unit.task = new Task_PerformChallenge(rt_roll);
                }



            }
        }



    }
}
