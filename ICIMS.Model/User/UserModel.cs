﻿using ICIMS.Model.BaseData;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace ICIMS.Model.User
{
    public class UserModel : BindableBase
    {
        public UserModel()
        {
            _roleNames = new List<string>();
            _roles = new ObservableCollection<RoleModel>();
        }
        public int? TenantId { get; set; }
        public string TenantName { get; set; }
        public string AccessToken { get; set; }
        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        private string _username;
        public string UserName { get => _username; set => SetProperty(ref _username, value); }

        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }


        private long _unitId;
        public long UnitId
        {
            get => _unitId; set => SetProperty(ref _unitId, value);
        }

        private string _unitName;
        public string UnitName
        {
            get => _unitName; set => SetProperty(ref _unitName, value);
        }

        public bool IsActive { get; set; } = true;
         
        private ObservableCollection<RoleModel> _roles;
        public ObservableCollection<RoleModel> Roles { get => _roles; set => SetProperty(ref _roles, value); }

        private List<string> _roleNames;
        public List<string> RoleNames
        {
            get => _roleNames; set => SetProperty(ref _roleNames, value);
        }
        private  UnitModel _unit;
        public UnitModel Unit { get => _unit; set => SetProperty(ref _unit, value); }
 
        

        private DateTime _lastLoginTime;
        public DateTime LastLoginTime { get => _lastLoginTime; set => SetProperty(ref _lastLoginTime, value); }
        public string EmailAddress { get; set; }
        public string Surname { get; set; }
        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }
    }
}
