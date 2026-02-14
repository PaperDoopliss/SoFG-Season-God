using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_IndustriousNewcomers : Property
    {

        public bool fromMadness = false;
        public Pr_Season_IndustriousNewcomers(Location loc)
            : base(loc)
        {

        }

        public override string getName()
        {
            return "Industrious Newcomers";
        }

        public override string getDesc()
        {
            return "Well-prepared land and psychically compelled harmony grant this location increased <b>food</b> and <b>habitability</b>.";
        }

        public override Sprite getSprite(World world)
        {
            return world.iconStore.nurture;
        }

        public override bool removedOnRuin()
        {
            return true;
        }

        public override bool survivesRuin()
        {
            return false;
        }

        public override int getFoodGeneratedFlat()
        {
            double toAdd = charge - ((charge / map.param.city_popMaxPerHabilitability) * map.param.city_foodPerHabilitability);
            return (int)Math.Ceiling(toAdd);
        }

    }
}
