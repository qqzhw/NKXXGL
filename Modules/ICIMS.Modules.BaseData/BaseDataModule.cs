using ICIMS.Modules.BaseData.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ICIMS.Modules.BaseData
{
    public class BaseDataModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _unityContainer;
        public BaseDataModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            _regionManager = regionManager;
            _unityContainer = unityContainer;

        }


        public void Initialize()
        {

        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
           
            var regionManager = containerProvider.Resolve<IRegionManager>();


        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<UserView>();
            //containerRegistry.RegisterForNavigation<ViewB>();
        }
    }
}
 
