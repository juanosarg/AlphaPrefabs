using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;
using static System.Collections.Specialized.BitVector32;

namespace AlphaPrefabs
{
    public class Window_Prefab : Window
    {
        public Building_Catalog building;
        public PrefabDef prefab;
        public override Vector2 InitialSize => new Vector2(620f, 500f);
        private Vector2 scrollPosition = new Vector2(0, 0);
        public int columnCount = 4;
        private static readonly Color borderColor = new Color(0.13f, 0.13f, 0.13f);
        private static readonly Color fillColor = new Color(0, 0, 0, 0.1f);

        public Window_Prefab(PrefabDef prefab, Building_Catalog building)
        {
            this.building = building;
            this.prefab = prefab;
           
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
                Window_PrefabsListing prefabWindow = new Window_PrefabsListing(prefab.category, building);
                Find.WindowStack.Add(prefabWindow);
                Close();
            }

            Widgets.Label(new Rect(40, 5, 300f, 32f), "AP_GoBack".Translate());

            if (Widgets.ButtonImage(new Rect(outRect.xMax - 18f - 4f, 0f, 18f, 18f), TexButton.CloseXSmall))
            {
                Close();
            }


            outRect.yMin += 20f;

            Rect rectIcon = new Rect(0f, outRect.yMin+12, 128, 128);

            Widgets.DrawBoxSolidWithOutline(rectIcon, fillColor, borderColor, 2);
            Rect rectIconInside = rectIcon.ContractedBy(2);
            GUI.DrawTexture(rectIconInside, ContentFinder<Texture2D>.Get(prefab.icon, true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);

            Text.Font = GameFont.Medium;
            Rect rectLabel = new Rect(150, outRect.yMin + 12, 400, 40);
            Widgets.Label(rectLabel, prefab.LabelCap);

            Text.Font = GameFont.Small;
            Rect rectDescription = new Rect(150, outRect.yMin + 50, 400, 100);
            Widgets.Label(rectDescription, prefab.description);

            Rect rectResearchNeeded = new Rect(0, outRect.yMin + 175, 600, 25);
            Widgets.Label(rectResearchNeeded, prefab.researchPrerequisites.ToStringSafeEnumerable());

            Rect modsNeeded = new Rect(0, outRect.yMin + 200, 600, 25);
            Widgets.Label(modsNeeded, prefab.modPrerequisites.ToStringSafeEnumerable());

            Rect totalCost = new Rect(0, outRect.yMin + 225, 600, 25);
            Widgets.Label(totalCost, prefab.marketvalue+" "+ "AP_Silver".Translate());


            Text.Font = GameFont.Small;
            if (Widgets.ButtonText(new Rect(outRect.width / 2f - CloseButSize.x / 2f, outRect.height+30, CloseButSize.x, CloseButSize.y), "AP_OrderNow".Translate()))
            {
                ThingDef newThingDef = InternalDefOf.AP_Prefab;
                Thing newPrefab = ThingMaker.MakeThing(newThingDef);
                Thing_Prefab prefabItem = newPrefab as Thing_Prefab;
                prefabItem.prefab = prefab;
                prefabItem.newLabel = prefab.LabelCap;

                DropPodUtility.DropThingsNear(building.Position, building.Map, new List<Thing>() { newPrefab }, 110, false, false, false, false);
                Close();
            }



        }
    }
}