using ICIMS.Core.Events;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Modules.BusinessManages.Views;
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
using System.Windows.Input;
using Unity;

namespace ICIMS.Modules.BusinessManages.ViewModels
{
   public class ReViewDefineViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private readonly IUnityContainer _unityContainer;
        private readonly IRegionManager _regionManager;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public ReViewDefineViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _unityContainer = unityContainer;
            _regionManager = regionManager;
            _title = "评审登记";
            AddCommand = new DelegateCommand(OnAddItem);
            EditCommand = new DelegateCommand<object>(OnEditItem);
            DeleteCommand = new DelegateCommand<object>(OnDelete);
            RefreshCommand = new DelegateCommand(OnRefresh);
        }

        private void OnDelete(object obj)
        {
             
        }

        private void OnRefresh()
        { 
        }

        private void OnEditItem(object obj)
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("Id", 1);
            var region = _regionManager.Regions["MainRegion"];
            _regionManager.RequestNavigate("MainRegion", "ReViewDefineEditView", navigationCallback, navigationParameters);
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
            var view = _unityContainer.Resolve<ReViewDefineEditView>();
            var notification = new Notification()
            {
                Content = view,// (new ParameterOverride("name", "")), 
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) => {
                int s = 0;
            });
           // view.BindAction(notification.Finish);

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
    }
   
     
}
