using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Act_Season_BanKudzu : Assets.Code.Action
    {
        public Pr_Season_KudzuCrisis crisis;

        public Act_Season_BanKudzu(Location loc, Pr_Season_KudzuCrisis crisis) : base(loc)
        {
            this.crisis = crisis;
        }

        public override string getName()
        {
            return "Ban Kudzu Use";
        }

        public override string getShortDesc()
        {
            return "Aware rulers and those who dislike Madness can ban the exploitation of Dreaming Kudzu by their nobles, preventing its sanity-damaging uses in reducing Unrest and political Agitation.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.property_dreaming_kudzu.png");
        }

        public override int getTurnsRequired()
        {
            return 3;
        }

        public override bool valid(Person ruler, SettlementHuman settlementHuman)
        {

            if (ruler.awareness >= 1)
                return true;

            if (ruler.getTagRanking(Tags.MADNESS) <= -1)
                return true;

            return false;
        }

        public override double getUtility(SettlementHuman hum, Person ruler, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(hum, ruler, reasons);

            utility -= 10;
            reasons?.Add(new ReasonMsg("Base Reluctance", -10));

            utility += crisis.exploitIntensity;
            reasons?.Add(new ReasonMsg("Maddened Population", crisis.exploitIntensity));

            return utility;
        }

        public override int[] getNegativeTags()
        {
            return new int[2] { Tags.MADNESS, map.soc_dark.index + 20000 };
        }

        public override void complete()
        {
            base.complete();

            crisis.exploitationOutlawed = true;
            if (location.settlement is SettlementHuman sh && sh.ruler != null)
                map.addUnifiedMessage(sh.ruler, location.soc, "Dreaming Kudzu Outlawed", sh.ruler.getName() + " has seen the dangers of using Dreaming Kudzu to control the population, and has declared that any use of Dreaming Kudzu on one's own people will be punished to the full extent of the law. Rulers will once again need to use other tools to preserve their reign.", "DREAMING KUDZU OUTLAWED");

        }

    }
}
