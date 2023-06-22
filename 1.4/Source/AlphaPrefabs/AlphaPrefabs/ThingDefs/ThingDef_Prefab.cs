using System;
using UnityEngine;
using Verse;
using RimWorld;
using System.Linq;

namespace AlphaPrefabs
{
    public class Thing_Prefab : ThingWithComps
    {

        string cachedLabel = "";

        public override string Label {
            get
            {
                if (cachedLabel.NullOrEmpty())
                {
                    cachedLabel = def.label +": "+ this.TryGetComp<CompPrefab>()?.Props.newLabel;
                }
                return cachedLabel;
            }

        }

        public override string GetInspectString()
        {
            return base.GetInspectString()+"AP_NeedsDeployment".Translate();
        }
    }
}