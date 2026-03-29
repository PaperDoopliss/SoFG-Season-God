using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class T_Season_HonoredHost : Trait
    {

        public T_Season_HonoredHost() : base()
        {

        }

        public override string getName()
        {
            return "Honored Host";
        }

        public override string getDesc()
        {
            return "Upon gaining this trait, gain a Fey Knight minion (5 <b>attack</b>, 4 <b>defence</b>, <b>hp</b>, 2 command cost).";
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);
            if (!(person.unit is UA uA))
            {
                return;
            }

            M_Season_FeyKnight m_knight = new M_Season_FeyKnight(person.map);
            for (int i = 0; i < uA.minions.Length; i++)
            {
                if (uA.minions[i] == null)
                {
                    uA.minions[i] = m_knight;
                    break;
                }
            }
        }

        public override int[] getTags()
        {
            return new int[1] { Tags.COMBAT };
        }
    }
}
