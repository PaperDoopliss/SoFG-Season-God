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
            return "This area has come to be a favoured stomping ground of the fey. Fey Presence can be spent to fuel your powers, and cannot rise above 300%.";
        }

        public override Sprite getSprite(World world)
        {
            return world.iconStore.nurture;
        }

        public override bool hasBackgroundHexView()
        {
            return true;
        }

        public override float getHexBackgroundViewOpacity()
        {
            return Mathf.Min(1f, (float)charge / 100f);
        }

        public override Sprite getHexBackgroundSprite()
        {
            return EventManager.getImg("ComSeasonGod.Icon_FeyPresence_Background.png");
        }

        public override void turnTick()
        {
            if (charge > 300)
                charge = 300;
        }
    }
}
