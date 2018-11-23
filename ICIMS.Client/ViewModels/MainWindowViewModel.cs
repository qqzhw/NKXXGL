using CommonServiceLocator;
using ICIMS.Core.Events;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Unity;
using Unity.Attributes;

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
        private readonly IWebApiClient _webApiClient;
        public MainWindowViewModel(IUnityContainer container, IEventAggregator eventAggregator, IRegionManager regionManager, IModuleManager moduleManager, IServiceLocator serviceLocator, IUserService userSerice, IWebApiClient webApiClient)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _moduleManager = moduleManager;
            _serviceLocator = serviceLocator;
            _userSerice = userSerice;
            _webApiClient = webApiClient;
            //  CustomPopupRequest = new InteractionRequest<INotification>();
            CustomPopupCommand = new DelegateCommand(RaiseCustomPopup);
            _systemInfos = new ObservableCollection<SystemInfoViewModel>();
            LoadedCommand = new DelegateCommand<object>(OnLoad);
            SelectedCommand = new DelegateCommand<SystemInfoViewModel>(OnItemSelected);
            ConnectCmd = new DelegateCommand<string>(OnConnectedDevice);
            eventAggregator.GetEvent<TabCloseEvent>().Subscribe(OnTabActive);
            Telerik.Windows.Controls.StyleManager.ApplicationTheme = new Telerik.Windows.Controls.Office2016TouchTheme();
            InitLoadSetting();

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

        private void OnConnectedDevice(string deviceNo)
        {
            foreach (var item in _systemInfos)
            {
                item.IsSelected = false;
            }
            var model = _systemInfos.FirstOrDefault(o => o.Id == "WaterDataView");
            model.IsSelected = true;
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("deviceNo", deviceNo);
            var region = _regionManager.Regions["MainRegion"];
            _regionManager.RequestNavigate("MainRegion", model.Id, navigationCallback, navigationParameters);
            SelectedItem = model;
        }

        public ICommand SelectedCommand { get; private set; }
        public ICommand ConnectCmd { get; private set; }
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
   

        /// <summary>
        /// 加载设置选项
        /// </summary>
        public  async void InitLoadSetting()
        {
            // _title = Settings.Default.AppName;
            //var ss =await _userSerice.GetUserInfoAsync(1);
           // _webApiClient.TenancyName = "Default";
            _webApiClient.UserName = "admin";
            _webApiClient.Password = "123qwe";
            _webApiClient.TokenBasedAuth();
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            keyValuePairs.Add(new KeyValuePair<string, string>("Id", "5"));
            keyValuePairs.Add(new KeyValuePair<string, string>("FileName", "FileNames"));
            keyValuePairs.Add(new KeyValuePair<string, string>("documenttype", "wORDWWEN文档"));
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
                Id = "ViewB",
                Title = "数据采集",
                InitMode = InitializationMode.OnDemand,
                IsDefaultShow = false,
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu1_软件接口.ico",
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ViewA",
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
