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
                Window_PrefabsListing prefabWindow = new Window_PrefabsListing(prefab.category, building);
                Find.WindowStack.Add(prefabWindow);
                Close();
            }

            Widgets.Label(new Rect(40, 5, 300f, 32f), "AP_GoBack".Translate());
            outRect.yMin += 20f;

            var orderNowRect = new Rect(228f, 350f, 128f, 64f);
            GUI.DrawTexture(orderNowRect, ContentFinder<Texture2D>.Get("UI/AP_OrderNow", true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);
            if (Widgets.ButtonInvisible(orderNowRect))
            {
                ThingDef newThingDef = InternalDefOf.AP_Prefab;
                Thing newPrefab = ThingMaker.MakeThing(newThingDef);
                Thing_Prefab prefabItem = newPrefab as Thing_Prefab;
                prefabItem.prefab = prefab;
                prefabItem.newLabel = prefab.LabelCap;

                DropPodUtility.DropThingsNear(building.Position, building.Map, new List<Thing>() { newPrefab }, 110,false,false,false,false);
                Close();
            }

        }
    }
}