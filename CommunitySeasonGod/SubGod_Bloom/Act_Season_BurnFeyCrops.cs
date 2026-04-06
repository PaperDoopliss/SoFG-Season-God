using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Act_Season_BurnFeyCrops : Assets.Code.Action
    {

        public Pr_Season_FeyCrops crops;
        public static double devastationToInflict = 10;

        public Act_Season_BurnFeyCrops(Location loc, Pr_Season_FeyCrops crops) : base(loc) { this.crops = crops; }

        public override string getName()
        {
            return "Burn Fey Crops";
        }

        public override string getShortDesc()
        {
            return "Destroys the local Fey Crops, inflicting " + devastationToInflict + "% <b>devastation</b>. Can only be performed by aware rulers who do not have positive opinions toward The Dark.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_fey_crops.png");
        }

        public override int getTurnsRequired()
        {
            return 5;
        }

        public override bool valid(Person ruler, SettlementHuman settlementHuman)
        {
            if (ruler.awareness < 1)
                return false;
            return true;
        }

        public override double getUtility(SettlementHuman hum, Person ruler, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(hum, ruler, reasons);

            reasons?.Add(new ReasonMsg("World Panic", map.worldPanic));
            utility += map.worldPanic;

            if (ruler.getTagRanking(map.soc_dark.index + 20000) > 0)
            {
                reasons?.Add(new ReasonMsg("Trusts the Niece", -200));
                utility -= 200;
            }

            return utility;
        }

        public override int[] getNegativeTags()
        {
            return new int[2] { Tags.GOLD, Tags.SHADOW };
        }

        public override int[] getPositiveTags()
        {
            return new int[1] { Tags.CRUEL };
        }

        public override void complete()
        {
            base.complete();

            Property.addToProperty("Burnt Fey Crops", Property.standardProperties.DEVASTATION, devastationToInflict, location);
            location.properties.Remove(crops);
            if (location.settlement is SettlementHuman sh)
            {
                if (sh.ruler != null)
                {
                    map.addUnifiedMessage(sh.ruler, null, "Fey Crops Burned", sh.ruler.getName() + " has learned of the danger the Fey Crops pose and took it upon themselves to root it out. " + location.getName() + " has lost its Fey Crops, and has gained " + devastationToInflict + "% Devastation.\n\nTo avoid having your Fey Crops destroyed, prevent the farms' rulers from becoming aware, or give them liking for The Dark.", "FEY CROPS BURNED");
                }
            }

            if (map.overmind.god is God_Season season)
            {
                foreach (SubGod subGod in season.SubGods)
                {
                    if (subGod is SubGod_Bloom bloom)
                        bloom.crops.Remove(crops);
                }
            }


        }

    }
}
