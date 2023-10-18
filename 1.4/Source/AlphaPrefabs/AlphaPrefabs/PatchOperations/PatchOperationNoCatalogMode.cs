using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Verse;

namespace AlphaPrefabs
{
    public class PatchOperationNoCatalogMode : PatchOperation
    {

        private PatchOperation match;

        private PatchOperation nomatch;

        protected override bool ApplyWorker(XmlDocument xml)
        {

            if (AlphaPrefabs_Settings.noPrefabCatalog)
            {
                if (match != null)
                {
                    return match.Apply(xml);
                }
            }
            else if (nomatch != null)
            {
                return nomatch.Apply(xml);
            }
            return true;
        }


    }
}