using RimWorld;
using UnityEngine;
using Verse;


namespace AlphaPrefabs
{


    public class AlphaPrefabs_Settings : ModSettings

    {


        public static bool devMode = false;
        public static bool noSilverMode = false;
        public static bool noResearchLockingMode = false;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref devMode, "devMode", false);
            Scribe_Values.Look(ref noSilverMode, "noSilverMode", false);
            Scribe_Values.Look(ref noResearchLockingMode, "noResearchLockingMode", false);


        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();


            ls.Begin(inRect);

            ls.CheckboxLabeled("AP_DevMode".Translate(), ref devMode, "AP_DevModeDescription".Translate());
            ls.Gap(12f);
            ls.CheckboxLabeled("AP_NoSilverMode".Translate(), ref noSilverMode, "AP_NoSilverModeDescription".Translate());
            ls.Gap(12f);
            ls.CheckboxLabeled("AP_NoResearchLockingMode".Translate(), ref noResearchLockingMode, "AP_NoResearchLockingModeDescription".Translate());
            ls.Gap(12f);


            ls.End();
        }



    }










}
