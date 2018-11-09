using ICIMS.Core.Events;
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
using System.Windows.Controls;
using Unity.Attributes;

namespace ICIMS.Modules.BaseData.ViewModels
{
    public class OrganizationUnitViewModel : BindableBase, INavigationAware
    {

        private readonly IEventAggregator _eventAggregator;
        private readonly IOrganizationUnitService _service;
        private readonly IRegionManager _regionManager;
        public OrganizationUnitViewModel(IEventAggregator eventAggregator, IOrganizationUnitService service, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _service = service;
            this._regionManager = regionManager;
            eventAggregator.GetEvent<TabCloseEvent>().Subscribe(OnTabActive);
        }

        private void OnTabActive(UserControl view)
        {
            var region = _regionManager.Regions["MainRegion"];
            if (region.Views.Count() > 1)
            {
                if (view != null)
                {
                    region.Remove(view);
                }
            }
        }

        [InjectionMethod]
        public async void Init()
        {
            _title = "部门信息";
            this.Items = new ObservableCollection<OrganizationUnitItem>();
            List<OrganizationUnitItem> datas = await _service.GetPaged();
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
        public ObservableCollection<OrganizationUnitItem> Items { get; set; }
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
        #endregion
    }
}
