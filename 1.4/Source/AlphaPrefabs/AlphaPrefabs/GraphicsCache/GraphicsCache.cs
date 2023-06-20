using System;
using UnityEngine;
using Verse;

namespace AlphaPrefabs
{
    [StaticConstructorOnStartup]
    public static class GraphicsCache
    {



        public static readonly Graphic graphicOrb = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Building/AP_DeployedPrefabOrb", ShaderDatabase.Cutout, Vector2.one*3, Color.white);
       

    }
}
