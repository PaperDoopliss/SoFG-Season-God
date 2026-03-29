using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_SpoilsOfWar : Property
    {

        public SocialGroup originalOwner;
        public static double chargeMultiplier = 5;

        public Pr_Season_SpoilsOfWar(Location loc)
            : base(loc)
        {
            originalOwner = loc.soc;
        }

        public override string getName()
        {
            return "Spoils of War";
        }

        public override string getDesc()
        {
            return "This location gains 1% increased prosperity per charge, but societies ruled by orcs or Feyblood will have a greater desire to capture it, and a sovereign that does capture it will gain " + chargeMultiplier + " gold per 1% charge.";
        }

        public override Sprite getSprite(World world)
        {
            return EventManager.getImg("ComSeasonGod.power_prepare_spoils.png");
        }

        public override bool removedOnRuin()
        {
            return true;
        }

        public override bool survivesRuin()
        {
            return false;
        }

        public override double getProsperityInfluence()
        {
            return charge / 100;
        }

        public override void turnTick()
        {
            base.turnTick();

            if (location.settlement is SettlementHuman && location.soc != originalOwner)
            {
                if (location.soc is Society conqueror)
                {
                    if (conqueror.getSovreign() != null)
                        conqueror.getSovreign().addGold(Convert.ToInt32(charge * 5));
                }
                else if (location.soc is SG_Orc conquerorc)
                {
                    Location capital = map.locations[conquerorc.capital];

                    bool foundPlunder = false;
                    foreach (Property pr in capital.properties)
                    {
                        if (pr is Pr_OrcPlunder)
                        {
                            foundPlunder = true;
                            pr.charge += charge;
                        }
                    }
                    if (!foundPlunder)
                    {
                        Pr_OrcPlunder plunder = new Pr_OrcPlunder(capital);
                        plunder.charge = charge;
                        capital.properties.Add(plunder);
                    }
                }

                location.properties.Remove(this);

            }
        }

    }
}
