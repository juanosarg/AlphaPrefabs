using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using Verse.Sound;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using static HarmonyLib.Code;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

namespace AlphaPrefabs
{
    public class JobDriver_UsePrefab : JobDriver
    {


        private Thing Item
        {
            get
            {
                return this.job.GetTarget(TargetIndex.B).Thing;
            }
        }

        private IntVec3 TargetPosition
        {
            get
            {
                return this.job.GetTarget(TargetIndex.A).Cell;
            }
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Map map = Item.Map;
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            Toil toil = Toils_General.Wait(600, TargetIndex.None);
            toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            toil.FailOnDespawnedOrNull(TargetIndex.A);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            yield return toil;
            Toil createPrefab = new Toil();
            createPrefab.initAction = delegate ()
            {
                Thing_Prefab prefabItem = Item as Thing_Prefab;

                InternalDefOf.AP_DeployPrefab.PlayOneShot(new TargetInfo(TargetA.Cell, Map, false));
                ThingDef newThing = InternalDefOf.AP_DeployedPrefab;
                Thing prefabBuilder = GenSpawn.Spawn(newThing, TargetPosition, map, WipeMode.Vanish);
                prefabBuilder.SetFaction(Faction.OfPlayer);
                Building_DeployedPrefab prefabBuilderWithClass = prefabBuilder as Building_DeployedPrefab;
                prefabBuilderWithClass.prefab = prefabItem.prefab;
                prefabBuilderWithClass.newLabel = prefabItem.newLabel;

                Item.Destroy();
            };
            yield return createPrefab;
            yield break;
        }

    }
}



