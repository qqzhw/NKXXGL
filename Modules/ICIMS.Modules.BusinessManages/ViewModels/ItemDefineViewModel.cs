using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ICIMS.Core.Events;
using System.Windows.Input;
using Unity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Modules.BusinessManages.Views;
using ICIMS.Core.Interactivity;
using Unity.Resolution;
using System.Threading;
using System.Collections.ObjectModel;
using ICIMS.Service.BusinessManages;
using ICIMS.Service;
using Newtonsoft.Json;
using ICIMS.Model.BusinessManages;
using Unity.Attributes;
using Telerik.Windows;
using Telerik.Windows.Controls;
using System.Windows;
using DelegateCommand = Prism.Commands.DelegateCommand;
using ICIMS.Model.User;

namespace ICIMS.Modules.BusinessManages.ViewModels
{
    public class ItemDefineViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IItemDefineService _itemDefineService;
        private readonly UserModel _userModel;



        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public ItemDefineViewModel(IEventAggregator eventAggregator,IUnityContainer unityContainer, IRegionManager regionManager, IItemDefineService itemDefineService,UserModel userModel)
        {
            _eventAggregator = eventAggregator;
            _container = unityContainer;
            _regionManager = regionManager;
            _itemDefineService = itemDefineService;
            _userModel = userModel;
            _title = "项目立项";
            _itemDefineLists = new ObservableCollection<ItemDefineList>();
            LoadedCommand = new DelegateCommand(OnLoad);
            AddCommand = new DelegateCommand(OnAddItem);
            EditCommand = new DelegateCommand(OnEditItem);
            DeleteCommand = new DelegateCommand(OnDelete);
            RefreshCommand= new DelegateCommand(OnRefresh);
            PageChanged = new DelegateCommand<Telerik.Windows.Controls.PageIndexChangedEventArgs>(OnPageChanged);
            SearchCommand = new DelegateCommand(OnSearchData);
           
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
            var view = _container.Resolve<ItemDefineEditView>(new ParameterOverride("data", SelectedItem));
            var notification = new Notification()
            {
                Title = "立项编辑",
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
            
            if (BeginTime != null)
            {
                if (BeginTime == EndTime)
                {
                    EndTime = BeginTime.Value.AddDays(1);
                }

            }
            var result = await _itemDefineService.GetAllItemDefines("", "", pageIndex: PageIndex, pageSize: PageSize);
            _itemDefineLists.Clear();
            TotalCount = result.TotalCount;
            ItemDefineLists.AddRange(result.Items);
            IsBusy = false;

        }
        private ObservableCollection<ItemDefineList> _itemDefineLists;  
        public ObservableCollection<ItemDefineList> ItemDefineLists
        {
            get { return _itemDefineLists; }
            set { SetProperty(ref _itemDefineLists, value); }
        }
        private ItemDefineList _selectedItem;
        public ItemDefineList SelectedItem
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
             
        }

        private void OnRefresh()
        {
            Initializer();
        }

        private void OnEditItem()
        {
            if (SelectedItem == null)
                return;
            var view = _container.Resolve<ItemDefineEditView>(new ParameterOverride("data", SelectedItem));
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
                Initializer();
                //}
                int s = 0;
            });
        }
        private void navigationCallback(NavigationResult nr)
        {
            if (nr.Error == null)
            {
                if (nr.Context.Parameters["Id"] != null)
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
        private void OnAddItem()
        {
            var view = _container.Resolve<ItemDefineEditView>();
            var notification = new Notification()
            {
                Title="立项新增",
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

        public ICommand AddCommand { get;protected set; }
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
         
        private int _pageIndex=0;
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
