using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_MysticBounty : Trait
    {
        public double amount = 0;
        public override string getName()
        {
            return "Mystic Bounty ("+amount+")";
        }

        public override string getDesc()
        {
            return "This agent has collected the bounty of the harvest and can use it in powerful rituals. More rituals will be unlocked as seals break.";
        }

        public override int getMaxLevel()
        {
            return 300;
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);
            if (person != null)
            {
                if(person.unit != null && person.unit is UA ua)
                {
                    person.unit.rituals.Add(new Rt_Season_AgentRevelryTierI_Disrupt(person.unit.location, ua));
                    person.unit.rituals.Add(new Rt_Season_AgentRevelryTierI_Liking(person.unit.location, ua));
                    person.unit.rituals.Add(new Rt_Season_AgentRevelryTierI_ArcaneSecret(person.unit.location, ua));
                    person.unit.rituals.Add(new Rt_Season_AgentRevelryTierI_GainXP(person.unit.location, ua));
                }
            }
        }



        public override int[] getTags()
        {
            return new int[0];
        }
    }
}
