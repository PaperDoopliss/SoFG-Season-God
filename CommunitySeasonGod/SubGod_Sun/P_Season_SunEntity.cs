using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_SunEntity : P_Season
    {

        public P_Season_SunEntity(Map map) : base(map) { }

        public override string getName()
        {
            return "Entity of the Sun";
        }

        public override string getDesc()
        {
            return "Creates a non-controllable Solar Entity agent that steadily increases temperature in farmlands and surrounding areas until it is killed.";
        }

        public override string getFlavour()
        {
            return "The Patriarch weaves a vessel of light and fire to spread his barren glory across the land.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_entity_of_the_sun.png");
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override int getCost()
        {
            return /*3*/0;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            UAEN_SolEntity entity = new UAEN_SolEntity(location, map.soc_dark, map);
            map.units.Add(entity);
            location.units.Add(entity);

        }


    }
}