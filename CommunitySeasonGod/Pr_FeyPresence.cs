using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_FeyPresence : Property
    {
        public Pr_FeyPresence(Location loc)
            : base(loc)
        {

        }

        public override string getName()
        {
            return "Fey Presence";
        }

        public override string getInvariantName()
        {
            return "Fey Presence";
        }

        public override string getDesc()
        {
            return "This area has come to be a favoured stomping-ground of the fey folk. Their songs and laughter blend seemlessly into the winding, restless wind. Their movements flicker at the periphery of vision. Their arcane influence seeps into the land, the water, the trees. It is as alian and monstrous as it is wondrous and captivating.";
        }

        public override Sprite getSprite(World world)
        {
            return base.getSprite(world);
        }

        public override bool hasHexView()
        {
            return true;
        }

        public override Sprite getHexBackgroundSprite()
        {
            return map.world.iconStore.fadedRaiding;
        }
    }
}
