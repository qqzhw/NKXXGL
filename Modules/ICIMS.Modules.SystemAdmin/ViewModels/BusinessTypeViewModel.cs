using ICIMS.Model.BusinessManages;
using ICIMS.Model.User;
using ICIMS.Service.BusinessManages;
using ICIMS.Service.User;
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
using Unity;
using Unity.Attributes;

namespace ICIMS.Modules.SystemAdmin.ViewModels
{
    public class BusinessTypeViewModel : BindableBase, INavigationAware
    {
        private readonly IBusinessTypeService _businessTypeService;
        private readonly IBusinessAuditService _businessAuditService;
        private readonly IRoleService _roleService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private IUnityContainer _unityContainer;

        private BusinessType _selectedItem;
        private ObservableCollection<BusinessType> _items;
        private List<BusinessAudit> _businessAudits;
        private ObservableCollection<RoleModel> _roles;
        private string _name;

        public BusinessTypeViewModel(IEventAggregator eventAggregator,
             IRegionManager regionManager,
            IUnityContainer unityContainer,
            IBusinessTypeService businessTypeService,
            IBusinessAuditService businessAuditService,
            IRoleService roleService
            )
        {
            this._regionManager = regionManager;
            this._eventAggregator = eventAggregator;
            this._unityContainer = unityContainer;
            this._businessTypeService = businessTypeService;
            this._businessAuditService = businessAuditService;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public UserControl View { get; internal set; }
        public BusinessType SelectedItem
        {
            get => _selectedItem;
            set
            {
                if(value.Audits == null)
                {
                    value.Audits = new ObservableCollection<BusinessAudit>(this.BusinessAudits.Where(a => a.BuinessTypeId == value.Id));
                }
                SetProperty(ref _selectedItem, value);
            }
        }
        public ObservableCollection<BusinessType> Items { get => _items; set => SetProperty(ref _items, value); }
        public List<BusinessAudit> BusinessAudits { get => _businessAudits; set => SetProperty(ref _businessAudits, value); }
        public ObservableCollection<RoleModel> Roles { get => _roles; set => SetProperty(ref _roles, value); }

        [InjectionMethod]
        public async void Init()
        {
            _title = "业务类型";
            var roles = await _roleService.GetAllRole();
            this.Roles = new ObservableCollection<RoleModel>(roles);

            var businessTypes = (await _businessTypeService.GetAllBusinessTypes()).Items;
            this.Items = new ObservableCollection<BusinessType>(businessTypes);

            var auditTypes = await _businessAuditService.GetAllBusinessAudits();
            this.BusinessAudits = auditTypes.Items;

            this.SelectedItem = this.Items.FirstOrDefault();
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
    }
}
