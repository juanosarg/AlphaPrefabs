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

        public Window_PrefabsListing(PrefabCategoryDef category, Building_Catalog building)
        {
            this.building = building;
            this.category = category;
            doCloseX = true;
            doCloseButton = true;
            closeOnClickedOutside = true;

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
                Window_PrefabCategories categoriesWindow = new Window_PrefabCategories(building);
                Find.WindowStack.Add(categoriesWindow);
                Close();
            }

            Widgets.Label(new Rect(40, 5, 300f, 32f), "AP_GoBack".Translate());
            outRect.yMin += 20f;
            List<PrefabDef> prefabs = (from x in DefDatabase<PrefabDef>.AllDefsListForReading where x.category == category
                                                        select x).OrderBy(x => x.priority).ToList();


            var viewRect = new Rect(0f, 40, outRect.width - 16f, prefabs.Sum(opt => 50 + 17f));
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            try
            {

                for (var i = 0; i < prefabs.Count; i++)
                {

                    Rect rectIcon = new Rect((128 * (i % columnCount)) + 10 * (i % columnCount), viewRect.y + (128 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 128, 128);

                    Widgets.DrawBoxSolidWithOutline(rectIcon, fillColor, borderColor, 2);
                    Rect rectIconInside = rectIcon.ContractedBy(2);

                    GUI.DrawTexture(rectIconInside, ContentFinder<Texture2D>.Get(prefabs[i].icon, true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);

                    TooltipHandler.TipRegion(rectIcon, prefabs[i].LabelCap + ": " + prefabs[i].description);
                    if (Widgets.ButtonInvisible(rectIcon))
                    {
                        Window_Prefab specificPrefabWindow = new Window_Prefab(prefabs[i],building);
                        Find.WindowStack.Add(specificPrefabWindow);
                        Close();
                    }
                }
            }
            finally
            {
                Widgets.EndScrollView();
            }
        }
    }
}