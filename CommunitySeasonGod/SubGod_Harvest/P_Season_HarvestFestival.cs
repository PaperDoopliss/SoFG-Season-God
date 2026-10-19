using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_HarvestFestival : P_Season
    {
        public static double feyCharge = 100;
        public P_Season_HarvestFestival(Map map) : base(map) { }

        public override string getName()
        {
            return "Harvest Festival";
        }

        public override string getDesc()
        {
            return "Destroys the targeted settlement and replaces it with the Harvest Festival a semi-permanent location that spreads shadow and fae presence. This location will periodically spawns a random Reveler with the Lingering Reveler trait, which will allow them to persist for a time after the season changes. Once the season changes, the Harvest Festival ends and takes the settlement with it.";
        }

        public override string getFlavour()
        {
            return "The harvest was collected. The feast had commenced. They did not notice the uninvited guests, that their cups no longer ran empty, that the music was unearthly or that the pleasures they partook in were inhuman. At the climax of the festival they felt the euphoria as two worlds collided and a being stepped through. The Uncle of the Harvest had come to this world, and in his presence the festival never ended.";
        }
        public override string getRestrictionText()
        {
            return "Must be cast on a fully infiltrated settlement Fey Presence >= "+feyCharge+".";
        }

        public override int getCost()
        {
            return 5;
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }
        public override bool validTarget(Location loc)
        {   
            if (loc.settlement == null)
            {
                return false;
            }

            if (Kernel_Season.ComLibKernel.checkIsElderTomb(loc))
            {
                return false;
            }

            if(Kernel_Season.ComLibKernel.checkIsNaturalWonder(loc))
            {
                return false; 
            }

            if (loc.settlement is SettlementHuman)
            {
                if (loc.settlement.isInfiltrated)
                {
                    Pr_FeyPresence presence = loc.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
                    if (presence != null)
                    {
                        if(presence.charge >= feyCharge)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);
            Settlement oldSettlement = loc.settlement;
            oldSettlement.fallIntoRuin("They are lost to the delights of the festival");
            Set_HarvestFestival festival = new Set_HarvestFestival(loc); //stores the harvest festival
            loc.settlement = festival;
            foreach (Subsettlement sub in oldSettlement.subs)
            {
                sub.settlement = festival;
                festival.subs.Add(sub);
            }
            oldSettlement.subs.Clear();
        }
    }
}
