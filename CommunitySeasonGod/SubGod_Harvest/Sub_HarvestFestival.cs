using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class Sub_HarvestFestival : Subsettlement
    {
        public List<Challenge> challenges;
        public Sub_HarvestFestival(Set_HarvestFestival set) : base(set)
        {
            challenges = new List<Challenge>();
            new Ch_LayLowWilderness(set.location);
            new Ch_Harvest_Revel(set.location);
           
            infiltrated = true;
        }
        public override string getName()
        {
            return "The Harvest Festival";
        }

        public override string getHoverOverText()
        {
            return "Within the heart of this storm of shadow and mist a god-prince leads his following in a ritual of celebration and excess.";
        }
        public override List<Challenge> getChallenges()
        {
            return challenges;
        }
    }
}
