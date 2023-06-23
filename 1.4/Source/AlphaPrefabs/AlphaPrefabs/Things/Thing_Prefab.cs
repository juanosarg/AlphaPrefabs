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
        public PrefabDef prefab = new PrefabDef();
        public string newLabel ="";
        string cachedLabel = "";


        public override void PostMake()
        {
            base.PostMake();
            prefab = DefDatabase<PrefabDef>.AllDefsListForReading.RandomElement();
            newLabel = def.label + ": " + prefab.LabelCap;
         
        }

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

        public override string LabelNoCount
        {
            get
            {
                if (cachedLabel.NullOrEmpty())
                {
                    cachedLabel = newLabel;
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