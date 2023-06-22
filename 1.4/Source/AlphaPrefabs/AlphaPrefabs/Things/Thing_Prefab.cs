using System;
using UnityEngine;
using Verse;
using RimWorld;
using System.Linq;
using KCSG;

namespace AlphaPrefabs
{
    public class Thing_Prefab : ThingWithComps
    {
        public StructureLayoutDef prefab;
        public string newLabel;
        string cachedLabel = "";
     
        public override string Label {
            get
            {
                if (cachedLabel.NullOrEmpty())
                {
                    cachedLabel = def.label +": "+ newLabel;
                }
                return cachedLabel;
            }

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref prefab, "prefab");
            Scribe_Values.Look(ref newLabel, "newLabel");

        }

      


        public override string GetInspectString()
        {
            return base.GetInspectString()+"AP_NeedsDeployment".Translate()+"\n"+"AP_WillTurnInto".Translate(newLabel);
        }
    }
}