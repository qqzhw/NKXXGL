
using ICIMS.Client.Properties;
using ICIMS.Model.User;
using ICIMS.Service;
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
using Unity.Lifetime;

namespace ICIMS.Client.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private readonly IUserService _userSerice;
        public ICommand LoginCommand { get; private set; }
        public ICommand KeyDown { get; set; }
        public Action Close;
        public LoginViewModel(IUnityContainer container, IEventAggregator eventAggregator, IUserService userSerice)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _userSerice = userSerice;
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
            AppName = "内控信息化管理系统【V2.81216】";
            _tenancyName = "XJHJD";
            CurrentDate = DateTime.Now;
            //_appName = Settings.Default.AppName;
            _userName = Settings.Default.UserName;
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
        private DateTime _currentdate;
        public DateTime CurrentDate
        {
            get { return _currentdate; }
            set { SetProperty(ref _currentdate, value); }
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
            //  TenancyName = "Default";
            // string UserName = "admin";
            // string Password = "123qwe";        
            try
            {
                var user =await Task.Run(()=> { return _userSerice.LoginAsync(UserName, Password, TenancyName); });
                if (user==null)
                { 
                    MessageBox.Show("登录失败,用户不存在");
                    Islogining = false;
                    return;
                }
                var userInfo = await _userSerice.GetUserInfoById(user.Id);                
                userInfo.AccessToken = user.AccessToken;
                userInfo.UnitId = userInfo.Unit!=null?userInfo.Unit.Id:0;
                userInfo.UnitName = userInfo.Unit!=null?userInfo.Name:"";
                foreach (var role in userInfo.Roles)
                {
                    foreach (var item  in role.Permissions)
                    {
                        userInfo.Permissions.Add(item);
                    }
                }
                userInfo.CurrentDate = CurrentDate;
                _container.RegisterInstance(userInfo, new ContainerControlledLifetimeManager());
                //var roles = await _userSerice。.GetUserRoles();
                //foreach (var item in roles.Items)
                //{
                //    user.RoleIds.Add(item.Id);
                //    user.RoleDisplayNames.Add(item.DisplayName); 
                //    user.RoleNames.Add(item.Name);
                //}
                // UserModel = userInfo;
                // DisplayNames = userInfo.Roles.Select(o => o.DisplayName).FirstOrDefault();
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
                Islogining = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录失败"+ex.Message);
                Islogining = false;
                return;
            }
        
            Close?.Invoke();
        }
    }
}
