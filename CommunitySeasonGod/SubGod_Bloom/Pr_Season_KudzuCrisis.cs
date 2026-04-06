using Assets.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_KudzuCrisis : Property
    {
        public bool isCapital = false;
        public bool exploitationOutlawed = false;
        public Pr_Season_KudzuCrisis parent = null;

        public Act_Season_BanKudzu act_ban;
        public Act_Season_KudzuQuarantine act_quarantine;
        public Act_Season_RazeKudzu act_raze;


        public double exploitIntensity = 0;

        public Pr_Season_KudzuCrisis(bool isCapital, Location loc, Pr_Season_KudzuCrisis parent = null) : base(loc)
        {
            this.isCapital = isCapital;

            act_ban = new Act_Season_BanKudzu(loc,this);
            act_quarantine = new Act_Season_KudzuQuarantine(loc);
            act_raze = new Act_Season_RazeKudzu(loc);
        }

        public override string getName()
        {
            return "Kudzu Crisis";
        }

        public override string getDesc()
        {
            return "Dreaming Kudzu has spread within this nation's borders. Rulers may dispatch their armies to cut it back, issue quarantines to limit its spread, or exploit the otherworldly dreams to pacify their people and manipulate their vassals.";
        }

        public override Sprite getSprite(World world)
        {
            return map.world.iconStore.badDiplomacy;
        }

        public override bool removedOnRuin()
        {
            return true;
        }

        public override bool survivesRuin()
        {
            return false;
        }

        public override List<Assets.Code.Action> getActions()
        {
            List<Assets.Code.Action> actions = new List<Assets.Code.Action>();

            if (location.settlement is SettlementHuman sh)
            {
                if (sh.supportedMilitary != null)
                    actions.Add(act_raze);

                if (location.soc.getCapitalHex() == location.hex)
                {
                    actions.Add(act_quarantine);
                    if (exploitationOutlawed == false)
                        actions.Add(act_ban);
                }
            }


            return actions;

        }
        public void populate()
        {
            bool kudzuPresent = false;

            foreach (Location l in location.soc.lastTurnLocs)
            {
                if (l.soc == location.soc)
                {
                    foreach (Property pr in l.properties)
                    {
                        if (pr is Pr_Season_DreamingKudzu)
                        {
                            kudzuPresent = true;
                            break;
                        }
                    }
                }
                if (kudzuPresent)
                    break;
            }

            if (kudzuPresent)
            {
                foreach (Location l in location.soc.lastTurnLocs)
                {
                    if (l.soc == location.soc)
                    {
                        bool hasCrisis = false;
                        foreach (Property pr in l.properties)
                        {
                            if (pr is Pr_Season_KudzuCrisis)
                            {
                                hasCrisis = true;
                                break;
                            }
                        }
                        if (!hasCrisis)
                        {
                            Pr_Season_KudzuCrisis newCrisis = new Pr_Season_KudzuCrisis(false, l, this);
                            l.properties.Add(newCrisis);
                        }
                    }
                }
            }
            else
            {
                foreach (Location l in location.soc.lastTurnLocs)
                {
                    if (l.soc == location.soc)
                    {
                        for (int i = 0; i < l.properties.Count; i++)
                        {
                            if (l.properties[i] is Pr_Season_KudzuCrisis)
                            {
                                l.properties.RemoveAt(i);
                                i--;
                            }
                        }

                    }
                }
            }

        }

        public override void turnTick()
        {
            base.turnTick();

            if (location.settlement is SettlementHuman == false)
            {
                location.properties.Remove(this);
                return;
            }

            if (location.soc.getCapitalHex() != null)
            {
                if (location.soc.getCapitalHex() == location.hex)
                {
                    isCapital = true;
                    populate();
                }
                else
                {
                    isCapital = false;
                    if (location.soc.getCapitalHex().location.soc == location.soc)
                    {
                        foreach (Property pr in location.soc.getCapitalHex().location.properties)
                        {
                            if (pr is Pr_Season_KudzuCrisis crisis)
                            {
                                crisis.isCapital = true;
                                parent = crisis;
                                return;
                            }
                        }

                        location.soc.getCapitalHex().location.properties.Add(new Pr_Season_KudzuCrisis(true, location.soc.getCapitalHex().location));
                    }
                }
            }
        }


    }
}
