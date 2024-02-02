using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using KCSG;
using RimWorld;
using UnityEngine;
using UnityEngine.UIElements;
using Verse;
using Verse.Noise;
using static UnityEngine.GraphicsBuffer;


namespace AlphaPrefabs
{
    public class Window_Prefab : Window
    {
        public Building_Catalog building;
        public PrefabDef prefab;
        public override Vector2 InitialSize => new Vector2(620f, 500f);   
        private static readonly Color borderColor = new Color(0.13f, 0.13f, 0.13f);
        private static readonly Color fillColor = new Color(0, 0, 0, 0.1f);

        public Window_Prefab(PrefabDef prefab, Building_Catalog building)
        {
            this.building = building;
            this.prefab = prefab;          
            closeOnClickedOutside = true;
            draggable = true;
            resizeable = true;
            preventCameraMotion = false;
        }

        public void OpenPrefabListWindow()
        {
            Window_PrefabsListing prefabWindow=null;
            if (prefab.category != null) { prefabWindow = new Window_PrefabsListing(prefab.category, building); }
            if (prefab.categories != null) { prefabWindow = new Window_PrefabsListing(prefab.categories[0], building); }
            Find.WindowStack.Add(prefabWindow);
            prefabWindow.windowRect = this.windowRect;
            Close();
        }

        public void OpenPrefabImageWindow()
        {
            Window_PrefabImage prefabImageWindow = new Window_PrefabImage(prefab, building);
            Find.WindowStack.Add(prefabImageWindow);
            prefabImageWindow.windowRect = this.windowRect;
            Close();
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            var outRect = new Rect(inRect);
            outRect.yMin += 20f;
            outRect.yMax -= 40f;
            outRect.width -= 16f;

            var arrowRect = new Rect(0f, 0f, 32f, 32f);
            GUI.DrawTexture(arrowRect, ContentFinder<Texture2D>.Get("UI/AP_GoBack", true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);
            if (Widgets.ButtonInvisible(arrowRect))
            {
                OpenPrefabListWindow();
            }

            var GoBackTextRect = new Rect(40, 5, 100f, 32f);
            Widgets.Label(GoBackTextRect, "AP_GoBack".Translate());
            if (Widgets.ButtonInvisible(GoBackTextRect))
            {
                OpenPrefabListWindow();
            }

            if (Widgets.ButtonImage(new Rect(outRect.xMax - 18f - 4f, 2f, 18f, 18f), TexButton.CloseXSmall))
            {
                Close();
            }


            outRect.yMin += 20f;

            Rect rectIcon = new Rect(0f, outRect.yMin+20, 128, 128);
            Widgets.DrawBoxSolidWithOutline(rectIcon, fillColor, borderColor, 2);
            Rect rectIconInside = rectIcon.ContractedBy(2);
            GUI.DrawTexture(rectIconInside, ContentFinder<Texture2D>.Get(prefab.detailedImage, true), ScaleMode.ScaleAndCrop, alphaBlend: true, 0f, Color.white, 0f, 0f);

            if (!prefab.detailedImage.NullOrEmpty())
            {
                Rect magGlassIcon = new Rect(rectIcon.xMax - 40, rectIcon.yMax - 40, 32, 32);
                GUI.DrawTexture(magGlassIcon, ContentFinder<Texture2D>.Get("UI/AP_MagnifyingGlass", true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);
                if (Widgets.ButtonInvisible(rectIconInside))
                {
                    OpenPrefabImageWindow();
                }

            }       

            Text.Font = GameFont.Medium;
            Rect rectLabel = new Rect(150, outRect.yMin + 12, 400, 40);
            Widgets.Label(rectLabel, prefab.LabelCap);

            Text.Font = GameFont.Small;
            Rect rectDescription = new Rect(150, outRect.yMin + 50, 400, 100);
            Widgets.Label(rectDescription, prefab.description);

            Rect rectPrefabDetails = new Rect(0, outRect.yMin + 175, 550, 300);

            StringBuilder textForDetails = new StringBuilder();
            if (!AlphaPrefabs_Settings.noResearchLockingMode)
            {
                if (!prefab.researchPrerequisites.NullOrEmpty())
                {
                    List<string> researchStrings = new List<string>();
                    foreach (ResearchProjectDef research in prefab.researchPrerequisites)
                    {
                        researchStrings.Add(research.LabelCap);
                    }
                    textForDetails.Append("AP_ResearchNeeded".Translate(researchStrings.ToStringSafeEnumerable()));



                }
                else textForDetails.Append("AP_NoResearchNeeded".Translate());
                textForDetails.AppendInNewLine(" ");
            }
            
            

            if (!prefab.modPrerequisites.NullOrEmpty())
            {
                List<string> modStrings = new List<string>();


                foreach (string mod in prefab.modPrerequisites)
                {
                    bool found = false;


                    foreach (ModMetaData item in ModsConfig.ActiveModsInLoadOrder)
                    {
                        if (item.PackageId.ToLower().Contains(mod))
                        {
                            modStrings.Add(item.Name);
                          
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        modStrings.Add("AP_ModNotFound".Translate(mod));
                      
                    }
                }
                textForDetails.Append("AP_ModsNeeded".Translate(modStrings.ToStringSafeEnumerable()));

            }
            else textForDetails.AppendInNewLine("AP_NoModsNeeded".Translate());
           


            if (!prefab.suggestedMods.NullOrEmpty())
            {
                textForDetails.AppendInNewLine("AP_SuggestedMods".Translate(prefab.suggestedMods.ToStringSafeEnumerable()));
               
            }
            textForDetails.AppendInNewLine(" ");
            textForDetails.AppendInNewLine("AP_PrefabDimensions".Translate(prefab.layout.Sizes.x, prefab.layout.Sizes.z));
         


            if (!prefab.author.NullOrEmpty())
            {

                textForDetails.AppendInNewLine("AP_Author".Translate(prefab.author));

               
            }
            if (!AlphaPrefabs_Settings.noSilverMode) {
                textForDetails.AppendInNewLine(((int)(prefab.marketvalue * AlphaPrefabs_Settings.costMultiplier)).ToString() + " " + "AP_Silver".Translate());

            }


            Widgets.Label(rectPrefabDetails, textForDetails.ToString());

            Text.Font = GameFont.Small;
            Rect oderButtonRect = new Rect(outRect.width / 2f - CloseButSize.x / 2f, outRect.height + 30, CloseButSize.x, CloseButSize.y);
            Rect exportButtonRect = new Rect(outRect.width / 2f + CloseButSize.x, outRect.height + 30, CloseButSize.x, CloseButSize.y);
            if (!prefab.variations.NullOrEmpty())
            {
                Utils.DrawButton(oderButtonRect, "AP_OrderNowWithVariation".Translate(), delegate
                {
                    var floatOptions = new List<FloatMenuOption>();
                    floatOptions.Add(new FloatMenuOption(prefab.labelForDefaultVariation.CapitalizeFirst(), delegate
                    {
                        OrderPrefab(null, "");
                    }));
                    foreach (LayoutVariationsWithName variation in prefab.variations)
                    {

                        if ((variation.modPrerequisites.NullOrEmpty() || (variation.modPrerequisites != null && Utils.ContainsAllItems(Utils.allActiveModIds, variation.modPrerequisites)))) {
                            floatOptions.Add(new FloatMenuOption(variation.name.CapitalizeFirst(), delegate
                            {
                                OrderPrefab(variation.layoutVariation, variation.name);
                            }));
                        }                      
                    }
                   
                    Find.WindowStack.Add(new FloatMenu(floatOptions));
                });

                if (BlueprintUtils.ModActive)
                    Utils.DrawButton(
                        exportButtonRect,
                        "AP_ExportWithVariation".Translate(),
                        () =>
                            Find.WindowStack.Add(
                                new FloatMenu(
                                    prefab
                                        .variations.Select(
                                            variation =>
                                                new FloatMenuOption(
                                                    variation.name.CapitalizeFirst(),
                                                    () =>
                                                        ExportPrefab(
                                                            variation.layoutVariation,
                                                            variation.name
                                                        )
                                                )
                                        )
                                        .Prepend(
                                            new FloatMenuOption(
                                                prefab.labelForDefaultVariation.CapitalizeFirst(),
                                                () => ExportPrefab()
                                            )
                                        )
                                        .ToList()
                                )
                            )
                    );
            }
            else {
                if (Widgets.ButtonText(oderButtonRect, "AP_OrderNow".Translate()))
                {
                    OrderPrefab(null,"");
                  
                }
                if (BlueprintUtils.ModActive)
                    if (Widgets.ButtonText(exportButtonRect, "AP_Export".Translate()))
                        ExportPrefab();

            }
            



        }

        public void OrderPrefab(StructureLayoutDef layoutVariation,string variationString)
        {
            string reason;
            if (CheckModsSilverAndResearch(out reason)) {
                ThingDef newThingDef = InternalDefOf.AP_Prefab;
                Thing newPrefab = ThingMaker.MakeThing(newThingDef);
                Thing_Prefab prefabItem = newPrefab as Thing_Prefab;
                prefabItem.prefab = prefab;
                prefabItem.newLabel = prefab.LabelCap;
                if (layoutVariation != null)
                {
                    prefabItem.variantLayout = layoutVariation;
                    prefabItem.variationString = variationString;

                }
                Map map = GetMap(building);
                IntVec3 position = GetPosition(building,map);
                
                if (!AlphaPrefabs_Settings.noSilverMode) {
                   
                    TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, (int)(prefab.marketvalue * AlphaPrefabs_Settings.costMultiplier), map, null);
                }
                
                DropPodUtility.DropThingsNear(position, map, new List<Thing>() { newPrefab }, 110, false, false, false, false);
                Close();
            }
            else
            {
                if (building == null)
                {
                    Messages.Message(reason, null, MessageTypeDefOf.RejectInput);
                } else
                Messages.Message(reason, building, MessageTypeDefOf.RejectInput);


            }

        }
        public void ExportPrefab(
            StructureLayoutDef layoutVariation = null,
            string variationString = null
        )
        {
            AccessTools
                .Method("Blueprints.BlueprintController:Add")
                .Invoke(
                    null,
                    new[]
                    {
                        BlueprintUtils.ToBlueprint(
                            layoutVariation ?? prefab.layout,
                            prefab.variations.NullOrEmpty()
                                ? $"{prefab.LabelCap}"
                                : $"{prefab.LabelCap} ({variationString ?? prefab.labelForDefaultVariation})"
                        )
                    }
                );
        }

        public bool CheckModsSilverAndResearch(out string reason)
        {

            // Checking research projects
            if (!AlphaPrefabs_Settings.noResearchLockingMode && !prefab.researchPrerequisites.NullOrEmpty())
            {
                foreach (ResearchProjectDef research in prefab.researchPrerequisites)
                {
                    if (!research.IsFinished)
                    {
                        reason = "AP_ResearchNotFinished".Translate(research.LabelCap);
                        return false;
                    }

                }
            }           
            // Checking mods
            if (!prefab.modPrerequisites.NullOrEmpty())
            {
                if(!Utils.ContainsAllItems(Utils.allActiveModIds, prefab.modPrerequisites)){

                    reason = "AP_NotAllModsPresent".Translate();
                    return false;
                }

            }
            // Checking money

            Map map;
            if (building != null)
            {
                map = building.Map;

            }else map = Find.CurrentMap;

            if (!AlphaPrefabs_Settings.noSilverMode && AmountSendableSilver(map) < (int)(prefab.marketvalue * AlphaPrefabs_Settings.costMultiplier))
            {              
                    reason = "AP_NotEnoughMoney".Translate((int)(prefab.marketvalue * AlphaPrefabs_Settings.costMultiplier));
                    return false;              
            }

            reason = "";
            return true;
        }

        public static int AmountSendableSilver(Map map)
        {
            return (from t in TradeUtility.AllLaunchableThingsForTrade(map)
                    where t.def == ThingDefOf.Silver
                    select t).Sum((Thing t) => t.stackCount);
        }

        public static Map GetMap(Building_Catalog building)
        {
            if (building == null)
            {
                return Find.CurrentMap;               
            }
            else
            {
                return building.Map;              
            }
        }

        public static IntVec3 GetPosition(Building_Catalog building,Map map)
        {

            List<Thing> prefabDeliverySpots = map.listerThings.ThingsOfDef(InternalDefOf.AP_PrefabDeliverySpot);
            if (prefabDeliverySpots.Count > 0)
            {
                return prefabDeliverySpots.RandomElement().Position;
            }

            if (building == null)
            {
               return Find.CameraDriver.MapPosition;
            }
            else
            {
                return building.Position;
            }
        }

    }
}