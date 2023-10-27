using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlphaPrefabs
{
    public class Window_PrefabCategories : Window
    {

        public Building_Catalog building;
        public override Vector2 InitialSize => new Vector2(620f, 500f);
        private Vector2 scrollPosition = new Vector2(0, 0);
        public int columnCount = 4;
        private static readonly Color borderColor = new Color(0.13f, 0.13f, 0.13f);
        private static readonly Color fillColor = new Color(0, 0, 0, 0.1f);

        public Window_PrefabCategories(Building_Catalog building)
        {
            this.building = building;
            Utils.StoreAllactiveMods();
            closeOnClickedOutside = true;
            draggable = true;
            resizeable = true;
            preventCameraMotion = false;


        }

        public void OpenPrefabListWindow(PrefabCategoryDef category)
        {
            Window_PrefabsListing prefabWindow = new Window_PrefabsListing(category, building);
            
            Find.WindowStack.Add(prefabWindow);
            prefabWindow.windowRect = this.windowRect;
          
            Close();
        }

        public override void DoWindowContents(Rect inRect)
        {
            
            var outRect = new Rect(inRect);
            outRect.yMin += 40f;
            outRect.yMax -= 40f;
            outRect.width -= 16f;

            Text.Font = GameFont.Medium;
            var IntroLabel = new Rect(0, 0, 300, 32f);
            Widgets.Label(IntroLabel, "AP_ChoosePrefabCategory".Translate());
            Text.Font = GameFont.Small;
            if (Widgets.ButtonImage(new Rect(outRect.xMax - 18f - 4f, 2f, 18f, 18f), TexButton.CloseXSmall))
            {
                Close();
            }

            List<PrefabCategoryDef> prefabCategories = (from x in DefDatabase<PrefabCategoryDef>.AllDefsListForReading where 
                                                        (x.modPrerequisites.NullOrEmpty() || (x.modPrerequisites != null && Utils.ContainsAllItems(Utils.allActiveModIds, x.modPrerequisites)))
                                                        select x).OrderBy(x => x.priority).ToList();

           
            var viewRect = new Rect(0f, 0f, outRect.width - 16f, 180*(float)prefabCategories.Count/4);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            try
            {
              
                for (var i = 0; i < prefabCategories.Count; i++)
                {

                    Rect rectIcon = new Rect((128 * (i % columnCount)) + 10 * (i % columnCount), viewRect.y  + (128 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 128, 128);
                    Widgets.DrawBoxSolidWithOutline(rectIcon, fillColor, borderColor, 2);
                    Rect rectIconInside = rectIcon.ContractedBy(2);
                    GUI.DrawTexture(rectIconInside, ContentFinder<Texture2D>.Get(prefabCategories[i].icon, true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);
                    TooltipHandler.TipRegion(rectIcon, prefabCategories[i].LabelCap+": "+ prefabCategories[i].description);

                    var categoryTextRect = new Rect((128 * (i % columnCount)) + 10 * (i % columnCount), viewRect.y + 128 + (128 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 128, 20);
                    Widgets.Label(categoryTextRect, prefabCategories[i].LabelCap);

                    if (Widgets.ButtonInvisible(rectIcon))
                    {
                        OpenPrefabListWindow(prefabCategories[i]);
                       
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