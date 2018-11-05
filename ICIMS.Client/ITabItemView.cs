using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Client
{
    public interface ITabItemView
    {
        int TabItemIndex { get; }

        bool IsStartupTab { get; }

        string TabName { get; set; }
    }
}
