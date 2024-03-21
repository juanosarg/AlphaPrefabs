using RimWorld;
using UnityEngine;
using Verse;


namespace AlphaPrefabs
{



    public class AlphaPrefabs_Mod : Mod
    {


        public AlphaPrefabs_Mod(ModContentPack content) : base(content)
        {
            GetSettings<AlphaPrefabs_Settings>();
        }

        public override string SettingsCategory()
        {
            return "Alpha Prefabs";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            AlphaPrefabs_Settings.DoWindowContents(inRect);
        }
    }


}
