using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;

namespace CommunitySeasonGod
{
    public class Ch_Harvest_Revel : Challenge
    {
        public Ch_Harvest_Revel(Location loc) : base(loc) { }
        public override string getName()
        {
            return "Revel";
        }

        public override double getMenace()
        {
            return 10;
        }

        public override double getProfile()
        {
            return 3;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override bool validFor(UA ua)
        {
            return ua.isCommandable();
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.LORE;
        }

        public override double getComplexity()
        {
            return 40.0;
        }

        
        public List<Location> getAffected()
        {
            List<Location> list = new List<Location>();

            list.Add(base.location);

            foreach (Location neighbour in base.location.getNeighbours())
            {
                list.Add(neighbour);
            }

            return list;
        }
        public override void onBegin(Unit unit)
        {
            CommunityLib.ModCore.Get().ResetTrackedPerTurnChallengeProgress(this, unit); //This and...
        }
        public override void turnTick(UA ua)
        {
            base.turnTick(ua);
            double progressMade = CommunityLib.ModCore.Get().CalculateScaledProgressPerTurn(this, ua); //this are new community library hooks that ILikeGoodFood asked me to use, i think the errors are because I don't have community library updated
            

            double delta = 1.2 * progressMade;
            List<Location> affected = getAffected();

            foreach (Location loc in affected)
            {
                Pr_FeyPresence existing = null; 

                foreach (Property property in loc.properties)
                {
                    if (property is Pr_FeyPresence)
                    {
                        existing = (Pr_FeyPresence)property; 
                    }
                }

                if (existing != null) 
                {
                    existing.influences.Add(new ReasonMsg("Revelry", delta)); 
                }
                else 
                {
                    Pr_FeyPresence pr_FeyPresence = new Pr_FeyPresence(loc); 
                    pr_FeyPresence.charge = delta;
                    loc.properties.Add(pr_FeyPresence);
                }
            }
        }
    }
}
