using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_BolsteredCurrents : Trait
    {

        public static double delta = 2;

        public override string getName()
        {
            return "Bolstered Currents";
        }

        public override string getDesc()
        {
            return "Wind Currents at this person's location generate an additional " + delta + "% Fey Presence per turn, even if the Painter of Winds is inactive.";
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            foreach (Property pr in p.getLocation().properties)
            {
                if (pr is Pr_Season_WindCurrent current)
                {
                    foreach (Property pr3 in p.getLocation().properties)
                    {
                        if (pr3 is Pr_FeyPresence)
                            pr3.influences.Add(new ReasonMsg("Bolstered Currents", delta));
                    }

                    foreach (Location l in current.downwind)
                    {
                        bool foundPresence = false;
                        foreach (Property pr2 in l.properties)
                        {
                            if (pr2 is Pr_FeyPresence)
                            {
                                foundPresence = true;
                                pr2.influences.Add(new ReasonMsg("Bolstered Currents", delta));
                            }
                        }
                        if (!foundPresence)
                        {
                            Pr_FeyPresence presence = new Pr_FeyPresence(l);
                            presence.charge = delta;
                            l.properties.Add(presence);
                        }
                    }
                }
            }
        }

    }
}
