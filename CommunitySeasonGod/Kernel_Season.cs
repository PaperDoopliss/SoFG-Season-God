using Assets.Code;
using Assets.Code.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{

    public class Kernel_Season : ModKernel
    {

        private static Kernel_Season _kernel;
        public static Kernel_Season Instance => _kernel;

        private static CommunityLib.ModCore _comLibKernel;
        public static CommunityLib.ModCore ComLibKernel => _comLibKernel;

        public static bool opt_deckMode = true;

        public static int opt_seasonLength = 80;
        public static int opt_draftSizeNatural = 3;
        public static int opt_draftSizeSelection = 3;
        public static int opt_windEnabled = 1;
        public static int opt_harvestEnabled = 1;
        public static int opt_feastEnabled = 1;

        public bool HasHostileShift = false;

        public static int GetSubGodEnabledState(SubGod subGod)
        {
            switch(subGod)
            {
                case SubGod_Wind _:
                    return opt_windEnabled;
                case SubGod_Harvest _:
                    return opt_harvestEnabled;
                case SubGod_Feast _:
                    return opt_feastEnabled;
                default:
                    return 0;
            }
        }

        public override void onModsInitiallyLoaded()
        {
            _kernel = this;
        }

        public override void beforeMapGen(Map map)
        {
            _kernel = this;
            UAEN_Season_GreatOrc.populateGreatOrc();

            GetModKernels(map);
            new ComLibHooks(ComLibKernel, map);
            EventModifications();
        }

        public override void afterLoading(Map map)
        {
            _kernel = this;
            UAEN_Season_GreatOrc.populateGreatOrc();

            GetModKernels(map);
            new ComLibHooks(ComLibKernel, map);
            EventModifications();
        }

        private void GetModKernels(Map map)
        {
            foreach (ModKernel kernel in map.mods)
            {
                switch(kernel.GetType().Namespace)
                {
                    case "CommunityLib":
                        _comLibKernel = kernel as CommunityLib.ModCore;
                        break;
                }
            }
        }

        public override void onStartGamePresssed(Map map, List<God> gods)
        {
            Clean();

            gods.Add(new God_Season());
        }

        private void Clean()
        {
            HasHostileShift = false;
        }

        public override void receiveModConfigOpts_int(string optName, int value)
        {
            switch(optName)
            {
                case "Season Length":
                    opt_seasonLength = value;
                    break;
                case "Natural Draft Size":
                    opt_draftSizeNatural = value;
                    break;
                case "Selection Draft Size":
                    opt_draftSizeSelection = value;
                    break;
                case "Enable: Painter of Winds":
                    opt_windEnabled = value;
                    break;
                case "Enable: Uncle of the Harvest":
                    opt_harvestEnabled = value;
                    break;
                case "Enable: Lord of the Feast":
                    opt_feastEnabled = value;
                    break;
            }
        }

        public override void receiveModConfigOpts_bool(string optName, bool value)
        {
            switch(optName)
            {
                case "Deck of Seasons":
                    opt_deckMode=value;
                    break;
                case "Test Mode":
                    opt_deckMode = value;
                    break;
            }
        }

        public void EventModifications()
        {
            Dictionary<string, EventRuntime.Field> fields = EventRuntime.fields;
            Dictionary<string, EventRuntime.Property> properties = EventRuntime.properties;

            if (!properties.ContainsKey("CHOOSE_SEASON_SUBGOD"))
            {
                properties.Add("CHOOSE_SEASON_SUBGOD", new EventRuntime.TypedProperty<int>(delegate (EventContext c, int v)
                {
                    if (World.staticMap?.overmind.god is God_Season seasonGod)
                    {
                        seasonGod.PresentDraft();
                    }
                }));
            }
        }

        public override void onCheatEntered(string command)
        {
            if (command != "fey")
            {
                return;
            }

            if (GraphicalMap.selectedHex != null && GraphicalMap.selectedHex.location != null)
            {
                Pr_FeyPresence feyPresence = (Pr_FeyPresence)GraphicalMap.selectedHex.location.properties.FirstOrDefault(pr => pr is Pr_FeyPresence);
                if (feyPresence != null)
                {
                    feyPresence.charge += 75;
                    if (feyPresence.charge > 300.0)
                    {
                        feyPresence.charge = 300.0;
                    }
                    return;
                }
                feyPresence = new Pr_FeyPresence(GraphicalMap.selectedHex.location);
                feyPresence.charge = 75.0;
                GraphicalMap.selectedHex.location.properties.Add(feyPresence);
            }
        }

        public override double sovereignAI(Map map, AN actionNational, Person ruler, List<ReasonMsg> reasons, double initialUtility)
        {
            double result = base.sovereignAI(map, actionNational, ruler, reasons, initialUtility);

            if (actionNational is AN_FormAlliance)
            {
                if (ruler != null)
                {
                    foreach (Trait t in ruler.traits)
                    {
                        if (t is T_Season_Feyblood)
                        {
                            reasons?.Add(new ReasonMsg("Feyblood Don't Ally", -100));
                            result -= 100;
                        }
                    }
                }
            }
            else //Merging a bunch of these together to represent the various war declarations
            {

                Society targetSoc = null;

                if (actionNational is AN_WarCrusade crusade)
                    targetSoc = crusade.target as Society;
                else if (actionNational is AN_DeclareWar war)
                    targetSoc = war.target as Society;
                else if (actionNational is AN_WarOnThreat threat)
                    targetSoc = threat.target as Society;

                if (targetSoc != null)
                {

                    if (ruler.society.isAlliance)
                    {

                        if (targetSoc.getSovreign() != null)
                        {
                            foreach (Trait t in targetSoc.getSovreign().traits)
                            {
                                if (t is T_Season_Feyblood)
                                {
                                    reasons?.Add(new ReasonMsg("Purge the Feyblood", 50));
                                    result += 50;
                                }
                            }
                        }

                    }
                    foreach (Trait t in ruler.traits)
                    {
                        if (t is T_Season_Feyblood)
                        {

                            double spoils = 0;

                            foreach (Location l in targetSoc.lastTurnLocs)
                            {
                                foreach (Property pr in l.properties)
                                {
                                    if (pr is Pr_Season_SpoilsOfWar)
                                        spoils += pr.charge;
                                }
                            }


                            if (spoils > 0)
                            {
                                reasons?.Add(new ReasonMsg("Spoils of War", spoils * 1.5));
                                result += spoils * 1.5;
                            }


                        }
                    }

                }

            }
            return result;
        }

        public override void populatingThreats(Overmind overmind, List<MsgEvent> threats)
        {
            if (overmind.god is God_Season season)
            {
                threats.Add(new MsgEvent($"Season will change in {season.TurnsRemainingInSeason} {(season.TurnsRemainingInSeason == 0 ? "turn." : "turns.")}", 0.5, true, season.ElderTombLocation.hex));
            }
        }

        public override void onGraphicalHexUpdated(GraphicalHex graphicalHex)
        {
            if (!HasHostileShift)
            {
                return;
            }

            if (graphicalHex == null || !(graphicalHex.map.world.selector is Sel_CastPower castSelector) || !(castSelector.power is P_HostileShift hostileShift))
            {
                return;
            }

            if (!castSelector.canTarget(graphicalHex.hex))
            {
                graphicalHex.modifierStrength?.gameObject.SetActive(false);
                return;
            }

            Pr_FeyPresence feyPresence = (Pr_FeyPresence)graphicalHex.hex.location.properties.FirstOrDefault(pr => pr is Pr_FeyPresence);
            if (feyPresence == null)
            {
                graphicalHex.modifierStrength?.gameObject.SetActive(false);
                return;
            }

            if (graphicalHex.modifierStrength == null)
            {
                graphicalHex.modifierStrength = graphicalHex.world.prefabStore.getModifierStrength(graphicalHex.hex.location.getName(true), Color.white);
                graphicalHex.modifierStrength.gameObject.transform.SetParent(graphicalHex.transform);
                graphicalHex.modifierStrength.gameObject.transform.localPosition = new Vector3(0f, 0f, -3.02f);
                graphicalHex.modifierStrength.gameObject.transform.localScale = new Vector3(0.015f, 0.015f, 1f);
            }
            else
            {
                graphicalHex.modifierStrength.gameObject.SetActive(true);
                graphicalHex.modifierStrength.gameObject.transform.localScale = new Vector3(0.015f, 0.015f, 1f);
            }

            graphicalHex.modifierStrength.words.text = hostileShift.GetCost(graphicalHex.hex.location).ToString();
        }

        public override float hexHabitability(Hex hex, float hab)
        {
            foreach (Property pr in hex.location.properties)
            {
                if (pr is Pr_Season_IndustriousNewcomers)
                    return hab + ((float)(pr.charge / pr.map.param.city_popMaxPerHabilitability) + 0.005f);
            }
            return base.hexHabitability(hex, hab);
        }
    }
}
