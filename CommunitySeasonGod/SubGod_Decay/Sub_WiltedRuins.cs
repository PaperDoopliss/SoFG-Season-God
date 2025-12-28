using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Sub_WiltedRuins : Subsettlement
    {
        public List<Challenge> challenges;

        public Sub_WiltedRuins(Settlement set)
          : base(set)
        {
            this.challenges = new List<Challenge>();
            this.challenges.Add((Challenge)new Ch_LayLowWilderness(set.location));
            this.infiltrated = true;
        }

        public override string getName() => "Wilted Ruins";

        public override string getHoverOverText()
        {
            return "The rotting carcass of the settlement that once stood here, in all of its rotting newly revealed beauty.";
        }

        public override string getIconText() => "Menace: " + this.menace.ToString();

        public override bool canBeInfiltrated() => false;

        public override bool survivesRuin() => false;

        public override List<Challenge> getChallenges() => this.challenges;

        public override Sprite getIcon() => this.settlement.map.world.iconStore.ancientRuins;


    }
}
