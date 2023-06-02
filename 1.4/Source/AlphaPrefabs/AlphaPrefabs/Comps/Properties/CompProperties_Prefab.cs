using RimWorld;
using Verse;
using System.Collections.Generic;
using KCSG;

namespace AlphaPrefabs
{
    public class CompProperties_Prefab : CompProperties
    {
        public StructureLayoutDef prefab;

        public CompProperties_Prefab()
        {
            compClass = typeof(CompPrefab);
        }
    }
}
