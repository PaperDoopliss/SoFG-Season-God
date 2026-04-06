using Assets.Code;
using CommunityLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class UAEN_Season_VineTerror : UAEN
    {
        public Location target = null;

        public UAEN_Season_VineTerror(Location loc, SocialGroup sg, Person p) : base(loc, sg, p)
        {

            bool hasVineTerrorTrait = false;

            foreach (Trait t in person.traits)
            {
                if (t is T_Season_VineTerror)
                    hasVineTerrorTrait = true;
            }
            if (!hasVineTerrorTrait)
                person.receiveTrait(new T_Season_VineTerror());


            minions[0] = new M_Season_Tendril(map);
            minions[1] = new M_Season_Tendril(map);
            minions[2] = new M_Season_Tendril(map);

            inner_profile = 20;
            inner_profileMin = 20;
            inner_menace = 90;
            inner_menaceMin = 90;

            p.stat_might = 1;
            p.stat_lore = 2;
            p.stat_intrigue = 1;
            p.stat_command = 1;
            p.species = map.species_monster;
            turnTickAI();
        }

        public override string getName()
        {
            return "Vine Terror";
        }

        public override Sprite getPortraitForeground()
        {
            return EventManager.getImg("ComSeasonGod.unit_vine_terror.png");
        }

        public void findTarget()
        {
            target = null;

            List<Location> possibleTargets = new List<Location>();
            foreach (Location l in map.locations)
            {
                if (l.settlement is SettlementHuman)
                    possibleTargets.Add(l);
            }

            if (possibleTargets.Count > 0)
                target = possibleTargets[Eleven.random.Next(possibleTargets.Count)];
        }

        public override void turnTickAI()
        {
            if (target == null || target.settlement is SettlementHuman == false)
            {
                findTarget();
            }

            if (target != null)
                task = new Task_Season_VineTerrorRoot(target, this);

        }

    }
}
