
using RimWorld;
using Verse;


namespace AlphaPrefabs
{
    [DefOf]
    public static class InternalDefOf
    {
        public static ThingDef AP_Prefab;
        public static ThingDef AP_DeployedPrefab;
        public static JobDef AP_UsePrefab;

        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }
    }
}
