﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace ICIMS.Model.User
{
    public class RoleModel : BindableBase
    {
        public RoleModel()
        {
            _permissions = new ObservableCollection<string>();
        }
        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

       
        private string _displayname;
        public string DisplayName { get => _displayname; set => SetProperty(ref _displayname, value); }

        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        private bool _isStatic;
        public bool IsStatic { get => _isStatic; set => SetProperty(ref _isStatic, value); }

        public ObservableCollection<string> _permissions;
        public ObservableCollection<string> Permissions { get => _permissions; set =>SetProperty(ref _permissions, new ObservableCollection<string> (value.Where(a => !string.IsNullOrEmpty(a)))); }
        //private long _unitId;
        //public long UnitId { get => _unitId; set => SetProperty(ref _unitId, value); }

        //private string _unitName;
        //public string UnitName { get => _unitName; set => SetProperty(ref _unitName, value); }

        //private long[] _roleIds;
        //public long[] RoleIds { get => _roleIds; set => SetProperty(ref _roleIds, value); }

        //private long[] _roleName;
        //public long[] RoleName { get => _roleName; set => SetProperty(ref _roleName, value); } 
        // "name": "Admin",
        //"displayName": "管理员",
        //"normalizedName": "ADMIN",
        //"description": null,
        //"isStatic": true,
        //"permissions": [],
        //"id": 1

        public int No { get; set; }
        public bool IsChecked { get => _isChecked; set => SetProperty(ref _isChecked,value); }

        private bool _isChecked;
    }
    
}
