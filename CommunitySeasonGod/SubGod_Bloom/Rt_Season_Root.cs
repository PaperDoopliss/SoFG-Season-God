using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace CommunitySeasonGod
{
    public class Rt_Season_Root : Ritual
    {
        public Rt_Season_Root(Location location) : base(location)
        {
        }

        public override string getName()
        {
            return "Root";
        }

        public override string getDesc()
        {
            return "Transforms this person into a Garden Nymph who does not contribute to agent cap, but has low base stats and cannot move from the location they've rooted in.";
        }

        public override string getRestriction()
        {
            return "Cannot be performed in the ocean";
        }

        public override double getComplexity()
        {
            return 1;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Base", 1));
            return 1;
        }

        public override bool valid()
        {
            return true;
        }

        public override bool validFor(UA ua)
        {
            return ua.location.isOcean == false;
        }

        public override Sprite getSprite()
        {
            return EventManager.getImg("ComSeasonGod.challenge_return_to_earth.png");
        }

        public override string getCastFlavour()
        {
            return "An overwhelming urge to fall asleep on bare soil. The body dissolves away while something new gestates beneath the grass. She always belonged to the Niece of Blooming Fields, but now the eldritch god is the whole of her existence.";
        }

        public override void complete(UA u)
        {
            base.complete(u);

            map.units.Remove(u);
            u.location.units.Remove(u);
            UAE_Season_GardenNymph nymph = new UAE_Season_GardenNymph(u.location, u.map.soc_dark, u.person);
            u.map.units.Add(nymph);
            nymph.location.units.Add(nymph);

            if (GraphicalMap.selectedUnit == u)
                GraphicalMap.selectedUnit = null;
        }



    }
}
