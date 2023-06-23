using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaPrefabs
{
    public class Utils
    {

        public static bool ContainsAllItems(List<string> a, List<string> b)
        {
            return !b.Except(a).Any();
        }


    }
}
