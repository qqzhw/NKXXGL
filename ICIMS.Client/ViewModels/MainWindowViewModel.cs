using CommonServiceLocator;
using ICIMS.Client.Properties;
using ICIMS.Client.Views;
using ICIMS.Core.Events;
using ICIMS.Model.User;
using ICIMS.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unity;
using Unity.Attributes;
using Unity.Lifetime;

namespace ICIMS.Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private readonly IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IModuleManager _moduleManager;
        private readonly IServiceLocator _serviceLocator;
        private readonly IUserService _userSerice;
        private readonly UserModel _userInfo;
        public MainWindowViewModel(IUnityContainer container, IEventAggregator eventAggregator, IRegionManager regionManager, IModuleManager moduleManager, IServiceLocator serviceLocator, IUserService userSerice, UserModel userInfo)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _moduleManager = moduleManager;
            _serviceLocator = serviceLocator;
            _userSerice = userSerice;
            _userModel = userInfo;
            //_userModel = new UserModel();
            //  CustomPopupRequest = new InteractionRequest<INotification>();
            CustomPopupCommand = new DelegateCommand(RaiseCustomPopup);
            _systemInfos = new ObservableCollection<SystemInfoViewModel>();
            LoadedCommand = new DelegateCommand<object>(OnLoad);
            SelectedCommand = new DelegateCommand<SystemInfoViewModel>(OnItemSelected);
            LoginCommand = new Prism.Commands.DelegateCommand(OnLogin);
            eventAggregator.GetEvent<TabCloseEvent>().Subscribe(OnTabActive);
            Telerik.Windows.Controls.StyleManager.ApplicationTheme = new Telerik.Windows.Controls.Office2016TouchTheme();
            Application.Current.MainWindow.Closing += MainWindow_Closing;
            InitLoadSetting();
            
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           // StartMainProgram();
        }

        private void OnLogin()
        {
            //Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            //var loginView = _container.Resolve<LoginView>();
            //loginView.ShowDialog();
            //if (loginView.DialogResult == true)
            //{
            //    return Container.Resolve<MainWindow>();
            //}
            RestartApplication();
        }
        private   void RestartApplication()
        {
            //System.Windows.Forms.Application.Restart();
            // Application.ExitThread();
            // Process.Start(Process.GetCurrentProcess().MainModule.FileName);
             StartMainProgram();
             Environment.Exit(0); 
            
            //Application.Current.Shutdown();
        }

        /// <summary>
        /// 启动主程序
        /// </summary>
        private  void StartMainProgram()
        {
            using (Process process = new Process())
            {
                process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory+Settings.Default.MainProgramName;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Verb = "RunAs";
                try
                {
                   // MessageListener.Instance.ReceiveMessage("正在启动主程序...");
                    Thread.Sleep(200);
                    process.Start();
                }
                catch (Exception ex)
                {
                     MessageBox.Show(string.Format("启动主程序失败：{0}", ex.Message));
                   // Thread.Sleep(500);
                }
            }
        }
        private void OnLoad(object obj)
        {
            //默认加载首页
            var defaultView = SystemInfos.FirstOrDefault(o => o.IsDefaultShow);
            OnItemSelected(defaultView);
        }

        private void OnTabActive(UserControl view)
        {
           
            var region = _regionManager.Regions["MainRegion"];
            if (region.Views.Count()>1)
            {
                if (view != null)
                {
                    region.Remove(view);
                }
            }
          
        }

       

        public ICommand SelectedCommand { get; private set; }
        public DelegateCommand LoginCommand { get; private set; }
        public ICommand LoadedCommand { get; private set; }

        private ObservableCollection<SystemInfoViewModel> _systemInfos;
        public ObservableCollection<SystemInfoViewModel> SystemInfos
        {
            get { return _systemInfos; }
            set { SetProperty(ref _systemInfos, value); }
        }
      //  public  InteractionRequest<INotification> CustomPopupRequest { get; set; }
        public DelegateCommand CustomPopupCommand { get; set; }

        private SystemInfoViewModel _selectItem;
        public SystemInfoViewModel SelectedItem
        {
            get { return _selectItem; }
            set { SetProperty(ref _selectItem, value); }
        }
        private UserModel _userModel;
        public UserModel UserModel
        {
            get { return _userModel; }
            set { SetProperty(ref _userModel, value); }
        }
        private string _displayNames;
        public string DisplayNames {
            get { return _displayNames; }
            set { SetProperty(ref _displayNames, value); }
        }
        /// <summary>
        /// 加载设置选项
        /// </summary>
        public  async void InitLoadSetting()
        {
            // _title = Settings.Default.AppName;
            //var ss =await _userSerice.GetUserInfoAsync(1);
            //string TenancyName = "Default";
            //string UserName = "admin";
            //string Password = "123qwe";
            //var user=_userSerice.LoginAsync(UserName,Password,TenancyName);
            //var userInfo =await _userSerice.GetUserInfoById(user.Id);
            //userInfo.AccessToken = user.AccessToken;
            //userInfo.UnitId = userInfo.Unit.Id;
            //userInfo.UnitName = userInfo.Unit.Name;
            //if (unit!=null)
            //{
            //    user.UnitId = unit.Id;
            //    user.UnitName = unit.Name;
            //}
           //  _container.RegisterInstance(userInfo, new ContainerControlledLifetimeManager());
            //var roles = await _userSerice。.GetUserRoles();
            //foreach (var item in roles.Items)
            //{
            //    user.RoleIds.Add(item.Id);
            //    user.RoleDisplayNames.Add(item.DisplayName); 
            //    user.RoleNames.Add(item.Name);
            //}
            //UserModel = _userInfo;
            DisplayNames = UserModel.Roles.Select(o=>o.DisplayName).FirstOrDefault();
            //List <KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            //keyValuePairs.Add(new KeyValuePair<string, string>("Id", "5"));
            //keyValuePairs.Add(new KeyValuePair<string, string>("FileName", "FileNames"));
            //keyValuePairs.Add(new KeyValuePair<string, string>("documenttype", "wORDWWEN文档"));
            //var sss = await _webApiClient.UploadFileAsync<object>("http://localhost:21025/api/FileManage/UploadAsync", keyValuePairs, "d:\\te55555.png", "te55555.png");
        }

        [InjectionMethod]
        public void Init()
        {

            InitHeader();


            //string fileName = "testdb.bak";
            //String sourceFullPath = Path.Combine("D:\\", fileName);
            //if (!File.Exists(sourceFullPath))
            //{
            //	throw new Exception("A file given by the sourcePath doesn't exist."); 
            //}

            //String targetFullPath = Path.Combine("F:\\5555\\", fileName); 


            //FileUtilities.CreateDirectoryIfNotExist(Path.GetDirectoryName(targetFullPath));

            //FileUtilities.CopyFileEx(sourceFullPath, targetFullPath, token);



        }

        private void InitHeader()
        {
            var defaultview = new SystemInfoViewModel()
            {
                Id = "BaseDataView",
                Title = "基础资料",
                InitMode = InitializationMode.OnDemand,
                IsDefaultShow = true,
                IsSelected = true,
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu1_基础资料.ico",
            };
            _systemInfos.Add(defaultview);
           _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "BusinessManagesView",
                Title = "内控管理",
                InitMode = InitializationMode.OnDemand,
                IsDefaultShow = false,
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu1_业务管理.ico",
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "UserView",
                Title = "资产管理",
                InitMode = InitializationMode.OnDemand,
                IsDefaultShow = false,
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu1_资产管理.ico",
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ScanFileView",
                Title = "数据采集",
                InitMode = InitializationMode.OnDemand,
                IsDefaultShow = false,
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu1_软件接口.ico",
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "SystemAdminView",
                Title = "系统管理",
                InitMode = InitializationMode.OnDemand,
                IsDefaultShow = false,
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu1_系统管理.ico",
            });
          
        }

        private void RaiseCustomPopup()
        {

        }

        private void OnItemSelected(SystemInfoViewModel selectedItem)
        {
            if (selectedItem != null )
            {
                foreach (var item in _systemInfos)
                {
                    item.IsSelected = false;
                }
                //var model = selectedItems[0] as SystemInfoViewModel;
                selectedItem.IsSelected = true;
                var region = _regionManager.Regions["MainRegion"];
                _regionManager.RequestNavigate("MainRegion", new Uri(selectedItem.Id, UriKind.Relative), navigationCallback);

                // _regionManager.RequestNavigate(RegionNames.TabControlRegion, new Uri("FlandersView", UriKind.Relative));
                //  CustomPopupRequest.Raise(new Notification { Title = "Custom Popup", Content = "Custom Popup Message " });

            }
        }


        private void navigationCallback(NavigationResult nr)
        {
            if (nr.Error == null)
            {
                if (nr.Context.Parameters["deviceNo"] != null)
                {
                    //var views = nr.Context.NavigationService.Region.ActiveViews;
                    //var view = views.FirstOrDefault() as UserControl;
                    //if (view!=null)
                    //{
                    //    var viewModel = view.DataContext as WaterDataViewModel;
                    //}
                 //   var deviceNo = nr.Context.Parameters["deviceNo"].ToString();
                 //   _eventAggregator.GetEvent<CommonEventArgs<string>>().Publish(deviceNo);

                }
            }
        }

    }
}
