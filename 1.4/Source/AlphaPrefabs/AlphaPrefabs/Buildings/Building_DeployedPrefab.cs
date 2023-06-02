using System;
using System.Collections.Generic;
using KCSG;
using RimWorld;
using Verse;
using Verse.Noise;

namespace AlphaPrefabs
{
    public class Building_DeployedPrefab : Building
    {
        public StructureLayoutDef prefab;

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
                var cellRect = CellRect.CenteredOn(this.Position, (int)prefab.Sizes.x, (int)prefab.Sizes.y);
                if (CheckNoBuildings(cellRect)) {
                    GenOption.GetAllMineableIn(cellRect, this.Map);
                    LayoutUtils.CleanRect(prefab, map, cellRect, false);
                    prefab.Generate(cellRect, map);
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

        public bool CheckNoBuildings(CellRect cellRect)
        {

            foreach(IntVec3 cell in cellRect.Cells)
            {
                if (cell.GetEdifice(this.Map)!=null && cell.GetEdifice(this.Map)?.def!=InternalDefOf.AP_DeployedPrefab )
                {
                    Messages.Message("AP_OccupiedBy".Translate(cell.GetEdifice(this.Map)?.LabelCap), cell.GetEdifice(this.Map), MessageTypeDefOf.NegativeEvent);
                    return false;

                }

            }

            return true;

        }


    }
}
