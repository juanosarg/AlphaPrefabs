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
    public class Window_PrefabImage : Window
    {
        public Building_Catalog building;
        public PrefabDef prefab;
        public override Vector2 InitialSize => new Vector2(620f, 500f);
        private static readonly Color borderColor = new Color(0.13f, 0.13f, 0.13f);
        private static readonly Color fillColor = new Color(0, 0, 0, 0.1f);

        public Window_PrefabImage(PrefabDef prefab, Building_Catalog building)
        {
            this.building = building;
            this.prefab = prefab;
            draggable = true;
            closeOnClickedOutside = true;
            preventCameraMotion = false;
            resizeable = true;

        }

        public void OpenIndividualPrefabWindow()
        {
            Window_Prefab specificPrefabWindow = new Window_Prefab(prefab, building);
            Find.WindowStack.Add(specificPrefabWindow);
            specificPrefabWindow.windowRect = this.windowRect;
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
                OpenIndividualPrefabWindow();
            }

            var GoBackTextRect = new Rect(40, 5, 100f, 32f);
            Widgets.Label(GoBackTextRect, "AP_GoBack".Translate());
            if (Widgets.ButtonInvisible(GoBackTextRect))
            {
                OpenIndividualPrefabWindow();
            }

            if (Widgets.ButtonImage(new Rect(outRect.xMax - 18f - 4f, 2f, 18f, 18f), TexButton.CloseXSmall))
            {
                Close();
            }


            outRect.yMin += 20f;
            var previewRect = new Rect(0f, outRect.yMin, outRect.xMax, outRect.yMax);
            GUI.DrawTexture(previewRect, ContentFinder<Texture2D>.Get(prefab.detailedImage, true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);




        }
    }
}