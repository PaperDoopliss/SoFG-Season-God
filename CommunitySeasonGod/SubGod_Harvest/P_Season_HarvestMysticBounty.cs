using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_HarvestMysticBounty : P_Season
    {

        public static double amountTake = 50;
        public static int amountGive = 4;
        public P_Season_HarvestMysticBounty(Map map)
            : base(map)
        {

        }

        public override string getName()
        {
            return "Mystic Harvest";
        }

        public override string getDesc()
        {
            return "Converts "+ amountTake + "% charge of a Fey Presence into 2 Mystic Bounty for an agent. The Mystic Bounty trait allows for additional rituals that can be used outside of the Uncle of Harvest's season.";
        }

        public override string getFlavour()
        {
            return "Some thought is put, as the festivals begin, to the coming months and seasons. The rational suggest storing some away to be used another day.";
        }

        public override string getRestrictionText()
        {
            return "Must target an agent under your control who is in a location with a Fey Presence modifer that has >= "+ amountTake + "% charge.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.give;
        }

        public override bool validTarget(Unit unit)
        {
            foreach (Property prop in unit.location.properties)
            {

                if (prop is Pr_FeyPresence presence && presence.charge >= amountTake)
                {
                    if (unit is UA uA && uA.isCommandable())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override int getCost()
        {
            return 3;
        }


        public override void cast(Unit u)
        {
            base.cast(u);
            T_MysticBounty trait = u.person.traits.OfType<T_MysticBounty>().FirstOrDefault();
            if(trait != null)
            {
                trait.amount += amountGive;
            }
            else
            {
                trait = new T_MysticBounty();
                trait.amount = amountGive;
                u.person.receiveTrait(trait);
            }

            Pr_FeyPresence presence = u.location.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            if(presence != null)
            {
                presence.charge -= amountTake;
            }
        }


    }
}
