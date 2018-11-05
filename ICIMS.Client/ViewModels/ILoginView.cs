using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJDeviceClient.ViewModels
{
    public  interface ILoginView
    {
        void CloseWindow();
        void ShowWindow();
        bool? DialogResult { get; set; }
    }
}
