using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_HiddenArtifact : Property
    {

        public Item item;
        public Ch_Season_SeekArtifactEvil ch_seekEvil;
        public Ch_Season_SeekArtifactGood ch_seekGood;

        public Pr_Season_HiddenArtifact(Location loc, Item item)
            : base(loc)
        {
            this.item = item;
            ch_seekEvil = new Ch_Season_SeekArtifactEvil(loc, this);
            ch_seekGood = new Ch_Season_SeekArtifactGood(loc, this);
        }

        public override string getInvariantName()
        {
            return "Fey Artifact";
        }

        public override string getName()
        {
            return item.getName();
        }

        public override string getDesc()
        {
            return "A " + item.getName() + " is here, and can be obtained through a challenge. Its effects are as follows:\n\n" + item.getShortDesc();
        }

        public override Sprite getSprite(World world)
        {
            return item.getIconFore();
        }

        public override bool removedOnRuin()
        {
            return false;
        }

        public override bool survivesRuin()
        {
            return true;
        }

        public override List<Challenge> getChallenges()
        {
            List<Challenge> result = new List<Challenge>();

            result.Add(ch_seekEvil);
            result.Add(ch_seekGood);

            return result;
        }

    }
}
