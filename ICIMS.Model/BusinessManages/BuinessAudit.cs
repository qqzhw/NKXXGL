using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BusinessManages
{
    public class BuinessAudit : BindableBase
    {
        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }
        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }


        /// <summary>
        /// DisplayOrder
        /// </summary>
        private int _displayOrder;
        public int DisplayOrder { get => _displayOrder; set => SetProperty(ref _displayOrder, value); }


        private long _roleId;
        public long RoleId { get => _roleId; set => SetProperty(ref _roleId, value); }
        private string _roleName;
        public string RoleName { get => _roleName; set => SetProperty(ref _roleName, value); }


        public int? _buinesstypeId;
        public int? BuinessTypeId { get => _buinesstypeId; set => SetProperty(ref _buinesstypeId, value); }

        private string _buinessTypeName;
        public string BuinessTypeName { get => _buinessTypeName; set => SetProperty(ref _buinessTypeName, value); }
 
       
 
    }
}
