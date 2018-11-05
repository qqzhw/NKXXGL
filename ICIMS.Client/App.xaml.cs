using ICIMS.Client.Views;
using ICIMS.Core;
using ICIMS.Metro.Controls;
using ICIMS.Modules.BaseData;
using ICIMS.Modules.SystemAdmin;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"Log4Net\log4Net.config", Watch = true)]
namespace ICIMS.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
         

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
           
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            regionAdapterMappings.RegisterMapping(typeof(RadTabControl), Container.Resolve<RadTabControlAdapter>());
            
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
           
            moduleCatalog.AddModule<ICIMSModule>();
            Type typeModule = typeof(BaseDataModule);
            ModuleInfo module2 = new ModuleInfo
            {   //  ModuleA没有设置InitializationMode,默认为WhenAvailable
                ModuleName = typeModule.Name,
                ModuleType = typeModule.AssemblyQualifiedName,
                //  InitializationMode=InitializationMode.OnDemand
            };
            moduleCatalog.AddModule<BaseDataModule>(); 
            moduleCatalog.AddModule<SystemAdminModule>();
        }
        
    }
}
