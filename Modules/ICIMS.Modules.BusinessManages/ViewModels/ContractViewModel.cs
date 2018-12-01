using ICIMS.Core.Events;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Model.BusinessManages;
using ICIMS.Model.User;
using ICIMS.Modules.BusinessManages.Views;
using ICIMS.Service.BusinessManages;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Unity;
using Unity.Resolution;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace ICIMS.Modules.BusinessManages.ViewModels
{
    public  class ContractViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager; 
        private readonly IContractService _contractService;
        private readonly UserModel _userModel;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public ContractViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, IRegionManager regionManager, IContractService contractService, UserModel userModel)
        {
            _eventAggregator = eventAggregator;
            _container = unityContainer;
            _regionManager = regionManager;
            _contractService = contractService;
            _userModel = userModel;
            _title = "合同登记";
            _contractList = new ObservableCollection<ContractList>();
            AddCommand = new DelegateCommand(OnAddItem); 
            DeleteCommand = new DelegateCommand<object>(OnDelete);
            RefreshCommand = new DelegateCommand(OnRefresh);
            LoadedCommand = new DelegateCommand(OnLoad);
            AddCommand = new DelegateCommand(OnAddItem);
            EditCommand = new DelegateCommand(OnEditItem);
            DeleteCommand = new DelegateCommand(OnDelete);
            RefreshCommand = new DelegateCommand(OnRefresh);
            PageChanged = new DelegateCommand<Telerik.Windows.Controls.PageIndexChangedEventArgs>(OnPageChanged);
            SearchCommand = new DelegateCommand(OnSearchData);
            
          
        }

        private void OnDelete(object obj)
        {
            string confirmText = "你确定要删除当前项吗?";
            RadWindow.Confirm(new DialogParameters
            {
                Content = confirmText,
                Closed = new EventHandler<WindowClosedEventArgs>(OnConfirmClosed),
                Owner = Application.Current.MainWindow
            });
        }

        private void OnConfirmClosed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                //删除
            }
        }

        private void OnRefresh()
        {
            
        }
        

        //初始加载
        private void OnLoad()
        {
            Initializer();
        }
        //双击事件
        internal void OnDoubleClick(object sender, RadRoutedEventArgs e)
        {
            if (SelectedItem == null)
                return;
            var view = _container.Resolve<ContractEditView>(new ParameterOverride("data", SelectedItem));
            var notification = new Notification()
            {
                Title = "合同编辑",
                WindowState = System.Windows.WindowState.Maximized,
                Content = view,
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) => {
                //if (callback.DialogResult == true)
                //{
                //    var selectView = callback.Content as SelectItemCategoryView;
                //    var viewModel = selectView.DataContext as SelectItemCategoryViewModel;

                //}
                int s = 0;
            });
        }
        private void OnPageChanged(Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            Initializer();
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        private void OnSearchData()
        {
            TotalCount = 0;
            PageIndex = 0;
            Initializer();
        }


        private void OnSaveData()
        {

        }

        private async void Initializer()
        {
            IsBusy = true;
            _contractList.Clear();
            if (BeginTime != null)
            {
                if (BeginTime == EndTime)
                {
                    EndTime = BeginTime.Value.AddDays(1);
                }

            }
            var result = await _contractService.GetAllContracts("", "", pageIndex: PageIndex, pageSize: PageSize);

            TotalCount = result.TotalCount;
            _contractList.AddRange(result.Items);
            IsBusy = false;

        }
        private ObservableCollection<ContractList>  _contractList;
        public ObservableCollection<ContractList> ContractLists
        {
            get { return _contractList; }
            set { SetProperty(ref _contractList, value); }
        }
        private ContractList _selectedItem;
        public ContractList SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public DelegateCommand LoadedCommand { get; private set; }
        public ICommand SearchCommand { get; set; }
        public DelegateCommand UploadCommand { get; private set; }
        public ICommand PageChanged { get; set; }


        private void OnDelete()
        {

        }
         
        private void OnEditItem()
        {
            if (SelectedItem == null)
                return;
            var view = _container.Resolve<ContractEditView>(new ParameterOverride("data", SelectedItem));
            var notification = new Notification()
            {
                Title = "立项编辑",
                WindowState = System.Windows.WindowState.Maximized,
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) => {
                //if (callback.DialogResult == true)
                //{
                //    var selectView = callback.Content as SelectItemCategoryView;
                //    var viewModel = selectView.DataContext as SelectItemCategoryViewModel;

                //}
                int s = 0;
            });
        }
       
        private void OnAddItem()
        {
            var view = _container.Resolve<ContractEditView>();
            var notification = new Notification()
            {
                Title = "合同新增",
                WindowState = System.Windows.WindowState.Maximized,
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) => {
                //if (callback.DialogResult == true)
                //{
                //    var selectView = callback.Content as SelectItemCategoryView;
                //    var viewModel = selectView.DataContext as SelectItemCategoryViewModel;

                //}
                int s = 0;
            });
        }

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

        public ICommand AddCommand { get; protected set; }
        public ICommand EditCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand RefreshCommand { get; protected set; }
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
            _closeCommand ?? (_closeCommand = new DelegateCommand(() => {
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

        #region  分页属性

        private string _itemNo;
        public string ItemNo
        {
            get { return _itemNo; }
            set { SetProperty(ref _itemNo, value); }
        }
        private DateTime? _beginTime;
        public DateTime? BeginTime
        {
            get { return _beginTime; }
            set { SetProperty(ref _beginTime, value); }
        }
        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get { return _endTime; }
            set { SetProperty(ref _endTime, value); }
        }

        public int PageSize { get; set; } = 20;

        private int _pageIndex = 0;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { SetProperty(ref _pageIndex, value); }
        }
        private long _totalCount;
        public long TotalCount
        {
            get { return _totalCount; }
            set { SetProperty(ref _totalCount, value); }
        }
        private bool _isbusy;
        public bool IsBusy
        {
            get { return _isbusy; }
            set { SetProperty(ref _isbusy, value); }
        }
        #endregion
        
       
    }
   
   
}
