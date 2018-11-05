using System;
using System.Net;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Prism.Commands;
 
using Prism.Events;
using CommonServiceLocator;

namespace ICIMS.Client
{
	public abstract class TabModel : INotifyPropertyChanged
	{
		public TabModel()
		{
			
		}
		
        public event PropertyChangedEventHandler PropertyChanged;

        //whether the tab is selected;
        private bool isSelected;
        public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
			set
			{
				if (this.isSelected != value)
				{
					this.isSelected = value;
					PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("IsSelected"));
				}
			}
		}
        
        //To do:define the UI for tabcontrol's content;
        public abstract FrameworkElement View { get; }
        
        //To-do:define the text in TabItem;
        public abstract string Title { get; set; }
        
        //The command when clicking Close Button;
        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(() => {
                if (ConfirmToClose()) {
                    var aggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
                    if(aggregator != null) {
                       // aggregator.GetEvent<TabCloseEvent>()?.Publish(this);
                    }
                }
            }));

        //It can be overwrite in inherited class to ask for confirming to closing the tab;
        protected virtual bool ConfirmToClose() => true;
	}
}
