
using RimWorld;
using Verse;


namespace AlphaPrefabs
{
    [DefOf]
    public static class InternalDefOf
    {
        public static ThingDef AP_Prefab;
        public static ThingDef AP_Prefab_LowValue;
        public static ThingDef AP_Prefab_MediumHighValue;
        public static ThingDef AP_Prefab_HighValue;
        public static ThingDef AP_DeployedPrefab;
        
        public static JobDef AP_UsePrefab;

        public static SoundDef AP_BuildPrefab;
        public static SoundDef AP_DeployPrefab;

        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }
    }
}
