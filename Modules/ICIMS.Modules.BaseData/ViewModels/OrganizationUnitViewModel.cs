using ICIMS.Core.Events;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Model.BaseData;
using ICIMS.Modules.BaseData.Views;
using ICIMS.Service;
using ICIMS.Service.BaseData;
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
using Unity;
using Unity.Attributes;

namespace ICIMS.Modules.BaseData.ViewModels
{
    public class OrganizationUnitViewModel : BindableBase, INavigationAware
    {

        private readonly IEventAggregator _eventAggregator;
        private readonly IOrganizationUnitService _service;
        private readonly IRegionManager _regionManager;
        private IUnityContainer _unityContainer;
        public OrganizationUnitViewModel(IEventAggregator eventAggregator,
            IOrganizationUnitService service,
            IRegionManager regionManager,
            IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            _service = service;
            this._regionManager = regionManager;
            eventAggregator.GetEvent<TabCloseEvent>().Subscribe(OnTabActive);
            AddCommand = new DelegateCommand<object>(OnAddCommand);
            EditCommand = new DelegateCommand<object>(OnEditCommand);
            DeleteCommand = new DelegateCommand<object>(OnDeleteCommand);
            RefreshCommand = new DelegateCommand<object>(OnRefreshCommand);
        }

        private async void OnDeleteCommand(object obj)
        {
            if (MessageBox.Show("请确认是否删除", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                try
                {
                    await _service.Delete(SelectedItem.Id);
                    this._datas.Remove(this.SelectedItem);
                    this.Items.Remove(this.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        private void OnEditCommand(object obj)
        {
            var newItem = new OrganizationUnitEditViewModel() { ShowReAddBtn = false };
            newItem.Item = CommonHelper.CopyItem(this.SelectedItem);
            OrganizationUnitEditView view = new OrganizationUnitEditView(newItem);
            var notification = new Notification()
            {
                Title = "部门信息",
                Content = view,// (new ParameterOverride("name", "")),
            };
            PopupWindows.NotificationRequest.Raise(notification, async (callback) =>
            {
                if (newItem.IsOkClicked != null)
                {
                    if (newItem.IsOkClicked.Value)
                    {
                        try
                        {
                            var data = await _service.Update(newItem.Item);
                            if (data != null)
                            {
                                var oriItem = this._datas.FirstOrDefault(a => a.Id == newItem.Item.Id);

                                CommonHelper.SetValue(oriItem, newItem.Item);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }


                    }
                }
            });
            view.BindAction(notification.Finish);
        }

        private void OnRefreshCommand(object obj)
        {
            this.Init();
        }

        private void OnAddCommand(object obj)
        {
            var newItem = new OrganizationUnitEditViewModel() { ShowReAddBtn = true };
            newItem.Item = new OrganizationUnitItem();
            OrganizationUnitEditView view = new OrganizationUnitEditView(newItem);
            var notification = new Notification()
            {
                Title = "部门信息",
                Content = view,// (new ParameterOverride("name", "")),
            };
            PopupWindows.NotificationRequest.Raise(notification, async (callback) =>
            {
                if (newItem.IsOkClicked != null)
                {
                    if (newItem.IsOkClicked.Value)
                    {
                        try
                        {
                            var data = await _service.Create(newItem.Item);
                            if (data != null)
                            {
                                this._datas.Add(data);
                                this.InitOneData(_datas, data);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                }
                else
                {

                }
            });
            view.BindAction(notification.Finish);

        }

        private void OnTabActive(UserControl view)
        {
            var region = _regionManager.Regions["MainRegion"];
            if (region.Views.Count() > 1)
            {
                if (view != null)
                {
                    if (region.Views.Contains(view))
                        region.Remove(view);
                }
            }
        }

        [InjectionMethod]
        public async void Init()
        {
            _title = "部门信息";
            this.Items = new ObservableCollection<OrganizationUnitItem>();
            var rs = await _service.GetPageItems(this.No, this.Name, this.PageIndex, this.PageSize);
            this._datas = rs.datas;
            foreach (var data in _datas)
            {
                InitOneData(_datas, data);
            }
            this.ItemCount = rs.totalCount;
            this.SelectedItem = this.Items.FirstOrDefault();
        }

        private void InitOneData(List<OrganizationUnitItem> datas, OrganizationUnitItem data)
        {
            if (data.GroupNo != data.Code)
            {
                data.Parent = datas.FirstOrDefault(a => a.Code == data.GroupNo);
                data.Parent?.Children.Add(data);
            }
            else
            {
                this.Items.Add(data);
            }
        }

        private List<OrganizationUnitItem> _datas;

        public OrganizationUnitItem SelectedItem { get => _selectedItem; set { this._selectedItem = value; this.RaisePropertyChanged(nameof(SelectedItem)); } }

        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        private ObservableCollection<OrganizationUnitItem> _items;

        public ObservableCollection<OrganizationUnitItem> Items
        {
            get
            {
                return this._items;
            }
            set
            {
                this._items = value;
                this.RaisePropertyChanged(nameof(Items));
            }
        }

        private int _pageIndex = 0;
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                this._pageIndex = value;
                this.Init();
                this.RaisePropertyChanged(nameof(PageIndex));
            }
        }



        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                this._pageSize = value;
                this.Init();
                this.RaisePropertyChanged(nameof(PageSize));
            }
        }

        private int _itemCount = 3;
        public int ItemCount
        {
            get
            {
                return this._itemCount;
            }
            set
            {
                this._itemCount = value;
                this.RaisePropertyChanged(nameof(ItemCount));
            }
        }



        private string _no;
        public string No
        {
            get
            {
                return _no;
            }
            set
            {
                this._no = value;
                this.Init();
                this.RaisePropertyChanged(nameof(No));
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                this._name = value;
                this.Init();
                this.RaisePropertyChanged(nameof(Name));
            }
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
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
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

        public Action<bool?> Close { get; internal set; }


        #endregion

        private DelegateCommand _pageChangedCommand;
        private OrganizationUnitItem _selectedItem;

        public DelegateCommand PageChangedCommand
        {
            get => _pageChangedCommand;
            set => _pageChangedCommand = value;
        }
    }
}
