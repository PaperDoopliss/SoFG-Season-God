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
    public class P_Season_EmboldenBlood : P_Season
    {
        public P_Season_EmboldenBlood(Map map) : base(map) { }


        public override string getName()
        {
            return "Embolden Blood";
        }

        public override string getDesc()
        {
            return "Spend a quarter of a location's Fey Presence to give an agent at that location 2 XP per percent spent. If they do not already have Feyblood, they gain Feyblood.";
        }

        public override string getFlavour()
        {
            return "The Lord of the Feast can be neglectful toward his servants as he fixates on human society, but even his cast-offs can reshape a life.";
        }

        public override string getRestrictionText()
        {
            return "Must target a controlled agent in a location with at least 25% Fey Presence";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_embolden_blood.png");
        }

        public override bool validTarget(Unit unit)
        {
            if (unit is UA && unit.isCommandable())
            {

                foreach (Property pr in unit.location.properties)
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

        public override void cast(Unit unit)
        {
            base.cast(unit);

            Pr_FeyPresence presence = null;
            foreach (Property pr in unit.location.properties)
            {
                if (pr is Pr_FeyPresence foundPresence)
                {
                    presence = foundPresence;

                    if (unit.person != null)
                    {
                        unit.person.receiveXP(Convert.ToInt32(Math.Floor(presence.charge / 2.0)));

                        bool foundFeyblood = false;
                        foreach (Trait t in unit.person.traits)
                        {
                            if (t is T_Season_Feyblood)
                            {
                                foundFeyblood = true;
                                break;
                            }
                        }

                        if (!foundFeyblood)
                            unit.person.receiveTrait(new T_Season_Feyblood(unit.person, null, true));
                    }

                    break;
                }
            }

            if (presence != null)
            {
                presence.charge *= 0.75;
                if (presence.charge <= 0)
                    unit.location.properties.Remove(presence);
            }
        }


    }

}
