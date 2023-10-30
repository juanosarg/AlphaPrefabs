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
        public Graphic cachedGraphic;


        public override void PostMake()
        {
            base.PostMake();

            Utils.StoreAllactiveMods();

            prefab = (from x in DefDatabase<PrefabDef>.AllDefsListForReading
                      where CheckModsAndResearch(x) && x.marketvalue > MarketvalueCutOff(this.def).Item1 && x.marketvalue <= MarketvalueCutOff(this.def).Item2
                      && (x.category?.silly == false || (x.category?.silly == true && !AlphaPrefabs_Settings.hideSillyCategory))
                      select x).ToList().RandomElement();

            if(prefab == null ) {
                prefab = InternalDefOf.AP_Prefab_Bedroom_1;
            }

            newLabel = def.label + ": " + prefab.LabelCap;
         
        }

        public Tuple<int, int> MarketvalueCutOff(ThingDef thingdef) { 
        
            if(thingdef == InternalDefOf.AP_Prefab_LowValue)
            {
                return new Tuple<int, int>(0,900);
            }
            if (thingdef == InternalDefOf.AP_Prefab)
            {
                return new Tuple<int, int>(900, 1500);
            }
            if (thingdef == InternalDefOf.AP_Prefab_MediumHighValue)
            {
                return new Tuple<int, int>(1500, 3000);
            }

            return  new Tuple<int, int>(3000, int.MaxValue); 

        }

        public bool CheckModsAndResearch(PrefabDef prefab)
        {
            //Check research
            if (!AlphaPrefabs_Settings.noResearchLockingMode && !prefab.researchPrerequisites.NullOrEmpty())
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

        public override Graphic Graphic
        {
            get
            {
                if(cachedGraphic == null) {

                    if (prefab?.marketvalue < 900)
                    {
                        return GraphicsCache.graphicLowValue;
                    }
                    else if (prefab?.marketvalue >= 900 && prefab?.marketvalue < 1500)
                    {
                        return GraphicsCache.graphicMediumValue;
                    }
                    else if (prefab?.marketvalue >= 1500 && prefab?.marketvalue < 3000)
                    {
                        return GraphicsCache.graphicMediumHighValue;
                    }
                    else
                    {
                        return GraphicsCache.graphicHighValue;
                    }

                }else return cachedGraphic;


                

            }
        
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