﻿using AlphaPrefabs;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;


namespace AlphaPrefabs
{
    public class CompTargetable_Ground : CompTargetable
    {

        private LocalTargetInfo target;

        protected override bool PlayerChoosesTarget
        {
            get
            {
                return true;
            }
        }

        protected override TargetingParameters GetTargetingParameters()
        {
            return new TargetingParameters
            {
                canTargetLocations = true
            };
        }

        public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
        {
            yield return targetChosenByPlayer;
            yield break;
        }

        public override bool SelectedUseOption(Pawn p)
        {

            Find.Targeter.BeginTargeting(this.GetTargetingParameters(), delegate (LocalTargetInfo t)
            {
                this.target = t;
                this.parent.GetComp<CompUsable>().TryStartUseJob(p, this.target);

            }, p, null, null);
            return true;


        }

        public override void DoEffect(Pawn user)
        {

            if (this.target.Cell.GetTerrain(user.Map).passability != Traversability.Impassable)
            {
                Job job = JobMaker.MakeJob(InternalDefOf.AP_UsePrefab, this.target, this.parent);
                job.count = 1;
                user.jobs.TryTakeOrderedJob(job, JobTag.Misc);

            }
            else
            {
                Messages.Message("AP_ImpassableTerrainDeploy".Translate(this.target.Cell.GetTerrain(user.Map).LabelCap), new LookTargets(target.Cell.ToVector3().ToIntVec3(), user.Map), MessageTypeDefOf.NegativeEvent);

            }


        }
    }
}
