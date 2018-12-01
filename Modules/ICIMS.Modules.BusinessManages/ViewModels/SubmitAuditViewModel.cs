using ICIMS.Model.BusinessManages;
using ICIMS.Model.User;
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
    public class SubmitAuditViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IAuditMappingService _auditMappingService;
        private readonly IItemDefineService _itemDefineService;
        private readonly UserModel _userModel;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public SubmitAuditViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, IRegionManager regionManager, IAuditMappingService auditMappingService, IItemDefineService itemDefineService, AuditMapping data, UserModel userModel)
        {
            _eventAggregator = eventAggregator;
            _container = unityContainer;
            _regionManager = regionManager;
            _auditMappingService = auditMappingService;
            _title = "审核";
            _userModel = userModel;
            SaveCommand = new DelegateCommand(OnAddItem);
            CancelCommand = new DelegateCommand(OnCancel);
            DoubleClick = new DelegateCommand<object>(OnDoubleClick);
            _itemDefineService = itemDefineService;
            _auditMapping = data;
        }

        internal void OnDoubleClick(object sender, RadRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AuditMapping.AuditOpinion))
            {
                MessageBox.Show("请输入审核意见");
                return;
            }
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
             
        }
         
        private AuditMapping _auditMapping;
        public AuditMapping AuditMapping { get => _auditMapping; set => SetProperty(ref _auditMapping, value); }


        private void OnCancel()
        {
            Close(false);
        }

        private void OnAddItem()
        {
          
            Close(true);
        } 

        public ICommand SaveCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }
        public DelegateCommand<object> DoubleClick { get; private set; }
        public Action<bool?> Close { get; internal set; }


    }
    
   
}
