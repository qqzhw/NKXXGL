using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.User
{
    public class RoleModel : BindableBase
    {
        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

       
        private string _displayname;
        public string DisplayName { get => _displayname; set => SetProperty(ref _displayname, value); }


        //private long _unitId;
        //public long UnitId { get => _unitId; set => SetProperty(ref _unitId, value); }

        //private string _unitName;
        //public string UnitName { get => _unitName; set => SetProperty(ref _unitName, value); }

        //private long[] _roleIds;
        //public long[] RoleIds { get => _roleIds; set => SetProperty(ref _roleIds, value); }

        //private long[] _roleName;
        //public long[] RoleName { get => _roleName; set => SetProperty(ref _roleName, value); } 
    }
    
}
