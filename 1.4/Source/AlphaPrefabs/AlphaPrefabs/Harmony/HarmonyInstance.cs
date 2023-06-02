using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;





namespace AlphaPrefabs
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.alphaprefabs");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
           

        }

       
       
    }

}
