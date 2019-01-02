using ICIMS.Core.Events;
using ICIMS.Model.User;
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
        private UserModel _userModel;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public BaseDataViewModel(IRegionManager regionManager,IEventAggregator eventAggregator,UserModel userModel)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _userModel = userModel;
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
            }.IsEnabled(_userModel.Permissions, "Pages.BuyCategory"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "SubjectView",
                Title = "功能科目",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_基础资料_基础信息_功能科目.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.FunctionSubject"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "YsCategoryView",
                Title = "预算分类",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_基础资料_基础信息_预算分类.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.YSCategory"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "VendorView",
                Title = "供应商信息",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_基础资料_基础信息_供应商信息.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.Vendor"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ContractCategoryView",
                Title = "合同分类",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_基础资料_基础信息_合同分类.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.ContractCategory"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "OrganizationUnitView",
                Title = "部门信息",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu2_资产管理_基础信息.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.BusinessAudit"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "DocumentTypeView",
                Title = "文档分类",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_基础资料_基础信息_文档分类.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.DocumentCategory"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ItemCategoryView",
                Title = "项目分类",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_基础资料_基础信息_项目分类.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.ItemCategory"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "PaymentTypeView",
                Title = "支付类型",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_基础资料_基础信息_支付类型.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.PaymentType"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "FundView",
                Title = "资金来源",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_基础资料_基础信息_资金来源.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.FundFrom"));

            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ProjectPropsView",
                Title = "项目属性",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_基础资料_基础信息_文档分类.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.ProjectProps"));
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
