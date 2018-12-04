using ICIMS.Modules.BusinessManages.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ICIMS.Modules.BusinessManages
{
    public class BusinessManagesModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _unityContainer;
        public BusinessManagesModule(IRegionManager regionManager, IUnityContainer unityContainer)
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
            containerRegistry.RegisterForNavigation<ItemDefineView>();
            containerRegistry.RegisterForNavigation<ItemDefineEditView>();
            containerRegistry.RegisterForNavigation<ContractView>();
            containerRegistry.RegisterForNavigation<ContractEditView>();
            containerRegistry.RegisterForNavigation<PayAuditEditView>();
            containerRegistry.RegisterForNavigation<PayAuditView>();
            containerRegistry.RegisterForNavigation<ReViewDefineView>();
            containerRegistry.RegisterForNavigation<ReViewDefineEditView>();
            containerRegistry.RegisterForNavigation<ScanFileView>();
            //containerRegistry.RegisterForNavigation<ViewB>();
        }
    }
  
}
