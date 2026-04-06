using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class UAE_Season_GardenNymph : UAE
    {
        public UAE_Season_GardenNymph(Location loc, Society sg, Person p) : base(loc, sg, p)
        {
            p.stat_command = 1;
            p.stat_intrigue = 1;
            p.stat_lore = 1;
            p.stat_might = 1;
            p.species = map.species_monster;
            p.isMale = false;
            for (int i = 0; i < p.traits.Count; i++)
            {
                if (p.traits[i] is T_Season_VerdantImmortality)
                {
                    p.traits.RemoveAt(i);
                    i--;
                }
            }

            p.receiveTrait(new T_Season_DeepRoots());
            movesTaken = getMaxMoves();
            rituals.Add(new Rt_Season_MeldIntoNature(loc));
            rituals.Add(new Rt_Season_ReturnToTheEarth(loc));
        }

        public override bool definesName()
        {
            return true;
        }

        public override string getName()
        {
            if (base.person.overrideName != null && base.person.overrideName.Length != 0)
            {
                return base.person.overrideName;
            }

            return "Garden Nymph";
        }

        public override bool isCommandable()
        {
            return true;
        }

        public override bool hasStartingTraits()
        {
            return false;
        }

        public override Sprite getPortraitBackground()
        {
            return map.world.iconStore.standardBack;
        }

        public override Sprite getPortraitForeground()
        {
            return EventManager.getImg("ComSeasonGod.unit_garden_nymph.png");
        }


    }
}
