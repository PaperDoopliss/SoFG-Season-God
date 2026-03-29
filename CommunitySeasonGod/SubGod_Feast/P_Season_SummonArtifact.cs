using Assets.Code;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{

    //Update for underground once versioning is in?

    public class P_Season_SummonArtifact : P_Season
    {
        public P_Season_SummonArtifact(Map map) : base(map) { }


        public override string getName()
        {
            return "Summon Artifact";
        }

        public override string getDesc()
        {
            return "Creates a randomly-selected artifact that boosts one of Might, Lore, or Intrigue alongside other effects. The artifact appears at a random land location that will normally not be enshadowed, in a society that has a Feyflood sovereign, or owned by the Alliance. It must be uncovered using a challenge or quest before being claimed, and heroes may race your agents for ownership of the artifact.";
        }

        public override string getFlavour()
        {
            return "It is a time of opulence and hunger, where magnificent relics are ripe for the taking.";
        }

        public override string getRestrictionText()
        {
            return "The artifact will appear in a random location, regardless of target";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_summon_artifact.png");
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override int getCost()
        {
            return 3;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            List<Location> preferredLocations = new List<Location>();
            List<Location> allPossibleLocations = new List<Location>();

            foreach (Location l in map.locations)
            {
                if (l.isOcean == false)
                {
                    allPossibleLocations.Add(l);

                    bool notPreferred = false;
                    if (l.soc is Society soc)
                    {
                        if (soc.isAlliance)
                            notPreferred = true;
                        else if (soc.getSovreign() != null)
                        {
                            foreach (Trait t in soc.getSovreign().traits)
                            {
                                if (t is T_Season_Feyblood)
                                {
                                    notPreferred = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (l.getShadow() >= 0.5)
                        notPreferred = true;

                    if (!notPreferred)
                        preferredLocations.Add(l);
                }
            }


            if (allPossibleLocations.Count > 0)
            {

                Pr_Season_HiddenArtifact property = null;
                Item artifact = null;

                double d = Eleven.random.NextDouble();
                if (d <= 0.3333)
                    artifact = new I_Season_CourtDuelingBlade(map);
                else if (d <= 0.6666)
                    artifact = new I_Season_FeyishEternalGoblet(map);
                else
                    artifact = new I_Season_ServantsSpectacles(map);

                if (preferredLocations.Count > 0)
                    property = new Pr_Season_HiddenArtifact(preferredLocations[Eleven.random.Next(preferredLocations.Count)], artifact);
                else
                    property = new Pr_Season_HiddenArtifact(allPossibleLocations[Eleven.random.Next(allPossibleLocations.Count)], artifact);

                property.location.properties.Add(property);

                map.addUnifiedMessage(property.location, null, "Fey Artifact", "A " + artifact.getName() + " has appeared at " + property.location.getName() + ". Its latent power will call to heroes nearby, who will seek to hunt it down before your agents can. Once retrieved, its effects are as follows:\n\n" + artifact.getShortDesc(), "FEY ARTIFACT");
            }
        }
    }

}
