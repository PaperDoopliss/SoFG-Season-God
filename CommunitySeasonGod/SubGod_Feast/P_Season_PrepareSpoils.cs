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

    //Update for Orcs once we're in Harmony mode
    public class P_Season_PrepareSpoils : P_Season
    {
        public P_Season_PrepareSpoils(Map map) : base(map) { }


        public override string getName()
        {
            return "Prepare Spoils";
        }

        public override string getDesc()
        {
            return "Spend 25% Fey Presence to add 50% Spoils of War to a location. Spoils of War increases <b>prosperity</b> by its charge, but makes Feyblood sovereigns and orc horrdes more likely to invade them.";
        }

        public override string getFlavour()
        {
            return "There are always those souls who do honest toil to make their lands healthier. They create delicious prizes.";
        }

        public override string getRestrictionText()
        {
            return "Must target a populated settlement with at least 25% Fey Presence";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_prepare_spoils.png");
        }

        public override bool validTarget(Location loc)
        {
            if (loc.settlement is SettlementHuman)
            {
                foreach (Property pr in loc.properties)
                {
                    if (pr is Pr_FeyPresence && pr.charge >= 25)
                        return true;
                }
            }

            return false;
        }

        public override int getCost()
        {
            return 2;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            Pr_FeyPresence presence = null;
            Pr_Season_SpoilsOfWar spoils = null;
            foreach (Property pr in location.properties)
            {
                if (pr is Pr_FeyPresence foundPresence)
                    presence = foundPresence;
                else if (pr is Pr_Season_SpoilsOfWar foundSpoils)
                    spoils = foundSpoils;
            }

            if (presence != null)
            {
                if (spoils != null)
                {
                    spoils.charge += 50;
                }
                else
                {
                    spoils = new Pr_Season_SpoilsOfWar(location);
                    spoils.charge = 50;
                    location.properties.Add(spoils);
                }
                presence.charge -= 25;
                if (presence.charge <= 0)
                    location.properties.Remove(presence);
            }
        }


    }

}
