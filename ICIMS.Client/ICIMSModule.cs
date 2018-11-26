
using ICIMS.Client.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using Unity;

namespace ICIMS.Client
{
	public class ICIMSModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _unityContainer;
        public ICIMSModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            _regionManager = regionManager;
            _unityContainer = unityContainer;
			 
		}


		public void Initialize()
		{
			
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //_regionManager.RegisterViewWithRegion("MainRegion", typeof(MainView));
            //_unityContainer.RegisterTypeForNavigation<MainView>("MainView");
            //_unityContainer.RegisterTypeForNavigation<WaterDataView>("WaterDataView");
            //_unityContainer.RegisterTypeForNavigation<DeviceInfoView>("DeviceInfoView");
            //_unityContainer.RegisterTypeForNavigation<DeviceStatusView>("DeviceStatusView");
            //_unityContainer.RegisterTypeForNavigation<SettingsView>("SettingsView");
            //_unityContainer.RegisterTypeForNavigation<RainDataView>("RainDataView");
            var regionManager = containerProvider.Resolve<IRegionManager>();
           

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA>();
            containerRegistry.RegisterForNavigation<ViewB>();
            containerRegistry.RegisterForNavigation<BusinessManagesView>();
            containerRegistry.RegisterForNavigation<SystemAdminView>();
        }
    }
}
