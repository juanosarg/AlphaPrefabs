
using RimWorld;
using UnityEngine;
using Verse;
namespace AlphaPrefabs
{
    public class StatPart_Prefab : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            Thing_Prefab prefab;
            if (req.HasThing && (prefab = (req.Thing as Thing_Prefab)) != null)
            {
                val = prefab.prefab.marketvalue;
                
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            Thing_Prefab prefab;
            if (req.HasThing && (prefab = (req.Thing as Thing_Prefab)) != null)
            {
                return "AP_PrefabFixedPrice".Translate();
            }
            return null;
        }

    }
}