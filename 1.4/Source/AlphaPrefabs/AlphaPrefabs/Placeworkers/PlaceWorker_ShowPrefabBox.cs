using System;
using UnityEngine;
using Verse;
using RimWorld;
using System.Linq;

namespace AlphaPrefabs
{
    public class PlaceWorker_ShowPrefabBox : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            Building_DeployedPrefab prefabWithClass = thing as Building_DeployedPrefab;
            IntVec2 size = prefabWithClass.prefab.layout.Sizes;
            var cellRect = CellRect.CenteredOn(thing.Position, (int)size.x, (int)size.z);
            GenDraw.DrawFieldEdges(cellRect.ToList());
        }
    }
}