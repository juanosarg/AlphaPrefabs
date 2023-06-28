using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse.AI.Group;
using Verse;
using RimWorld.QuestGen;
using AlphaPrefabs;
using System.Security.Cryptography;

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

                List<Thing> listThings = new List<Thing>();

                float totalValue = 0;
                foreach (IntVec3 cell in rect)
                {

                    
                    foreach (Thing thing in cell.GetThingList(Map))
                    {
                        if (!listThings.Contains(thing) && thing.MarketValue > 0)
                        {
                            listThings.Add(thing);
                            if (thing.def.Minifiable && thing.def.minifiedDef.tradeability == Tradeability.Sellable)
                            {
                                //Log.Message("Adding sellable minified " + thing.def + " of value " + thing.MarketValue);
                                
                                totalValue += thing.MarketValue;

                            }
                            
                            else
                            {
                                if (thing.def.CostList != null)
                                {
                                    float num = 0;
                                    foreach (ThingDefCountClass ingredient in thing.def.CostList)
                                    {
                                        float count = ingredient.count;
                                        num += ingredient.thingDef.BaseMarketValue * count;

                                    }
                                   
                                    //Log.Message("Adding deconstructible " + thing.def + " of value " + num);
                                    totalValue += num;
                                }
                                else if (thing.def.CostStuffCount != 0)
                                {
                                    float num = thing.Stuff.BaseMarketValue* thing.def.CostStuffCount;
                                    //Log.Message("Adding stuffed deconstructible " + thing.def + " of value " + num);
                                    totalValue += num;


                                }
                                else
                            
                                {
                                    //Log.Message("Adding plain item " + thing.def + " of value " + thing.MarketValue);
                                  
                                    totalValue += thing.MarketValue;

                                }


                            }
                        }

                        


                    }

                    TerrainDef terrain = cell.GetTerrain(Map);

                    if (terrain?.GetStatValueAbstract(StatDefOf.MarketValue) > 0)
                    {
                        //Log.Message("Adding terrain " + terrain.LabelCap + " of value " + terrain.GetStatValueAbstract(StatDefOf.MarketValue));

                        totalValue += terrain.GetStatValueAbstract(StatDefOf.MarketValue);

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
                                modsUsed.Add(thing.ContentSource.PackageId.ToLower());
                            }

                        }


                    }

                    if (cell.GetTerrain(Map)?.modContentPack?.PackageId.NullOrEmpty() == false)
                    {
                        if (cell.GetTerrain(Map).modContentPack.PackageId != "ludeon.rimworld" && !modsUsed.Contains(cell.GetTerrain(Map).modContentPack.PackageId))
                        {
                            modsUsed.Add(cell.GetTerrain(Map).modContentPack.PackageId.ToLower());
                        }

                    }

                }
                Messages.Message("AP_PrefabMods".Translate(modsUsed.ToStringSafeEnumerable().ToLower()), new LookTargets(rect.CenterCell.ToVector3().ToIntVec3(), Map), MessageTypeDefOf.NeutralEvent);
                Log.Message("AP_PrefabMods".Translate(modsUsed.ToStringSafeEnumerable().ToLower()));

            });

        }

        [DebugAction("Alpha prefabs", "Normalize qualities", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DebugNormalizeQualitiesOnRect()
        {
            DebugToolsGeneral.GenericRectTool("normalize quality", delegate (CellRect rect)
            {              
                foreach (IntVec3 cell in rect)
                {
                    foreach (Thing thing in cell.GetThingList(Map))
                    {
                        CompQuality comp = thing.TryGetComp<CompQuality>();
                        if (comp!=null)
                        {
                            comp.SetQuality(QualityCategory.Normal,ArtGenerationContext.Colony);

                        }
                    }                
                }                
            });
        }

        [DebugAction("Alpha prefabs", "Change stuff",false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static DebugActionNode DebugChangeItemStuff()
        {
            DebugActionNode debugActionNode = new DebugActionNode();
            List<ThingDef> materials = GetStuffableMaterials();

            for (int i = 0; i < materials.Count; i++)
            {
                ThingDef material = materials[i];
                if (material.IsStuff)
                {
                    debugActionNode.AddChild(new DebugActionNode(material.LabelCap.ToString(), DebugActionType.ToolMap, delegate
                    {
                        foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap))
                        {
                            thing.SetStuffDirect(material);
                          
                        }
                    }));
                }
            }
            return debugActionNode;


        }

        private static List<ThingDef> GetStuffableMaterials()
        {
            List<ThingDef> materials = new List<ThingDef>();
            foreach (ThingDef item in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (item.IsStuff)
                {
                    materials.Add(item);
                }
            }
            return materials;

        }




    }
}

