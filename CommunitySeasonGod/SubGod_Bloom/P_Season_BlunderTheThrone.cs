using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_BlunderTheThrone : P_Season
    {

        public P_Season_BlunderTheThrone(Map map) : base(map)
        {   
        }

        public override string getName()
        {
            return "Blunder the Throne";
        }

        public override string getDesc()
        {
            return "Brings up a list of seasons which you can immediately switch to, allowing early transition to a season of your choice. This power does not cost Fey Presence, and grants the normal Season Changes bonus.";
        }

        public override string getFlavour()
        {
            return "The eldritch god's fragments do not squabble in the traditional sense, but clumsiness and inexperience can still allow another to seize control.";
        }

        public override string getRestrictionText()
        {
            return "";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_blunder_the_throne.png");
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override int getCost()
        {
            return 1;
        }

        public override void cast(Location location)
        {
            if (map.overmind.god is God_Season seasonGod)
            {
                seasonGod.PresentDraft();
                seasonGod.forceNextShiftNatural(true);
            }
        }




    }
}
