using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_VerdantHerbalist : Trait
    {

        public static double amountToReduce = 1;

        public override string getName()
        {
            return "Verdant Herbalist";
        }

        public override string getDesc()
        {
            return "Any Death, Hunger, Unrest or Devastation in this location is reduced by " + amountToReduce + "% per turn, and an equal amount of Madness and Fey Presence is added. When the season changes, they will turn into a hero outside your control.";
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);

            person.increasePreference(Tags.MADNESS);
            person.increasePreference(person.map.soc_dark.index + 20000);
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            double amountToAdd = 0;
            Location l = p.getLocation();
            if (l != null)
            {
                foreach (Property pr in l.properties)
                {
                    if (pr is Pr_Death || pr is Pr_Unrest || pr is Pr_Plague || pr is Pr_Devastation)
                    {
                        pr.influences.Add(new ReasonMsg("Verdant Herbalist", -amountToReduce));
                        amountToAdd += amountToReduce;
                    }
                }

                if (amountToAdd > 0)
                {
                    bool foundMadness = false;
                    bool foundPresence = false;
                    foreach (Property pr in l.properties)
                    {
                        if (pr is Pr_FeyPresence)
                        {
                            if (foundPresence == false)
                            {
                                foundPresence = true;
                                pr.influences.Add(new ReasonMsg("Verdant Herbalist", amountToAdd));
                            }
                        }
                        else if (pr is Pr_Madness)
                        {
                            if (foundMadness == false)
                            {
                                foundMadness = true;
                                pr.influences.Add(new ReasonMsg("Verdant Herbalist", amountToAdd));
                            }
                        }
                    }

                    if (!foundMadness)
                    {
                        Pr_Madness madness = new Pr_Madness(l);
                        madness.charge = amountToAdd;
                        l.properties.Add(madness);
                    }
                    if (!foundPresence)
                    {
                        Pr_FeyPresence presence = new Pr_FeyPresence(l);
                        presence.charge = amountToAdd;
                        l.properties.Add(presence);
                    }


                }
            }
        }



    }
}
