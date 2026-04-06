using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_VerdantImmortality : Trait
    {

        public Rt_Season_Root rt_root;

        public T_Season_VerdantImmortality()
        {
            rt_root = new Rt_Season_Root(World.staticMap.locations[0]);
        }

        public override string getName()
        {
            return "Verdant Immortality";
        }

        public override string getDesc()
        {
            return "When killed on land, this person resurrects as a Garden Nymph that has a base of 1 in all stats and cannot move from the location they died in.";
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);

            if (person.unit != null)
            {
                person.unit.rituals.Add(rt_root);
            }
        }


        public override void turnTick(Person p)
        {
            base.turnTick(p);

            if (p.unit != null)
            {
                if (p.unit.rituals.Contains(rt_root) == false)
                    p.unit.rituals.Add(rt_root);
            }
        }

        /*public override void onDeath(Unit unit, Person killer)
        {
            base.onDeath(unit, killer);

            if (unit.location.isOcean == false)
            {

                if (unit.person != null)
                {
                    UAE_Season_GardenNymph nymph = new UAE_Season_GardenNymph(unit.location, unit.map.soc_dark, unit.person);
                    unit.map.units.Add(nymph);
                    nymph.location.units.Add(nymph);
                    unit.person.isDead = false;

                    for (int i = 0; i < unit.location.properties.Count; i++)
                    {
                        if (unit.location.properties[i] is Pr_FallenHuman soul)
                        {
                            if (soul.personIndex == unit.person.index)
                            {
                                unit.location.properties.RemoveAt(i);
                                break;
                            }
                        }
                    }

                    unit.map.addUnifiedMessage(unit, nymph, "Verdant Rebirth", "A day after " + unit.getName() + "'s death, a massive flower sprouts in " + unit.location.getName() + "'s outskirts. In the week to come the bloom unfurls, revealing a creature cloaked in leaves and appearing almost human. In many ways her nature is new and strange, but the eldritch god's psychic presence is the same as ever, and her immortal life will be spent in furtherance of the work.\n\n" + unit.getName() + " has been reborn as a Garden Nymph.", "VERDANT REBIRTH");
                }
            }
        }*/



    }
}
