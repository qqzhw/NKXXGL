
using ICIMS.Modules.BaseData;
using Prism.Modularity;
using Prism.Unity;  
using System;
using System.Windows;
using Unity.Lifetime;

namespace ICIMS.Client
{
	class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            //Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            //var loginView= Container.Resolve<LoginView>();
            //  loginView.ShowDialog();
            //if (loginView.DialogResult == true)
            //{
            //    return Container.Resolve<MainWindow>();
            //}
            //else
            //{
            //    System.Environment.Exit(0);
           return null;
            //}
           
        }

        protected override void InitializeShell()
        {
            // var ident = WindowsIdentity.GetCurrent();
            // var principal = new GenericPrincipal(ident, new string[] { "User" });
            //Thread.CurrentPrincipal = principal; 
            //  AppDomain.CurrentDomain.SetThreadPrincipal(principal);
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }
        protected override void ConfigureServiceLocator()
        {
            base.ConfigureServiceLocator();
           // Container.RegisterType<ILoginView, LoginView>(new ContainerControlledLifetimeManager());
           // Container.RegisterType<MainWindowViewModel>(new ContainerControlledLifetimeManager());
			RegisterService();
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
           // Container.RegisterType<IModuleInitializer, RoleBasedModuleInitializer>(new ContainerControlledLifetimeManager());
           
        }
		private void RegisterService()
		{
			//Container.RegisterType<IDataService, DataService>(new ContainerControlledLifetimeManager());
			//Container.RegisterType<SettingsViewModel>(new ContainerControlledLifetimeManager());
          //  var dataService = new DataService(Settings.Default.ServerUrl);
          //  Container.RegisterInstance(typeof(IDataService), dataService,new ContainerControlledLifetimeManager());           
		}

		protected override void InitializeModules()
		{
			base.InitializeModules();
		 
		}
		protected override void ConfigureModuleCatalog()
		{
			Type typeModule = typeof(ICIMSModule);
			ModuleInfo module = new ModuleInfo
			{   //  ModuleA没有设置InitializationMode,默认为WhenAvailable
				ModuleName = typeModule.Name,
				ModuleType = typeModule.AssemblyQualifiedName,
			};
            Type typeModule2 = typeof(BaseDataModule);
            ModuleInfo module2 = new ModuleInfo
            {   //  ModuleA没有设置InitializationMode,默认为WhenAvailable
                ModuleName = typeModule.Name,
                ModuleType = typeModule.AssemblyQualifiedName,
              //  InitializationMode=InitializationMode.OnDemand
            };
            base.ModuleCatalog.AddModule(module);
		}
		 

	}
}
