using System;
using UnityEngine;
using Verse;
using RimWorld;
using System.Linq;
using KCSG;

namespace AlphaPrefabs
{
    public class PlaceWorker_ShowPrefabBoxAsItem : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
           
            Thing_Prefab prefabItem = thing as Thing_Prefab;

            StructureLayoutDef layoutToUse;
            if (prefabItem.variantLayout != null)
            {
                layoutToUse = prefabItem.variantLayout;
            }
            else
            {
                layoutToUse = prefabItem.prefab.layout;
            }


            IntVec2 size = layoutToUse.Sizes;
            var cellRect = CellRect.CenteredOn(thing.Position, (int)size.x, (int)size.z);
            GenDraw.DrawFieldEdges(cellRect.ToList());
        }
    }
}