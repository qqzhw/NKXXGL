using ICIMS.Model.BaseData;
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
using System.Windows.Input;
using Telerik.Windows;
using Unity;
using Unity.Attributes;

namespace ICIMS.Modules.BusinessManages.ViewModels
{
    public class SelectedDocumentTypeModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IDocumentTypeService _documentTypeService;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public SelectedDocumentTypeModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, IRegionManager regionManager, IDocumentTypeService documentTypeService)
        {
            _eventAggregator = eventAggregator;
            _container = unityContainer;
            _regionManager = regionManager;
            _documentTypeService = documentTypeService;
            _title = "文档分类";
            SaveCommand = new DelegateCommand(OnAddItem);
            CancelCommand = new DelegateCommand(OnCancel);
            DoubleClick = new DelegateCommand<object>(OnDoubleClick);


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
            this.Items = new ObservableCollection<DocumentTypeItem>();
            var datas = await _documentTypeService.GetPageItems();
            foreach (var data in datas.datas)
            {
                if (data.GroupNo != data.No)
                {
                    data.Parent = datas.datas.FirstOrDefault(a => a.No == data.GroupNo);
                    data.Parent.Children.Add(data);
                }
                else
                {
                    this.Items.Add(data);
                }
            }
        }
        public ObservableCollection<DocumentTypeItem> Items { get; set; }
        private DocumentTypeItem _selectedItem;
        public DocumentTypeItem SelectedItem { get => _selectedItem; set => SetProperty(ref _selectedItem, value); }


        private void OnCancel()
        {
            Close(false);
        }

        private void OnAddItem()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("请选择文档分类");
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
