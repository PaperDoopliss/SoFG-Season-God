using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_PaleKnightsChevauchee : P_Season_LimitedCharges
    {


        public P_Season_PaleKnightsChevauchee(Map map) : base(map) { }


        public override string getName()
        {
            return "The Pale Knight's Chevauchée";
        }

        public override string getDesc()
        {
            return "Summons the Pale Knight agent under your control (Might 6, Lore 4, Intrigue 2, Command 6), as well as a Fey Knights army with " + UM_Season_FeyKnights.startingHP + " <b>hp</b>. The Fey Knights will march toward the closest populated non-Dark Empire location with at least " + UM_Season_FeyKnights.presenceTargetCutoff + "% Fey Presence, burning down any locations in the way and healing off Fey Presence in their location. The Pale Knight can change the Pale Army's target, as well as granting nearby heroes, acolytes, and agents Feyblood."; 
        }

        public override string getFlavour()
        {
            return "The season of gluttony and war reaches its climax as the fey break through to this world and feast on the trickle of fey energies that preceded them.";
        }

        public override string getRestrictionText()
        {
            return "Can target any location, but can only be cast once per season";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_chevauchee.png");
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override int getCost()
        {
            return 4;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            UM_Season_FeyKnights knights = new UM_Season_FeyKnights(loc);
            map.units.Add(knights);
            loc.units.Add(knights);

            UAE_Season_PaleKnight paleKnight = new UAE_Season_PaleKnight(loc, map.soc_dark, knights);
            map.units.Add(paleKnight);
            loc.units.Add(paleKnight);

            SpendCharge();
        }

    }
}
