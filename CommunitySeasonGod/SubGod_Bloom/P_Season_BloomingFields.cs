using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_BloomingFields : P_Season
    {

        public static int range = 4;

        public P_Season_BloomingFields(Map map) : base(map)
        {
        }

        public override string getName()
        {
            return "Blooming Fields";
        }

        public override string getDesc()
        {
            return "Create Blooming Fields at a location, granting it +" + Pr_Season_BloomingFields.foodIncrease + " <b>food</b>, +" + Pr_Season_BloomingFields.habitabilityIncrease * 100 + "% <b>habitability</b>, and " + Pr_Season_BloomingFields.presencePerTurn + "% Fey Presence per turn. If the local ruler has no opinion of The Dark, they will gain liking for it.";
        }

        public override string getFlavour()
        {
            return "The Niece's gifts are all the more insidious for being completely genuine.";
        }

        public override string getRestrictionText()
        {
            return "Must target a non-City settlement with an unaware ruler within " + range + " steps of Fey Crops";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_blooming_fields.png");
        }

        public override bool validTarget(Location loc)
        {

            if (loc.settlement is SettlementHuman sh)
            {
                if (sh is Set_City)
                    return false;


                if (sh.ruler == null || sh.ruler.awareness < 1)
                {

                    if (map.overmind.god is God_Season season && season.ActiveSubGod is SubGod_Bloom bloom)
                    {
                        foreach (Property pr in bloom.crops)
                        {
                            if (map.getStepDist(pr.location,loc) <= range)
                            {
                                return true;
                            }
                        }

                        return false;
                    }
                    return true;
                }
            }

            return false;
        }

        public override int getCost()
        {
            return 2;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            Pr_Season_BloomingFields fields = new Pr_Season_BloomingFields(loc);
            loc.properties.Add(fields);

            if (loc.settlement is SettlementHuman sh)
            {
                if (sh.ruler != null)
                {
                    if (sh.ruler.getTagRanking(map.soc_dark.index + 20000) == 0)
                        sh.ruler.likes.Add(map.soc_dark.index + 20000);
                }
            }
        }


    }
}
