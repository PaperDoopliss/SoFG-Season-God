using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_HopefulPromises : P_Season
    {

        public static double presenceCostForm = 300;
        public static double presenceCostJoin = 150;

        public static int costForm = 6;
        public static int costJoin = 3;

        public static double range = 4;

        public P_Season_HopefulPromises(Map map) : base(map) { }

        public override string getName()
        {
            foreach (SocialGroup sg in map.socialGroups)
            {
                if (sg is Society soc)
                {
                    if (soc.isDarkEmpire && soc.isGone() == false)
                        return "Hopeful Promises (Join Empire)";

                }
            }
            return "Hopeful Promises (Form Empire)";

        }

        public override string getDesc()
        {
            return "If a Dark Empire is already active, spends " + presenceCostJoin + " Fey Presence and " + costJoin + " <b>power</b> to make another nation join the Dark Empire. If no Dark Empire is active, spends " + presenceCostForm + " Fey Presence and " + costForm + " <b>power</b> to turn a nation into the Dark Empire. All dukes with personal AND location shadow < 90% will immediately rebel in either case.";
        }

        public override string getFlavour()
        {
            return "The Niece, childish as she is, can bring hope for a kinder age blanketed in life and darkness.";
        }

        public override string getRestrictionText()
        {
            foreach (SocialGroup sg in map.socialGroups)
            {
                if (sg is Society soc)
                {
                    if (soc.isDarkEmpire && soc.isGone() == false)
                        return "Must target a non-Dark Empire capital with 100% shadow within " + range + " steps of Fey Crops. The target nation must have at least " + presenceCostJoin + " Fey Presence in all of its lands combined. Cannot target Holy Orders.";

                }
            }
            return "Must target a capital with 100% shadow within " + range + " steps of Fey Crops. The target nation must have at least " + presenceCostForm + " Fey Presence in all of its lands combined. Cannot target Holy Orders.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_hopeful_promises.png");
        }

        public override bool validTarget(Location loc)
        {
            if (loc.settlement is SettlementHuman && loc.soc is HolyOrder == false)
            {
                if (loc.getShadow() >= 0.99)
                {
                    if (loc.soc is Society soc && soc.isDarkEmpire == false)
                    {
                        if (loc.soc.getCapitalHex() == loc.hex)
                        {


                            if (map.overmind.god is God_Season season && season.ActiveSubGod is SubGod_Bloom bloom)
                            {
                                foreach (Property pr2 in bloom.crops)
                                {
                                    if (map.getStepDist(pr2.location, loc) <= range)
                                    {
                                        double cost = presenceCostForm;
                                        foreach (SocialGroup sg in map.socialGroups)
                                        {
                                            if (sg is Society soc2)
                                            {
                                                if (soc2.isDarkEmpire && soc2.isGone() == false)
                                                    cost = presenceCostJoin;

                                            }
                                        }

                                        double presence = 0;
                                        foreach (Location l in map.locations) 
                                        {
                                            if (l.soc == loc.soc)
                                            {
                                                foreach (Property pr in loc.properties)
                                                {
                                                    if (pr is Pr_FeyPresence)
                                                    {
                                                        presence += pr.charge;
                                                        if (presence >= cost)
                                                        {
                                                            return true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        return false;

                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public override int getCost()
        {
            foreach (SocialGroup sg in map.socialGroups)
            {
                if (sg is Society soc)
                {
                    if (soc.isDarkEmpire && soc.isGone() == false)
                        return costJoin;

                }
            }
            return costForm;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);


            double presenceSpent = 0;

            foreach (SocialGroup sg in map.socialGroups)
            {
                if (sg is Society soc)
                {
                    if (soc.isDarkEmpire && soc.isGone() == false)
                    {

                        List<Location> locationsToConvert = new List<Location>();

                        //Join Version
                        foreach (Location l in map.locations)
                        {
                            if (l.soc == loc.soc)
                            {
                                locationsToConvert.Add(l);

                                if (presenceSpent < presenceCostJoin)
                                {
                                    for (int i = 0; i < l.properties.Count; i++)
                                    {
                                        if (l.properties[i] is Pr_FeyPresence)
                                        {
                                            if (l.properties[i].charge >= presenceCostJoin - presenceSpent)
                                            {
                                                l.properties[i].charge -= presenceCostJoin - presenceSpent;
                                                presenceSpent = presenceCostJoin;
                                                break;
                                            }
                                            else
                                            {
                                                presenceSpent += l.properties[i].charge;
                                                l.properties.RemoveAt(i);
                                                i--;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        List<Location> rebels = new List<Location>();
                        foreach (Location l in locationsToConvert)
                        {
                            l.soc = soc;
                            if (l.person() != null)
                                l.person().society = soc;

                            if (l.settlement is Set_City && l.person() != null && l.person().shadow < 0.9 && l.getShadow() < 0.9)
                                rebels.Add(l);
                        }

                        if (rebels.Count > 0)
                            soc.triggerCivilWar(rebels);
                        

                        return;
                    }

                }
            }

            //Form Version
            List<Location> rebels2 = new List<Location>();
            foreach (Location l in map.locations)
            {
                if (l.soc == loc.soc)
                {

                    if (l.settlement is Set_City && l.person() != null && l.person().shadow < 0.9 && l.getShadow() < 0.9)
                        rebels2.Add(l);

                    if (presenceSpent < presenceCostForm)
                    {
                        for (int i = 0; i < l.properties.Count; i++)
                        {
                            if (l.properties[i] is Pr_FeyPresence)
                            {
                                if (l.properties[i].charge >= presenceCostForm - presenceSpent)
                                {
                                    l.properties[i].charge -= presenceCostForm - presenceSpent;
                                    presenceSpent = presenceCostForm;
                                    break;
                                }
                                else
                                {
                                    presenceSpent += l.properties[i].charge;
                                    l.properties.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                    }
                }
            }

            if (loc.soc is Society soc2)
            {
                soc2.isDarkEmpire = true;
                for (int i = 0; i < 3; i++)
                {
                    soc2.color[i] /= 3f;
                }
                if (rebels2.Count > 0)
                    soc2.triggerCivilWar(rebels2);
            }


        }

    }
}
