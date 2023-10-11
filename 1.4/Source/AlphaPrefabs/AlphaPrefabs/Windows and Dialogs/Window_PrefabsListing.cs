using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using static System.Collections.Specialized.BitVector32;

namespace AlphaPrefabs
{
    public class Window_PrefabsListing : Window
    {
        public Building_Catalog building;
        public PrefabCategoryDef category;
        public override Vector2 InitialSize => new Vector2(620f, 500f);
        private Vector2 scrollPosition = new Vector2(0, 0);
        public int columnCount = 4;
        private static readonly Color borderColor = new Color(0.13f, 0.13f, 0.13f);
        private static readonly Color fillColor = new Color(0, 0, 0, 0.1f);
        private string searchKey;

        public Window_PrefabsListing(PrefabCategoryDef category, Building_Catalog building)
        {
            Utils.StoreAllactiveMods();
            this.building = building;
            this.category = category;           
            closeOnClickedOutside = true;
        }

        public void OpenCategoriesWindow()
        {
            Window_PrefabCategories categoriesWindow = new Window_PrefabCategories(building);
            Find.WindowStack.Add(categoriesWindow);
            Close();
        }

        public void OpenIndividualPrefabWindow(PrefabDef prefab)
        {
            Window_Prefab specificPrefabWindow = new Window_Prefab(prefab, building);
            Find.WindowStack.Add(specificPrefabWindow);
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
                OpenCategoriesWindow();
            }

            var GoBackTextRect = new Rect(40, 5, 100f, 32f);
            Widgets.Label(GoBackTextRect, "AP_GoBack".Translate());
            if (Widgets.ButtonInvisible(GoBackTextRect))
            {
                OpenCategoriesWindow();
            }

            var searchRect = new Rect(160, 5, 150, 24);
            searchKey = Widgets.TextField(searchRect, searchKey);
            var searchLabel = new Rect(320,5,100,32);
            Widgets.Label(searchLabel, "AP_PrefabSearch".Translate());         

            if (Widgets.ButtonImage(new Rect(outRect.xMax - 18f - 4f, 2f, 18f, 18f), TexButton.CloseXSmall))
            {
                Close();
            }

            outRect.yMin += 20f;
            List<PrefabDef> prefabs = (from x in DefDatabase<PrefabDef>.AllDefsListForReading where (x.category == category || x.categories?.Contains(category)==true) && (x.label.ToLower().Contains(searchKey.ToLower())||
                                       x.shortLabel.ToLower().Contains(searchKey.ToLower())) &&
                                       (x.modPrerequisites.NullOrEmpty() ||(x.modPrerequisites!=null && Utils.ContainsAllItems(Utils.allActiveModIds,x.modPrerequisites)))
                                       select x).OrderBy(x => x.priority).ToList();


            var viewRect = new Rect(0f, 40, outRect.width - 16f, 188 * ((prefabs.Count / columnCount)+1));
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            try
            {

                for (var i = 0; i < prefabs.Count; i++)
                {

                    Rect rectIcon = new Rect((128 * (i % columnCount)) + 10 * (i % columnCount), viewRect.y + (148 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 128, 128);

                    Widgets.DrawBoxSolidWithOutline(rectIcon, fillColor, borderColor, 2);
                    Rect rectIconInside = rectIcon.ContractedBy(2);

                    GUI.DrawTexture(rectIconInside, ContentFinder<Texture2D>.Get(prefabs[i].detailedImage, true), ScaleMode.ScaleAndCrop, alphaBlend: true, 0f, Color.white, 0f, 0f);

                    TooltipHandler.TipRegion(rectIcon, prefabs[i].LabelCap + ": " + prefabs[i].description);
                    if (Widgets.ButtonInvisible(rectIcon))
                    {
                        OpenIndividualPrefabWindow(prefabs[i]);
                    }

                    Text.Font = GameFont.Tiny;
                    var prefabTextRect = new Rect((128 * (i % columnCount)) + 10 * (i % columnCount), viewRect.y + 128 + (148 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 128, 20);
                    Widgets.Label(prefabTextRect, prefabs[i].shortLabel.CapitalizeFirst());
                    Text.Font = GameFont.Small;
                    Rect silverIcon = new Rect((128 * (i % columnCount)) + 10 * (i % columnCount), viewRect.y + 148 + (148 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 20, 20);
                    GUI.DrawTexture(silverIcon, ContentFinder<Texture2D>.Get("Things/Item/Resource/Silver/Silver_c", true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);

                    Rect silverDetails = new Rect((128 * (i % columnCount)) + 10 * (i % columnCount)+24, viewRect.y +148+ (148 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 104, 20);
                    Widgets.Label(silverDetails, ((int)(prefabs[i].marketvalue * Constants.SellPriceModifier)).ToString());


                }
            }
            finally
            {
                Widgets.EndScrollView();
            }
        }
    }
}