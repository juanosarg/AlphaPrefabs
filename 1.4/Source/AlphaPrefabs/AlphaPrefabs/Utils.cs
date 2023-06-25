using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AlphaPrefabs
{
    public static class Utils
    {

        public static List<string> allActiveModIds = new List<string>();

        public static bool ContainsAllItems(List<string> a, List<string> b)
        {
            return !b.Except(a).Any();
        }

        public static void StoreAllactiveMods()
        {
            foreach (ModMetaData item in ModsConfig.ActiveModsInLoadOrder)
            {
                if (!allActiveModIds.Contains(item.PackageId))
                {
                    allActiveModIds.Add(item.PackageId);
                }
            }
        }


    }
}
