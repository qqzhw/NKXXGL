using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICIMS.Launcher.AutoUpdater
{
    public interface IAutoUpdater
    {
        void Update();
        bool IsHasUpdate();

        void RollBack();
    }
}
