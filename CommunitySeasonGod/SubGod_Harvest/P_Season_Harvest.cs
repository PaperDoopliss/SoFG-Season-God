using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;

namespace CommunitySeasonGod
{
    public class P_Season_Harvest : P_Season
    {
        public P_Season_Harvest(Map map) : base(map) { }

        public override string getName()
        {
            return "The Harvest";
        }

        public override string getDesc()
        {
            return "Transforms Fey Presence into the Harvests Bounty agent trait. This can be used to perform powerful rituals. More rituals will be unlocked as seals break.";
        }

        public override string getFlavour()
        {
            return "Harvest time has come, any willing to put in the work to feed the many will be blessed and thanked by the Uncle.";
        }
        public override string getRestrictionText()
        {
            return "Must be cast on an agent in a location with Fey Presence.";
        }



        public override int getCost()
        {
            return 2;
        }
        public override bool validTarget(Unit unit)
        {
            if (unit is UA uA && uA.isCommandable())

                foreach (Property prop in unit.location.properties)
                {

                    if (prop is Pr_FeyPresence)
                    {
                        return true;
                    }
                }
                
            return false;
        }
        public override bool validTarget(Location loc)
        {
            return false;
        }

        public override void cast(Unit u)
        {   
            base.cast(u);
            foreach (Property prop in u.location.properties)
            {
                if (prop is Pr_FeyPresence fey)
                {
                    double charge = fey.charge;
                    fey.charge = 0;
                    
                    foreach (Trait T in u.person.traits)
                    {
                        if (T is T_MysticBounty harvest)
                        {
                            harvest.amount = harvest.amount + charge;
                            return;
                        }
                    }
                    
                    T_MysticBounty newBounty = new T_MysticBounty();
                    newBounty.amount = charge;
                    u.person.receiveTrait(newBounty);
                }
            }

        }
    }
}
