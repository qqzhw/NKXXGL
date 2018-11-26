﻿using ICIMS.Model.BaseData;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.User
{
    public class UserModel : BindableBase
    {
        public UserModel()
        {
            _roleIds = new List<int>();
            _rolesNames = new List<string>();
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


        private long? _unitId;
        public long? UnitId { get => _unitId; set => SetProperty(ref _unitId, value); }

        private string _unitName;
        public string UnitName { get => _unitName; set => SetProperty(ref _unitName, value); }

        private List<int> _roleIds;
        public List<int> RoleIds { get => _roleIds; set => SetProperty(ref _roleIds, value); }

        private List<string> _rolesNames = new List<string>();
        public List<string> RolesNames { get => _rolesNames; set => SetProperty(ref _rolesNames, value); }
        public List<OrganizationUnitItem> Units { get => _units; set => SetProperty(ref _units, value); }

        private List<OrganizationUnitItem> _units;

        public string RolesNameStr
        {
            get
            {
                return string.Join(",", RolesNames);
            }
        }

        public string EmailAddress { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }   

    }
}
