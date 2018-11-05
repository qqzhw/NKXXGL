using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICIMS.Controls
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContainerItemAttribute:Attribute
    {
        public Type ItemType
        {
            get;
            private set;
        }

        public ContainerItemAttribute(Type itemType)
		{ 
			this.ItemType = itemType;
		}
    }
}
