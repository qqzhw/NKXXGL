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
    public class FundViewModel : BindableBase, INavigationAware
    {
         

       
        private readonly IEventAggregator _eventAggregator;
        private readonly IFundFromService _fundFromService;
        public FundViewModel(IEventAggregator eventAggregator,IFundFromService fundFromService)
        {
            _eventAggregator = eventAggregator;
            _fundFromService = fundFromService;
        }
        [InjectionMethod]
        public void Init()
        {
            _title = "资金来源";
            this.Items = new ObservableCollection<FundItem>();
            List<FundItem> datas = _fundFromService.GetPageItems().Result;
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
        public ObservableCollection<FundItem> Items { get; set; }
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
