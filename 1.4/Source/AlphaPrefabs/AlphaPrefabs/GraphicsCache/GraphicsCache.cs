using System;
using UnityEngine;
using Verse;

namespace AlphaPrefabs
{
    [StaticConstructorOnStartup]
    public static class GraphicsCache
    {



        public static readonly Graphic graphicOrb = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Building/AP_DeployedPrefabOrb", ShaderDatabase.Cutout, Vector2.one*3, Color.white);

        public static readonly Graphic graphicLowValue = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Item/AP_PrefabItem_LowValue", ShaderDatabase.Cutout, Vector2.one, Color.white);
        public static readonly Graphic graphicMediumHighValue = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Item/AP_PrefabItem_MediumHighValue", ShaderDatabase.Cutout, Vector2.one, Color.white);
        public static readonly Graphic graphicHighValue = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Item/AP_PrefabItem_HighValue", ShaderDatabase.Cutout, Vector2.one, Color.white);

        public static readonly Graphic graphicLowValueBuilding = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Building/AP_DeployedPrefab_LowValue", ShaderDatabase.Cutout, Vector2.one*2, Color.white);
        public static readonly Graphic graphicMediumHighValueBuilding = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Building/AP_DeployedPrefab_MediumHighValue", ShaderDatabase.Cutout, Vector2.one * 2, Color.white);
        public static readonly Graphic graphicHighValueBuilding = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Building/AP_DeployedPrefab_HighValue", ShaderDatabase.Cutout, Vector2.one * 2, Color.white);


    }
}
