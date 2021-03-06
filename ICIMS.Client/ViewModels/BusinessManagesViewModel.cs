﻿using ICIMS.Core.Events;
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
    public partial class BusinessManagesViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly UserModel _userModel;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public BusinessManagesViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, UserModel userModel)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _title = "内控管理";
            _userModel = userModel;
        }

        private ObservableCollection<SystemInfoViewModel> _systemInfos;

        public ICommand SelectedCommand { get; private set; }

        public ObservableCollection<SystemInfoViewModel> SystemInfos
        {
            get { return _systemInfos; }
            set { SetProperty(ref _systemInfos, value); }
        }
        private ObservableCollection<SystemInfoViewModel> _ysInfos;
        public ObservableCollection<SystemInfoViewModel> YsInfos
        {
            get { return _ysInfos; }
            set { SetProperty(ref _ysInfos, value); }
        }
        private ObservableCollection<SystemInfoViewModel> _bbInfos;
        public ObservableCollection<SystemInfoViewModel> BbInfos
        {
            get { return _bbInfos; }
            set { SetProperty(ref _bbInfos, value); }
        }
        [InjectionMethod]
        public void Init()
        {
            _systemInfos = new ObservableCollection<SystemInfoViewModel>();
            _ysInfos= new ObservableCollection<SystemInfoViewModel>();
            _bbInfos = new ObservableCollection<SystemInfoViewModel>();
            SelectedCommand = new DelegateCommand<SystemInfoViewModel>(OnItemSelected);
            InitMenu();
            InitMenu2();
            InitMenu3();
        }
        private void InitMenu()
        {
            var itemInit = new SystemInfoViewModel()
            {
                Id = "ItemDefineEditView",
                Title = "项目初始化",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_项目初始化.ico",
                IsDefaultShow = false,
                IsReadOnly = false
            };
            itemInit.IsEnabled(_userModel.Permissions, "");
            _systemInfos.Add(itemInit);
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ItemDefineView",
                Title = "立项登记",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_立项登记.ico",
                IsDefaultShow = false,
                
            }.IsEnabled(_userModel.Permissions, "Pages.ItemDefine"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ReViewDefineView",
                Title = "评审登记",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_评审登记.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.ReViewDefine"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ContractView",
                Title = "合同登记",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_合同登记.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.Contract"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "PayAuditView",
                Title = "支付审核",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_支付审核系统.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.PayAudit"));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "ContractView",
                Title = "结算管理",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_合同结算.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "PayAuditView",
                Title = "项目决算",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_项目决算.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "PayAuditView",
                Title = "项目绩效评价",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_项目绩效评价.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            _systemInfos.Add(new SystemInfoViewModel()
            {
                Id = "DocManagerView",
                Title = "文档综合管理",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_项目管理_档案管理.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "Pages.ReViewDefine"));


        }
        private void InitMenu2()
        {
            _ysInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetView",
                Title = "预算编制",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_预算管理_预算编制.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            _ysInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetLabelView",
                Title = "预算归类",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_预算管理_预算归类.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            _ysInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetAdjustView",
                Title = "预算调整",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_预算管理_预算调整.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            _ysInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetReplyView",
                Title = "预算批复",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_预算管理_预算批复.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            _ysInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetMiddleAdjustView",
                Title = "中期调整",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_预算管理_中期调整.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            _ysInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetMiddleReplyView",
                Title = "中期批复",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目管理_预算管理_中期批复.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));


        }
        private void InitMenu3()
        {
            _bbInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetView",
                Title = "合同汇总表",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_合同汇总表.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, "")); 
            _bbInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetLabelView",
                Title = "合同明细表",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_合同明细表.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            _bbInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetAdjustView",
                Title = "收支汇总表",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_收支汇总表.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            BbInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetReplyView",
                Title = "收支明细表",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_收支明细表.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            BbInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetMiddleAdjustView",
                Title = "项目汇总表",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目汇总表.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            BbInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetMiddleReplyView",
                Title = "项目明细表",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_项目明细表.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));
            BbInfos.Add(new SystemInfoViewModel()
            {
                Id = "BudgetMiddleReplyView",
                Title = "预算执行情况表",
                Icon = "pack://application:,,,/ICIMS.Controls;component/MenuImage/Menu3_预算执行情况表.ico",
                IsDefaultShow = false,
            }.IsEnabled(_userModel.Permissions, ""));

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
     
