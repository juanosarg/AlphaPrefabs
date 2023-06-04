using System;
using UnityEngine;
using Verse;
using RimWorld;
using System.Linq;

namespace AlphaPrefabs
{
    public class PlaceWorker_ShowPrefabBoxAsItem : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            CompPrefab comp = thing.TryGetComp<CompPrefab>();
            IntVec2 size = comp.Props.prefab.Sizes;
            var cellRect = CellRect.CenteredOn(thing.Position, (int)size.x, (int)size.z);
            GenDraw.DrawFieldEdges(cellRect.ToList());
        }
    }
}