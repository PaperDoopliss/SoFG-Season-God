using Assets.Code;
using Assets.Code.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Set_WiltedGrove : Settlement
    {
        public List<Challenge> challenges = new List<Challenge>();
        public int Size { get; private set; }
        public int armyRebuildTimer = 0;

        public Set_WiltedGrove(Location loc, int size)
          : base(loc)
        {
            this.fallIntoRuin("Rotted into a Wilted Grove");
            this.subs.Add(new Sub_WiltedRuins(this));
            this.shadow = 1.0;
            this.shadowPolicy = Settlement.shadowResponse.FULL_FLOW;
            Size = size;
            loc.soc = this.map.soc_dark;
        }

        public override Sprite getSprite()
        {
            return this.location.map.world.textureStore.loc_evil_hive;
        }

        public override List<Challenge> getChallenges()
        {
            this.challenges.Clear();
            return this.challenges;
        }

        public override string getName() => "Wilted Grove";

        public override double getMaxDefence() => Size;

        public override void turnTick()
        {
            base.turnTick();
            armyRebuildTimer += 1;
            if (armyRebuildTimer == 7)
            {
                armyRebuildTimer = 0;
                this.map.units.Add(new UM_WiltedHorde(this.location, this.location.soc, this, this.Size));
            }
        }
    }
}
