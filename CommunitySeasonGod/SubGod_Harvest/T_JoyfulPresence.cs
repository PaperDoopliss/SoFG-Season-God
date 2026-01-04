using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class T_JoyfulPresence : Trait
    {
        public override string getName()
        {
            return "Joyful Presence";
        }

        public override string getDesc()
        {
            return "The Supplicant spreads Fey Presence wherever he goes";
        }

        public override int getMaxLevel()
        {
            return 1;
        }

        public override void turnTick(Person p)
        {
            foreach (Property property in p.unit.location.properties)
            {
                if (property is Pr_FeyPresence pr_FaePresence)
                {
                    pr_FaePresence.influences.Add(new ReasonMsg("Joyful Presence", 5.0));
                    return;
                }
            }

            Pr_FeyPresence pr_FeyPresence = new Pr_FeyPresence(p.unit.location);
            pr_FeyPresence.charge = 5.0;
            p.unit.location.properties.Add(pr_FeyPresence);
        }

        public override int[] getTags()
        {
            return new int[0];
        }
    }
}
