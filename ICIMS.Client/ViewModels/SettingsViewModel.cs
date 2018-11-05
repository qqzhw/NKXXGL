
using CommonServiceLocator;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
 
using System.Windows.Input;
using Unity;

namespace ICIMS.Client.ViewModels
{
	public class SettingsViewModel: BindableBase
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly IUnityContainer _container; 
		private readonly IServiceLocator _serviceLocator; 
		public SettingsViewModel(IUnityContainer container, IEventAggregator eventAggregator, IServiceLocator serviceLocator)
		{
			_container = container;
			_eventAggregator = eventAggregator; 
			_serviceLocator = serviceLocator;
			SaveCommand = new DelegateCommand(OnSaveData);
			Initializer();
		}

		private void OnSaveData()
		{ 
            //Settings.Default.ServerUrl = ApiUrl;
            //Settings.Default.AppName = AppName;
            //Settings.Default.Save();
            //Settings.Default.Reload();
        }

		private void Initializer()
		{
           //_apiUrl = Settings.Default.ServerUrl;
           // _appName = Settings.Default.AppName;
        }
		#region 属性 
	 
		private string _ip;
		public string IP
		{
			get { return _ip; }
			set { SetProperty(ref _ip, value); }
		}
		private int _port;
		public int  Port
        {
			get { return _port; }
			set { SetProperty(ref _port, value); }
		}

        private string _apiUrl;
        public string ApiUrl
        {
            get { return _apiUrl; }
            set { SetProperty(ref _apiUrl, value); }
        }

        private string  _appName;
        public string AppName
        {
            get { return _appName; }
            set { SetProperty(ref _appName, value); }
        }
        public ICommand SaveCommand { get; private set; }


        

        #endregion
    }
}
