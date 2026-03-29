using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class Task_Season_SackLocation : Assets.Code.Task
    {

        public override string getShort()
        {
            return "Sacking location";
        }

        public override string getLong()
        {
            return "This army is sacking this location and sending gold home until nothing remains.";
        }

        public override void turnTick(Unit unit)
        {
            if (unit is UM_HumanArmy uM)
            {
                SettlementHuman sh = uM.location.settlement as SettlementHuman;

                if (sh == null)
                {
                    unit.task = null;
                    return;
                }

                Person lord = null;
                if (uM.homeLocation >= 0 && unit.map.locations[uM.homeLocation].settlement is SettlementHuman home)
                    lord = home.ruler;

                unit.map.world.prefabStore.particleCombat(unit.location.hex, unit.location.hex);
                if (lord != null)
                {
                    lord.gold += (int)(Math.Round(8 * sh.prosperity));
                }
                Property.addToProperty("Military Action", Property.standardProperties.DEVASTATION, 8.0, uM.location);

                uM.location.settlement.defences -= uM.hp / 5 + 1;
                if (!(uM.location.settlement.defences <= 0.0))
                {
                    return;
                }

                Pr_Devastation devastation = null;
                double devastationAmount = 0;
                foreach (Property pr in uM.location.properties)
                {
                    if (pr is Pr_Devastation foundDevastation)
                    {
                        devastation = foundDevastation;
                        devastationAmount += pr.charge;

                        foreach (ReasonMsg msg in pr.influences)
                            devastationAmount += msg.value;
                    }
                }

                double devastationRemaining = 300 - devastationAmount;
                if (devastationRemaining > uM.hp)
                {
                    if (devastation != null)
                    {
                        devastation.influences.Add(new ReasonMsg("Sacking Army", uM.hp));
                    }

                    if (lord != null)
                    {
                        lord.gold += (int)(Math.Round(uM.hp * sh.prosperity));
                    }
                }
                else
                {
                    if (lord != null)
                    {
                        lord.addGold((int)(Math.Round(devastationRemaining * sh.prosperity)));
                        if (sh.ruler != null)
                        {
                            lord.addGold(sh.ruler.gold);
                            sh.ruler.gold = 0;
                        }
                    }
                    sh.fallIntoRuin("Sacked by " + uM.getName());
                }

            }
            else
            {
                unit.task = null;
            }
        }


    }
}
