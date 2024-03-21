using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlphaPrefabs
{
    internal class Designator_Prefabs : Designator_Cells
    {
        public override bool Visible => AlphaPrefabs_Settings.noPrefabCatalog;

        public override int DraggableDimensions => 2;

        public Designator_Prefabs()
        {
           
            defaultLabel = "AP_OpenPrefabCatalog".Translate();
            defaultDesc = "AP_OpenPrefabCatalogDesc".Translate();
            icon = ContentFinder<Texture2D>.Get("UI/AP_OpenPrefabCatalog", true);
            soundDragSustain = SoundDefOf.Designate_DragStandard;
            soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
            useMouseIcon = true;
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            if (!c.InBounds(Map))
            {
                return false;
            }
            return true;
        }

        public override void ProcessInput(Event ev)
        {
            if (!CheckCanInteract())
            {
                return;
            }
            Window_PrefabCategories categoriesWindow = new Window_PrefabCategories(null);
            Find.WindowStack.Add(categoriesWindow);

        }

       
    }
}