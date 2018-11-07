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
    public class BaseDataViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public BaseDataViewModel(IRegionManager regionManager,IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _title = "基础资料";
            
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
                Id = "BuyCategoryView",
                Title = "采购分类",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_基础资料_基础信息_采购分类.ico",
                IsDefaultShow = false,               
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "UsersView",
                Title = "内控管理",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu2_资产管理_基础信息.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "UserView",
                Title = "资产管理",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu2_资产管理_基础信息.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ViewB",
                Title = "数据采集",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu2_资产管理_基础信息.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ViewA",
                Title = "系统管理",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu2_资产管理_基础信息.ico",
                IsDefaultShow = false,
            });
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "FundView",
                Title = "资金来源",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu2_资产管理_基础信息.ico",
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
