using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse.AI.Group;
using Verse;
using RimWorld.QuestGen;
using AlphaPrefabs;

namespace AlphaPrefabs
{
    public static class DebugActions
    {
        private static Map Map
        {
            get
            {
                return Find.CurrentMap;
            }
        }

        [DebugAction("Alpha prefabs", "Calculate prefab value", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DebugCalculateMarketValueOnRect()
        {
            DebugToolsGeneral.GenericRectTool("value", delegate (CellRect rect)
            {

                float totalValue = 0;
                foreach (IntVec3 cell in rect)
                {
                    foreach (Thing thing in cell.GetThingList(Map))
                    {
                        if (thing.MarketValue > 0)
                        {
                            totalValue += thing.MarketValue;
                           
                        }


                    }

                    if (cell.GetTerrain(Map)?.GetStatValueAbstract(StatDefOf.MarketValue) > 0)
                    {
                        totalValue += cell.GetTerrain(Map).GetStatValueAbstract(StatDefOf.MarketValue);

                    }

                }
                Messages.Message("AP_TotalPrefabValue".Translate(totalValue), new LookTargets(rect.CenterCell.ToVector3().ToIntVec3(), Map), MessageTypeDefOf.NeutralEvent);
                Log.Message("AP_TotalPrefabValue".Translate(totalValue));

            });

        }

        [DebugAction("Alpha prefabs", "Calculate prefab techs", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DebugCalculateTechsOnRect()
        {
            DebugToolsGeneral.GenericRectTool("techs", delegate (CellRect rect)
            {

                List<ResearchProjectDef> researchesNeeded = new List<ResearchProjectDef>();
                foreach (IntVec3 cell in rect)
                {
                    foreach (Thing thing in cell.GetThingList(Map))
                    {
                        if (thing.def?.researchPrerequisites != null)
                        {
                            foreach (ResearchProjectDef research in thing.def.researchPrerequisites)
                            {
                                if (!researchesNeeded.Contains(research))
                                {
                                    researchesNeeded.Add(research);
                                }
                            }
                        }
                        else if (thing.def?.recipeMaker?.researchPrerequisites != null)
                        {
                            foreach (ResearchProjectDef research in thing.def.recipeMaker.researchPrerequisites)
                            {
                                if (!researchesNeeded.Contains(research))
                                {
                                    researchesNeeded.Add(research);
                                }
                            }

                        }
                        else if (thing.def?.recipeMaker?.researchPrerequisite != null)
                        {
                            if (!researchesNeeded.Contains(thing.def.recipeMaker.researchPrerequisite))
                            {
                                researchesNeeded.Add(thing.def.recipeMaker.researchPrerequisite);
                            }

                        }


                    }

                    if (cell.GetTerrain(Map)?.researchPrerequisites!=null)
                    {
                        foreach (ResearchProjectDef research in cell.GetTerrain(Map).researchPrerequisites)
                        {
                            if (!researchesNeeded.Contains(research))
                            {
                                researchesNeeded.Add(research);
                            }
                        }
                    }

                }
                Messages.Message("AP_PrefabLockingResearches".Translate(researchesNeeded.ToStringSafeEnumerable()), new LookTargets(rect.CenterCell.ToVector3().ToIntVec3(), Map), MessageTypeDefOf.NeutralEvent);
                Log.Message("AP_PrefabLockingResearches".Translate(researchesNeeded.ToStringSafeEnumerable()));
            });

        }

        [DebugAction("Alpha prefabs", "Calculate prefab mods", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DebugCalculateModsOnRect()
        {
            DebugToolsGeneral.GenericRectTool("mods", delegate (CellRect rect)
            {

                List<string> modsUsed = new List<string>();
                foreach (IntVec3 cell in rect)
                {
                    foreach (Thing thing in cell.GetThingList(Map))
                    {
                        if (thing.ContentSource?.PackageId.NullOrEmpty() == false)
                        {
                            if (thing.ContentSource.PackageId != "ludeon.rimworld" && !modsUsed.Contains(thing.ContentSource.PackageId))
                            {
                                modsUsed.Add(thing.ContentSource.PackageId);
                            }

                        }


                    }

                    if (cell.GetTerrain(Map)?.modContentPack?.PackageId.NullOrEmpty() == false)
                    {
                        if (cell.GetTerrain(Map).modContentPack.PackageId != "ludeon.rimworld" && !modsUsed.Contains(cell.GetTerrain(Map).modContentPack.PackageId))
                        {
                            modsUsed.Add(cell.GetTerrain(Map).modContentPack.PackageId);
                        }

                    }

                }
                Messages.Message("AP_PrefabMods".Translate(modsUsed.ToStringSafeEnumerable()), new LookTargets(rect.CenterCell.ToVector3().ToIntVec3(), Map), MessageTypeDefOf.NeutralEvent);
                Log.Message("AP_PrefabMods".Translate(modsUsed.ToStringSafeEnumerable()));

            });

        }




    }
}

