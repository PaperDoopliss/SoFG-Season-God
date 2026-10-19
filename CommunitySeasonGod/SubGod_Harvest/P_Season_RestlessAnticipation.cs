using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;

namespace CommunitySeasonGod
{
    public class P_Season_RestlessAnticipation : P_Season
    {
        public P_Season_RestlessAnticipation(Map map) : base(map) { }

        public override string getName()
        {
            return "Restless Anticipation";
        }

        public override string getDesc()
        {
            return "Adds Lingering Resentment in location with Fae Presence. The modifier's charge will match that of the local Fey Presence (up to 30%).";
        }

        public override string getFlavour()
        {
            return "The harsh seasons that came before have created great tension in the people here. Watching the Harvest ripen around them they begin to long for a catharsis, to finally let loose and live instead of survive.";
        }
        public override string getRestrictionText()
        {
            return "Must be cast on a location with Fey Presences.";
        }



        public override int getCost()
        {
            return 1;
        }
        public override bool validTarget(Location loc)
        {
            foreach (Property prop in loc.properties)
            {

                if (prop is Pr_FeyPresence)
                {
                    return true;
                }
            }

            return false;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);
            Pr_FeyPresence presence = loc.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            if (presence != null)
            {
                Pr_LingeringResentment resentment = new Pr_LingeringResentment(loc);
                resentment.charge = presence.charge;
                if(resentment.charge >= 30)
                {
                    resentment.charge = 30;
                }
                loc.properties.Add(resentment);
            }
        }
    }
}
