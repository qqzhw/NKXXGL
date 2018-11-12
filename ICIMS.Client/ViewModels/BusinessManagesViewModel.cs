using ICIMS.Core.Events;
using Prism.Commands;
using Prism.Events;
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
using Unity.Attributes;

namespace ICIMS.Client.ViewModels
{
    public partial class BusinessManagesViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public BusinessManagesViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _title = "内控管理";

        }

        private ObservableCollection<SystemInfoViewModel> _systemInfos;

        public ICommand SelectedCommand { get; private set; }

        public ObservableCollection<SystemInfoViewModel> SystemInfos
        {
            get { return _systemInfos; }
            set { SetProperty(ref _systemInfos, value); }
        }
        [InjectionMethod]
        public void Init()
        {
            _systemInfos = new ObservableCollection<SystemInfoViewModel>();
            SelectedCommand = new DelegateCommand<SystemInfoViewModel>(OnItemSelected);
            InitMenu();
        }
        private void InitMenu()
        {
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ItemDefineEditView",
                Title = "项目初始化",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_项目初始化.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ItemDefineView",
                Title = "立项登记",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_立项登记.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ReViewDefineEditView",
                Title = "评审登记",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_评审登记.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "YsCategoryView",
                Title = "合同登记",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_合同登记.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "VendorView",
                Title = "支付审核",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_支付审核系统.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ContractView",
                Title = "结算管理",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_合同结算.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "FundView",
                Title = "项目决算",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_项目决算.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "DocumentTypeView",
                Title = "项目绩效评价",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_项目绩效评价.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ItemCategoryView",
                Title = "档案管理",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_档案管理.ico",
                IsDefaultShow = false,
            });
           
            
        }

        private void OnItemSelected(SystemInfoViewModel selectedItem)
        {
            if (selectedItem == null)
                return;
            foreach (var item in _systemInfos)
            {
                item.IsSelected = false;
            }
            //  var model = selectedItems[0] as SystemInfoViewModel;
            selectedItem.IsSelected = true;
            var region = _regionManager.Regions["MainRegion"];
            _regionManager.RequestNavigate("MainRegion", new Uri(selectedItem.Id, UriKind.RelativeOrAbsolute), navigationCallback);

            // _regionManager.RequestNavigate(RegionNames.TabControlRegion, new Uri("FlandersView", UriKind.Relative));
            //  CustomPopupRequest.Raise(new Notification { Title = "Custom Popup", Content = "Custom Popup Message " });


        }

        private void navigationCallback(NavigationResult result)
        {

        }



        #region 通用属性


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }


        //whether the tab is selected;
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }

        //To do:define the UI for tabcontrol's content;
        public virtual UserControl View { get; set; }


        //The command when clicking Close Button;
        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(() =>
            {
                if (ConfirmToClose())
                {
                    _eventAggregator.GetEvent<TabCloseEvent>().Publish(View);
                }
            }));

        //It can be overwrite in inherited class to ask for confirming to closing the tab;
        protected virtual bool ConfirmToClose()
        {
            return true;
        }

        #endregion

    }
}
     
