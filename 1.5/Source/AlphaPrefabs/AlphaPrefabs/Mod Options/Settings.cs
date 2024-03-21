using RimWorld;
using System;
using UnityEngine;
using Verse;


namespace AlphaPrefabs
{


    public class AlphaPrefabs_Settings : ModSettings

    {


        public static bool devMode = false;
        public static bool noPrefabCatalog = false;
        public static bool noSilverMode = false;
        public static bool noResearchLockingMode = false;
        public static bool noModLockingMode = false;
        public static bool hideSillyCategory = false;
        public static float costMultiplier = baseCostMultiplier;
        public const float baseCostMultiplier = 0.6f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref devMode, "devMode", false);
            Scribe_Values.Look(ref noPrefabCatalog, "noPrefabCatalog", false);
            Scribe_Values.Look(ref noSilverMode, "noSilverMode", false);
            Scribe_Values.Look(ref noResearchLockingMode, "noResearchLockingMode", false);
            Scribe_Values.Look(ref noModLockingMode, "noModLockingMode", false);
            Scribe_Values.Look(ref hideSillyCategory, "hideSillyCategory", false);
            Scribe_Values.Look(ref costMultiplier, "costMultiplier", baseCostMultiplier);

        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();


            ls.Begin(inRect);

            ls.CheckboxLabeled("AP_DevMode".Translate(), ref devMode, "AP_DevModeDescription".Translate());
            ls.Gap(12f);
            ls.CheckboxLabeled("AP_NoPrefabCatalog".Translate(), ref noPrefabCatalog, "AP_NoPrefabCatalogDescription".Translate());
            ls.Gap(12f);
            ls.CheckboxLabeled("AP_NoSilverMode".Translate(), ref noSilverMode, "AP_NoSilverModeDescription".Translate());
            ls.Gap(12f);
            ls.CheckboxLabeled("AP_NoResearchLockingMode".Translate(), ref noResearchLockingMode, "AP_NoResearchLockingModeDescription".Translate());
            ls.Gap(12f);
            ls.CheckboxLabeled("AP_NoModLockingMode".Translate(), ref noModLockingMode, "AP_NoModLockingModeDescription".Translate());
            ls.Gap(12f);
            /*ls.CheckboxLabeled("AP_HideSilly".Translate(), ref hideSillyCategory, "AP_HideSillyDescription".Translate());
            ls.Gap(12f);*/
            var costLabel = ls.LabelPlusButton("AP_CostMultiplier".Translate() + ": " + costMultiplier, "AP_CostMultiplierDesc".Translate());
            costMultiplier = (float)Math.Round(ls.Slider(costMultiplier, 0.1f, 3), 2);

            if (ls.Settings_Button("AP_Reset".Translate(), new Rect(0f, costLabel.position.y + 35, 250f, 29f)))
            {
                costMultiplier = baseCostMultiplier;
            }

            ls.End();
        }



    }










}
