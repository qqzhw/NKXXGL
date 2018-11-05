using ICIMS.Core.Events;
using ICIMS.Core.Interactivity;
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Modules.SystemAdmin.Views;
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
using Unity.Attributes;

namespace ICIMS.Modules.SystemAdmin.ViewModels
{
   public partial class UsersViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private IUnityContainer _unityContainer;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public UsersViewModel(IUnityContainer unityContainer, IEventAggregator eventAggregator)
        {
            _unityContainer = unityContainer;
            _eventAggregator = eventAggregator;
            _title = "ViewB";
        }

        [InjectionMethod]
        public void Init()
        {
            ShowCommand = new DelegateCommand<object>(OnMenuCommand);
        }
        public ICommand ShowCommand { get; private set; }
        private void OnMenuCommand(object obj)
        {

            var notification = new Notification()
            {
                Title = "测试",
                Content = _unityContainer.Resolve<UsersView>(),// (new ParameterOverride("name", "")),
            };
            PopupWindows.NotificationRequest.Raise(notification, (callback) => {

            });
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

        //whether the tab is selected;
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }

        //To do:define the UI for tabcontrol's content;
        public virtual UserControl View { get; set; }

        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(() =>
            {
                if (ConfirmToClose())
                {
                    _eventAggregator.GetEvent<TabCloseEvent>().Publish(View);
                }
            }));
        protected virtual bool ConfirmToClose()
        {
            return true;
        }
        #endregion
    }
     
}
