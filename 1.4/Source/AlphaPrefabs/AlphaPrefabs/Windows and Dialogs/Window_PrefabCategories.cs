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
         

          
            List<PrefabCategoryDef> prefabCategories = (from x in DefDatabase<PrefabCategoryDef>.AllDefsListForReading                                           
                                            select x).OrderBy(x => x.priority).ToList();

           
            var viewRect = new Rect(0f, 0f, outRect.width - 16f, prefabCategories.Sum(opt => 50 + 17f));
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            try
            {
              
                for (var i = 0; i < prefabCategories.Count; i++)
                {

                    Rect rectIcon = new Rect((128 * (i % columnCount)) + 10 * (i % columnCount), viewRect.y + (128 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 128, 128);

                    Widgets.DrawBoxSolidWithOutline(rectIcon, fillColor, borderColor, 2);
                    Rect rectIconInside = rectIcon.ContractedBy(2);

                    GUI.DrawTexture(rectIconInside, ContentFinder<Texture2D>.Get(prefabCategories[i].icon, true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);

                    TooltipHandler.TipRegion(rectIcon, prefabCategories[i].LabelCap+": "+ prefabCategories[i].description);

                    if (Widgets.ButtonInvisible(rectIcon))
                    {
                        Window_PrefabsListing prefabWindow = new Window_PrefabsListing(prefabCategories[i],building);
                        Find.WindowStack.Add(prefabWindow);
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