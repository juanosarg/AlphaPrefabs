using System;
using System.Collections.Generic;
using KCSG;
using RimWorld;
using UnityEngine;
using UnityEngine.UI;
using Verse;
using Verse.Noise;
using Verse.Sound;
using static UnityEngine.GraphicsBuffer;

namespace AlphaPrefabs
{
    public class Building_Catalog : Building
    {
       

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }


            Command_Action openCatalog = new Command_Action();
            openCatalog.defaultLabel = "AP_OpenPrefabCatalog".Translate();
            openCatalog.defaultDesc = "AP_OpenPrefabCatalogDesc".Translate();
            openCatalog.icon = ContentFinder<Texture2D>.Get("UI/AP_OpenPrefabCatalog", true);
            openCatalog.hotKey = KeyBindingDefOf.Misc1;
            openCatalog.action = delegate ()
            {
                Window_PrefabCategories categoriesWindow = new Window_PrefabCategories(this);
                Find.WindowStack.Add(categoriesWindow);
            };
            yield return openCatalog;

           
        }

       




    }
}
