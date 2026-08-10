using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class T_Season_FeyishReliquary : Trait
    {

        public T_Season_FeyishReliquary() : base()
        {

        }

        public override string getName()
        {
            return "Feyish Reliquary";
        }

        public override string getDesc()
        {
            return "Upon gaining this trait, gain one rare item and one common item.";
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);

            if (person.unit != null)
            {
                if (person.unit.isCommandable())
                {
                    Pr_ItemCache cache = new Pr_ItemCache(person.unit.location);
                    cache.items[0] = Item.getItemFromPool2(person.map);
                    cache.items[2] = Item.getItemFromPool1(person.map);
                    person.unit.location.properties.Add(cache);

                    person.map.world.prefabStore.popItemTrade(person, cache);

                    return;
                }
            }

            person.gainItem(Item.getItemFromPool2(person.map));
            person.gainItem(Item.getItemFromPool2(person.map));
        }
    }
}
