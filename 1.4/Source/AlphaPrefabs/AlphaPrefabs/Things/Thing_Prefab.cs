using System;
using UnityEngine;
using Verse;
using RimWorld;
using System.Linq;
using KCSG;
using System.Collections.Generic;
using System.Text;

namespace AlphaPrefabs
{
    public class Thing_Prefab : ThingWithComps
    {
        public PrefabDef prefab = new PrefabDef();
        public StructureLayoutDef variantLayout;
        public string newLabel ="";
        string cachedLabel = "";
        public string variationString = "";


        public override void PostMake()
        {
            base.PostMake();

            Utils.StoreAllactiveMods();

            prefab = (from x in DefDatabase<PrefabDef>.AllDefsListForReading
                      where CheckModsAndResearch(x)
                      select x).ToList().RandomElement();

            newLabel = def.label + ": " + prefab.LabelCap;
         
        }

        public bool CheckModsAndResearch(PrefabDef prefab)
        {
            //Check research
            if (!prefab.researchPrerequisites.NullOrEmpty())
            {
                foreach (ResearchProjectDef research in prefab.researchPrerequisites)
                {
                    if (!research.IsFinished)
                    {                        
                        return false;
                    }

                }
            }
            //Check mods
            if (!prefab.modPrerequisites.NullOrEmpty())
            {
                Utils.StoreAllactiveMods();
                if (!Utils.ContainsAllItems(Utils.allActiveModIds, prefab.modPrerequisites))
                {
                    return false;
                }

            }
            return true;

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
            Scribe_Defs.Look(ref variantLayout, "variantLayout");
            Scribe_Values.Look(ref newLabel, "newLabel");
            Scribe_Values.Look(ref variationString, "variationString");


        }




        public override string GetInspectString()
        {
            StringBuilder sb = new StringBuilder(base.GetInspectString());
            sb.AppendLine("AP_NeedsDeployment".Translate());
            sb.AppendLine("AP_WillTurnInto".Translate(newLabel));
            if (variationString != "")
            {
                sb.AppendLine("AP_VariantLayout".Translate(variationString));
            }


            return sb.ToString().Trim();
        }
    }
}