using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class UAE_Season_PaleKnight : UAE
    {

      public UAE_Season_PaleKnight(Location loc, Society sg, UM_Season_FeyKnights knights) : base(loc, sg)
      {
            base.person.stat_might = 5;
            base.person.stat_command = 5;
            base.person.stat_intrigue = 2;
            base.person.stat_lore = 3;
            base.person.isMale = true;
            T_Season_LordOfTheFeast chosen = new T_Season_LordOfTheFeast();
            base.person.receiveTrait(chosen);
            T_Season_Feyblood feyblood = new T_Season_Feyblood(person);
            feyblood.skipMessage = true;
            person.receiveTrait(feyblood);

            rituals.Add(new Rt_Season_BeckonKnights(loc, knights));
            rituals.Add(new Rt_Season_BestowFeyblood(loc));
            base.person.species = map.species_monster;
      }


        public override string getName()
        {
            if (base.person.overrideName != null && base.person.overrideName.Length != 0)
            {
                return base.person.overrideName;
            }

            return "The Pale Knight";
        }

        public override bool isCommandable()
        {
            return true;
        }

        public override Sprite getPortraitBackground()
        {
            return map.world.iconStore.standardBack;
        }

        public override Sprite getPortraitForeground()
        {
            return EventManager.getImg("ComSeasonGod.unit_pale_knight.png");
        }



    }
}
