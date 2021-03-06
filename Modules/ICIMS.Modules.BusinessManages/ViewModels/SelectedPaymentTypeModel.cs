﻿using ICIMS.Model.BaseData;
using ICIMS.Service.BaseData;
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
using System.Windows.Input;
using Telerik.Windows;
using Unity;
using Unity.Attributes;

namespace ICIMS.Modules.BusinessManages.ViewModels
{
  
    public class SelectedPaymentTypeModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IPaymentTypeService _paymenttypeService;
        private readonly IItemDefineService _itemDefineService;         
        public SelectedPaymentTypeModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, IRegionManager regionManager, IPaymentTypeService paymenttypeService, IItemDefineService itemDefineService)
        {
            _eventAggregator = eventAggregator;
            _container = unityContainer;
            _regionManager = regionManager;
            _paymenttypeService = paymenttypeService;
            _itemDefineService = itemDefineService;
            
            SaveCommand = new DelegateCommand(OnAddItem);
            CancelCommand = new DelegateCommand(OnCancel);
            DoubleClick = new DelegateCommand<object>(OnDoubleClick);
            SearchCommand = new Prism.Commands.DelegateCommand(OnSearchData);
        }

        internal void OnDoubleClick(object sender, RadRoutedEventArgs e)
        {
            Close(true);
        }

        private void OnDoubleClick(object obj)
        {
            int s = 0;
        }

        [InjectionMethod]
        public async Task Init()
        {
          
            _items = new ObservableCollection<PaymentTypeItem>();
            int count = 0;
            // var items = await _itemDefineService.GetAllItemDefines("","",PageIndex,PageSize);
            //   count = items.TotalCount;
            // this.Items = new ObservableCollection<ItemDefine>(datas.Items);
            Initializer();
            await Task.CompletedTask;
        }
        private ObservableCollection<PaymentTypeItem> _items;
        public ObservableCollection<PaymentTypeItem> Items { get => _items; set => SetProperty(ref _items, value); }
        private PaymentTypeItem _selectedItem;
        public PaymentTypeItem SelectedItem { get => _selectedItem; set => SetProperty(ref _selectedItem, value); }

        private async void Initializer(int pageIndex = 0, int pageSize = 20)
        {
            IsBusy = true;
            Items.Clear();
            //if (BeginTime != null)
            //{
            //    if (BeginTime == EndTime)
            //    {
            //        EndTime = BeginTime.Value.AddDays(1);
            //    }

            //}
            //var result = await _dataService.GetWaterListAsync(DeviceId, BeginTime, EndTime, pageIndex: PageIndex, pageSize: PageSize);
            long dataCount = 0;
            var items = await _paymenttypeService.GetPageItems("", "", PageIndex, PageSize);

            TotalCount = items.totalCount;
            Items.AddRange(items.datas);

            IsBusy = false;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        private void OnSearchData()
        {
            TotalCount = 0;
            PageIndex = 0;
            Initializer(PageIndex, PageSize);
        }

        private void OnPageChanged(Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            Initializer(PageIndex, PageSize);
        }
        private string _itemNo;
        public string ItemNo
        {
            get { return _itemNo; }
            set { SetProperty(ref _itemNo, value); }
        }
        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set { SetProperty(ref _itemName, value); }
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
        private int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set { SetProperty(ref _pageSize, value); }
        }
        private int _pageIndex;
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
        private void OnCancel()
        {
            Close(false);
        }

        private void OnAddItem()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("请选择支付类型");
                return;
            }
            Close(true);
        }


        public ICommand SaveCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }
        public DelegateCommand<object> DoubleClick { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }
        public Action<bool?> Close { get; internal set; }


    }

}
