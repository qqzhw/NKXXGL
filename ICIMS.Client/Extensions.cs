using ICIMS.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Client
{
   public static class Extensions
    {

        public static SystemInfoViewModel IsEnabled(this SystemInfoViewModel viewModel, List<string> permissions, string permissionName)
        {
            var findItem = permissions.FirstOrDefault(o => o == permissionName);
            if (!string.IsNullOrEmpty(findItem))
            {
                viewModel.IsReadOnly = true;
                viewModel.Opacity = 1.0;
            }
            else
            {
                viewModel.IsReadOnly = false;
                viewModel.Opacity = 0.5;
            }
            return viewModel;
        }
    }
}
