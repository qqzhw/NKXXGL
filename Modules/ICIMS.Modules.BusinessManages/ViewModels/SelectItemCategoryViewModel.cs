using ICIMS.Model.BaseData;
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
    public class SelectItemCategoryViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IItemCategoryService _itemCategoryService;
        private readonly IItemDefineService _itemDefineService;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public SelectItemCategoryViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, IRegionManager regionManager, IItemCategoryService itemCategoryService, IItemDefineService itemDefineService)
        {
            _eventAggregator = eventAggregator;
            _container = unityContainer;
            _regionManager = regionManager;
            _itemCategoryService = itemCategoryService;
            _title = "项目分类";
            SaveCommand = new DelegateCommand(OnAddItem); 
            CancelCommand = new DelegateCommand(OnCancel);
            DoubleClick = new DelegateCommand<object>(OnDoubleClick);
            _itemDefineService = itemDefineService;
           
        }

        internal void OnDoubleClick(object sender, RadRoutedEventArgs e)
        {
            //_itemDefineService.GetAllItemDefines();
            //双击事件
            Close(true);
        }

        private void OnDoubleClick(object obj)
        {
            int s = 0;
        }

        [InjectionMethod]
        public async void Init()
        {
            _title = "项目分类";
            this.Items = new ObservableCollection<ItemCategoryItem>();
            List<ItemCategoryItem> datas = await _itemCategoryService.GetPaged();
            foreach (var data in datas)
            {
                if (data.GroupNo != data.No)
                {
                    data.Parent = datas.FirstOrDefault(a => a.No == data.GroupNo);
                    data.Parent.Children.Add(data);
                }
                else
                {
                    this.Items.Add(data);
                }
            }
        }
        public ObservableCollection<ItemCategoryItem> Items { get; set; }
        private ItemCategoryItem _selectedItem;
        public ItemCategoryItem SelectedItem { get => _selectedItem; set => SetProperty(ref _selectedItem, value); }
         

        private void OnCancel()
        {
            Close(false);
        } 
        
        private void OnAddItem()
        {
            if (SelectedItem==null)
            {
                MessageBox.Show("请选择项目分类");
                return; 
            }
            Close(true);
        }
         

        public ICommand SaveCommand { get; protected set; } 
        public ICommand CancelCommand { get; protected set; }
        public DelegateCommand<object> DoubleClick { get; private set; }
        public Action<bool?> Close { get; internal set; }
         
     
    }
    
}
