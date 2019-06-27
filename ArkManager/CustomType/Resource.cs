using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkManager.CustomType
{
    public class Resource
    {
        private string resourceType;
        private int quantity;

        public string ResourceType
        {
            get { return resourceType; }
            set { resourceType = value; }
        }
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public Resource(string _ResourceType, int _Quantity)
        {
            ResourceType = _ResourceType;
            Quantity = _Quantity;
        }
    }
}
