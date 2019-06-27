using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkManager.CustomType
{
    public class Blueprint
    {
        private string blueprintType;
        private List<Resource> resources;

        public string BlueprintType
        {
            get { return blueprintType; }
            set { blueprintType = value; }
        }
        public List<Resource> Resources
        {
            get { return resources; }
            set { resources = value; }
        }

        public Blueprint(string _BlueprintType, List<Resource> _resources)
        {
            BlueprintType = _BlueprintType;
            Resources = _resources;
        }
    }
}
