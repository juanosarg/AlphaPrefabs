using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HarmonyLib;
using KCSG;
using Verse;

namespace AlphaPrefabs
{
    public static class BlueprintUtils
    {
        public static bool ModActive =>
            Utils.allActiveModIds.Any(packageId => packageId.StartsWith("Defi.Blueprints"));

        public static object ToBlueprint(StructureLayoutDef structureLayoutDef, string name)
        {
            if (!Utils.allActiveModIds.Any(packageId => packageId == "Defi.Blueprints.fork"))
                name = Regex.Replace(name, @"[^A-Za-z0-9_]", "");
            var layouts = Traverse
                .Create(structureLayoutDef)
                .Field("_layouts")
                .GetValue<List<SymbolDef[,]>>();
            var sizes = structureLayoutDef.Sizes;
            var BuildableInfo = AccessTools.TypeByName("Blueprints.BuildableInfo");
            var Blueprint = AccessTools.TypeByName("Blueprints.Blueprint");
            var contents = Activator.CreateInstance(typeof(List<>).MakeGenericType(BuildableInfo));
            var addMethod = contents.GetType().GetMethod("Add");
            foreach (var layout in layouts)
                for (var i = 0; i < sizes.z; ++i)
                for (var j = 0; j < sizes.x; ++j)
                    if (layout[i, j] != null)
                    {
                        var symbolDef = layout[i, j];
                        var traverse = Traverse.Create(symbolDef);
                        var thingDef = traverse.Field("thingDef").GetValue<ThingDef>();
                        var stuffDef = traverse.Field("stuffDef").GetValue<ThingDef>();
                        var rotation = traverse.Field("rotation").GetValue<Rot4>();
                        var position = new IntVec3(j, 0, i);
                        if (thingDef == null)
                            continue;
                        if (!thingDef.BuildableByPlayer)
                            continue;
                        var thing = CreateThing(thingDef, stuffDef, position, rotation);
                        var constructor = AccessTools.Constructor(
                            BuildableInfo,
                            new[] { typeof(Thing), typeof(IntVec3) }
                        );
                        var buildableInfo = constructor.Invoke(
                            new object[] { thing, IntVec3.Zero }
                        );
                        addMethod.Invoke(contents, new[] { buildableInfo });
                    }
            var terrainGrid = Traverse
                .Create(structureLayoutDef)
                .Field("_terrainGrid")
                .GetValue<TerrainDef[,]>();
            for (var i = 0; i < sizes.z; ++i)
            for (var j = 0; j < sizes.x; ++j)
                if (terrainGrid[i, j] != null)
                {
                    var terrain = terrainGrid[i, j];
                    if (DefDatabase<TerrainDef>.GetNamed(terrain.defName) == null)
                        continue;
                    if (!terrain.BuildableByPlayer)
                        continue;
                    var constructor = AccessTools.Constructor(
                        BuildableInfo,
                        new[] { typeof(TerrainDef), typeof(IntVec3), typeof(IntVec3) }
                    );
                    var buildableInfo = constructor.Invoke(
                        new object[] { terrain, new IntVec3(j, 0, i), IntVec3.Zero }
                    );
                    addMethod.Invoke(contents, new[] { buildableInfo });
                }
            var listType = typeof(List<>);
            listType.MakeGenericType(BuildableInfo);
            var BlueprintConstructor = AccessTools.Constructor(
                Blueprint,
                new[]
                {
                    typeof(List<>).MakeGenericType(BuildableInfo),
                    typeof(IntVec2),
                    typeof(string),
                    typeof(bool)
                }
            );
            return BlueprintConstructor.Invoke(new object[] { contents, sizes, name, false });
        }

        public static Thing CreateThing(
            ThingDef thingDef,
            ThingDef stuff,
            IntVec3 position,
            Rot4 rotation
        )
        {
            var thing = new Thing { def = thingDef };
            var traverse = Traverse.Create(thing);
            traverse.Field("stuffInt").SetValue(stuff);
            traverse.Field("positionInt").SetValue(position);
            traverse.Field("rotationInt").SetValue(rotation);
            return thing;
        }
    }
}
