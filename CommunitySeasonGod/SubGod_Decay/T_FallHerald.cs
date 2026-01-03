using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommunitySeasonGod
{
    public class T_FallHerald : Trait
    {

        public T_FallHerald() { }

        public override string getName() => "Fall Herald";

        public override string getDesc()
        {
            return $"While in a location with less than 50% <b>fey presence</b>, increase <b>fey presence</b> by 5%.";
        }

        public override int getMaxLevel() => 1;

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            var fey = p.unit.location.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            if (fey == null)
                Property.addToProperty("Trait: Fall Herald", Property.standardProperties.FAMINE, 5, p.unit.location);
            //p.unit.location.
        }

        public override int[] getTags() => new int[0];
    }
}
