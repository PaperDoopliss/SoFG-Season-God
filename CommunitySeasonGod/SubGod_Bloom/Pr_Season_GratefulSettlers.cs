using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_GratefulSettlers : Property
    {

        public Pr_Season_GratefulSettlers(Location loc)
            : base(loc)
        {

        }

        public override string getName()
        {
            return "Grateful Settlers";
        }

        public override string getDesc()
        {
            return "This place's infrastructure was grown by the Niece of Blooming Fields in the span of a week, and the gift was well-noted. Once this location appoints a ruler, they will gain extreme liking for The Dark.";
        }

        public override Sprite getSprite(World world)
        {
            return EventManager.getImg("ComSeasonGod.power_fey_bargain.png");
        }

        public override bool removedOnRuin()
        {
            return true;
        }

        public override bool survivesRuin()
        {
            return false;
        }


        public override void turnTick()
        {
            base.turnTick();

            if (location.settlement is SettlementHuman sh)
            {
                if (sh.ruler != null)
                {
                    sh.ruler.increasePreference(map.soc_dark.index + 20000);
                    sh.ruler.increasePreference(map.soc_dark.index + 20000);
                    location.properties.Remove(this);
                }
            }
            else
                location.properties.Remove(this);

        }

    }
}
