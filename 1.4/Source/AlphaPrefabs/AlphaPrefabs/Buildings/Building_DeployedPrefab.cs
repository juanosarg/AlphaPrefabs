using System;
using System.Collections.Generic;
using KCSG;
using RimWorld;
using UnityEngine;
using UnityEngine.UI;
using Verse;
using Verse.Noise;

namespace AlphaPrefabs
{
    public class Building_DeployedPrefab : Building
    {
        public StructureLayoutDef prefab;
        public int tickCounter;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref prefab, "prefab");
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }


            Command_Action buildPrefab = new Command_Action();
            buildPrefab.defaultLabel = "Build prefab";
            buildPrefab.action = delegate
            {
                Map map = this.Map;
                var cleanCellRect = CellRect.CenteredOn(this.Position, (int)prefab.Sizes.x, (int)prefab.Sizes.z);
                
                if (CheckNoBuildingsOrWater(cleanCellRect)) {
                    GenOption.GetAllMineableIn(cleanCellRect, map);
                    LayoutUtils.CleanRect(prefab, map, cleanCellRect, true);
                    prefab.Generate(cleanCellRect, map);
                }
                   

            };
            yield return buildPrefab;

            Command_Action undeployPrefab = new Command_Action();
            undeployPrefab.defaultLabel = "Undeploy prefab";
            undeployPrefab.action = delegate
            {
                
                ThingDef newThing = InternalDefOf.AP_Prefab;
                Thing prefab = GenSpawn.Spawn(newThing, this.Position, this.Map, WipeMode.Vanish);               
                CompPrefab comp = prefab.TryGetComp<CompPrefab>();
                comp.Props.prefab = this.prefab;
                this.DeSpawn();
            };
            yield return undeployPrefab;
        }

        public bool CheckNoBuildingsOrWater(CellRect cellRect)
        {

            foreach(IntVec3 cell in cellRect.Cells)
            {
                if (cell.GetEdifice(this.Map)!=null && cell.GetEdifice(this.Map)?.def!=InternalDefOf.AP_DeployedPrefab )
                {
                    Messages.Message("AP_OccupiedBy".Translate(cell.GetEdifice(this.Map)?.LabelCap), cell.GetEdifice(this.Map), MessageTypeDefOf.NegativeEvent);
                    return false;

                }
                TerrainDef terrain = cell.GetTerrain(this.Map);
                if (terrain.passability== Traversability.Impassable)
                {
                    Messages.Message("AP_ImpassableTerrain".Translate(terrain.LabelCap), new LookTargets(cell.ToVector3().ToIntVec3(), this.Map), MessageTypeDefOf.NegativeEvent);
                    return false;

                }

            }

            return true;

        }

        public override void Draw()
        {
            
            var vector = this.DrawPos;
            float speed = 0.1f;

            float oscillation = Mathf.Cos(Current.Game.tickManager.TicksGame*speed / Mathf.PI);

            vector.y += 6;
            vector.z += 0.8f + oscillation/4;
            GraphicsCache.graphicOrb?.DrawFromDef(vector, Rot4.North, null);
            base.Draw();
            
        }


        


    }
}
