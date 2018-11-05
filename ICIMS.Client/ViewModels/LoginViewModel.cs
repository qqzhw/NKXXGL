
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace ICIMS.Client.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container; 
      
        public ICommand LoginCommand { get; private set; }
        public ICommand KeyDown { get; set; }
        public Action Close;
        public LoginViewModel(IUnityContainer container, IEventAggregator eventAggregator )
        {
            _container = container;
            _eventAggregator = eventAggregator;
            
            LoginCommand = new Prism.Commands.DelegateCommand(OnLogin);
            KeyDown = new DelegateCommand<KeyEventArgs>(OnKeyDown);
           Initializer();
        }

        private void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                if (string.IsNullOrEmpty(Password))
                {
                    MessageBox.Show("请输入密码！");
                    return;
                }
                OnLogin();
            }
        }

        private void Initializer()
        {
            AppName = "内控管理系统";
            //_userName = "qqzhw";
            //_password = "123qwe"; 
            //_appName = Settings.Default.AppName;
            //_userName = Settings.Default.UserName;
            if (!string.IsNullOrEmpty(_userName))
            {
                IsSave = true;
            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _password;
        public string  Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        private string _tenancyName;
        public string TenancyName
        {
            get { return _tenancyName; }
            set { SetProperty(ref _tenancyName, value); }
        }
        private bool _islogining = false;
        public bool  Islogining
        {
            get { return _islogining; }
            set { SetProperty(ref _islogining, value); }
        }
        private bool _issave = false;
        public bool IsSave
        {
            get { return _issave; }
            set { SetProperty(ref _issave, value); }
        }
        private string _appName;
        public string AppName
        {
            get { return _appName; }
            set { SetProperty(ref _appName, value); }
        }
        private async void OnLogin()
        {
            Islogining = true;
            //var result=await _dataService.Authenticate(UserName, Password);
            //var item = JsonConvert.DeserializeObject<ResultData<User>>(result).ToEntity();
            //if (item!=null)
            //{
            //    _container.RegisterInstance(typeof(User), item, new ContainerControlledLifetimeManager());
            //    if (item.TenantId==null)
            //    {
            //         Islogining = false;
            //        MessageBox.Show("账户不存在,请联系管理员！");
            //        return;
            //    }
            //    _dataService.SetToken(item.AccessToken);
            //    Close?.Invoke();
            //    if (IsSave)
            //    {
            //        //保存账号
            //        Settings.Default.UserName = UserName;
            //    }
            //    else
            //        Settings.Default.UserName = string.Empty;
            //    Settings.Default.Save();
            //}
            //else 
            //MessageBox.Show("登录失败！");
            Close?.Invoke();
        }
    }
}
