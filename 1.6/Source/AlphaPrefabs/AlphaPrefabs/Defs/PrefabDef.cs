using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCSG;
using RimWorld;
using Verse;

namespace AlphaPrefabs
{
    public class PrefabDef : Def
    {
       
        public string detailedImage;
        public string labelForDefaultVariation;
        public string shortLabel;
        public string author;
        public float priority;
        public PrefabCategoryDef category;
        public List<PrefabCategoryDef> categories;
        public KCSG.StructureLayoutDef layout;
        public float marketvalue;
        public List<ResearchProjectDef> researchPrerequisites;
        public List<string> modPrerequisites;
        public List<string> suggestedMods;
        public List<LayoutVariationsWithName> variations;
    }
}