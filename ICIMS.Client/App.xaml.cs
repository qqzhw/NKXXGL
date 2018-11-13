using ICIMS.Client.ViewModels;
using ICIMS.Client.Views;
using ICIMS.Core;
using ICIMS.Core.Common;
using ICIMS.Metro.Controls;
using ICIMS.Modules.BaseData;
using ICIMS.Modules.BusinessManages;
using ICIMS.Modules.SystemAdmin;
using ICIMS.Service;
using ICIMS.Service.BaseData;
using ICIMS.Service.BusinessManages;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using Unity.Lifetime;
using WJDeviceClient.ViewModels;

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
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.Error((Exception)e.ExceptionObject);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogHelper.Error((Exception)e.Exception);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!SingleInstanceCheck())
            {
                return;
            }
            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            //Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            //var loginView = Container.Resolve<LoginView>();
            //loginView.ShowDialog();
            //if (loginView.DialogResult == true)
            //{
            //    return Container.Resolve<MainWindow>();
            //}
            //else
            //{
            //    System.Environment.Exit(0);
            //    return null;
            //}

            return Container.Resolve<MainWindow>();
        }
        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Current.MainWindow = (Window)shell;
            Application.Current.MainWindow.Show();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BaseDataView>();
            //containerRegistry.Register<IWebApiClient, WebApiClient>();
            var webApiClient = new WebApiClient();
            webApiClient.TenancyName = "Default";
            webApiClient.UserName = "admin";
            webApiClient.Password = "123qwe";
            webApiClient.TokenBasedAuth();
            containerRegistry.RegisterInstance<IWebApiClient>(webApiClient);

            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IFundFromService, FundFromService>();
            containerRegistry.Register<IPaymentTypeService, PaymentTypeService>();
            containerRegistry.Register<IItemCategoryService, ItemCategoryService>();
            containerRegistry.Register<IDocumentTypeService, DocumentTypeService>();
            containerRegistry.Register<IContractCategoryService, ContractCategoryService>();
            containerRegistry.Register<IVendorService, VendorService>();
            containerRegistry.Register<IYsCategoryService, YsCategoryService>();
            containerRegistry.Register<ISubjectService, SubjectService>();
            containerRegistry.Register<IOrganizationUnitService, OrganizationUnitService>();

            containerRegistry.Register<IContractService, ContractService>();
            containerRegistry.Register<IItemDefineService, ItemDefineService>();
            containerRegistry.Register<IReViewDefineService, ReViewDefineService>();
            containerRegistry.Register<IBudgetService, BudgetService>();
            containerRegistry.Register<IPayAuditService, PayAuditService>();
            containerRegistry.Register<IBusinessTypeService, BusinessTypeService>();
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
            moduleCatalog.AddModule<BusinessManagesModule>();
        }
        private static Mutex SingleInstanceMutex = new Mutex(true, "{86A802DF-C96C-8765-BAA8-1BC527857BEB}");

        private static bool SingleInstanceCheck()
        {

            if (!SingleInstanceMutex.WaitOne(TimeSpan.Zero, true))
            {
                Process thisProc = Process.GetCurrentProcess();
                Process process = Process.GetProcessesByName(thisProc.ProcessName).FirstOrDefault(delegate (Process p)
                {
                    if (p.Id != thisProc.Id)
                    {
                        return true;
                    }
                    return false;
                });
                if (process != null)
                {
                    IntPtr mainWindowHandle = process.MainWindowHandle;
                    if (NativeMethods.IsIconic(mainWindowHandle))
                    {
                        NativeMethods.ShowWindow(mainWindowHandle, 9);
                    }
                    NativeMethods.SetForegroundWindow(mainWindowHandle);
                }
                Application.Current.Shutdown(1);
                return false;
            }
            return true;
        }

        internal static void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                NativeMethods.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
            GC.Collect();
        }

    }
}
