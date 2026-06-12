using System;
using HarmonyLib;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection;


namespace AlphaPrefabs
{

    [HarmonyPatch(typeof(Dialog_InfoCard), "FillCard")]
    public static class AlphaPrefabs_Dialog_InfoCard_FillCard_Patch
    {
       
        public static void Prefix(Rect cardRect, Dialog_InfoCard __instance, Thing ___thing, Dialog_InfoCard.InfoCardTab ___tab)
        {
            Thing_Prefab thing = ___thing as Thing_Prefab;
           
            if (thing != null)
            {
                if (___tab == Dialog_InfoCard.InfoCardTab.Stats)
                {
                    Texture2D texture = ContentFinder<Texture2D>.Get(thing.prefab.detailedImage, false);
                    Rect position = cardRect.AtZero();
                    position.width = 384f;
                    position.height = 576f;
                    position.x = cardRect.width * 0.75f - position.width / 2f + 18f;
                    position.y = cardRect.center.y - position.height / 2f + 120;
                    GUI.DrawTexture(position, texture, ScaleMode.ScaleToFit, true);
                }
            }
        }
    }
}