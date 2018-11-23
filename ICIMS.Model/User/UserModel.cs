using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.User
{
    public class UserModel: BindableBase
    {
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

        private long[] _roleIds;
        public long[] RoleIds { get => _roleIds; set => SetProperty(ref _roleIds, value); }

        private List<string> _rolesName;
        public List<string> RolesName { get => _rolesName; set => SetProperty(ref _rolesName, value); }


    }
}
