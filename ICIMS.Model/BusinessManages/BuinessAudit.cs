using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BusinessManages
{
    public class BusinessAudit : BindableBase
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


        public int? _businesstypeId;
        public int? BusinessTypeId { get => _businesstypeId; set => SetProperty(ref _businesstypeId, value); }

        private string _businessTypeName;
        public string BusinessTypeName { get => _businessTypeName; set => SetProperty(ref _businessTypeName, value); }

        private int _status;
        public int Status { get => _status; set => SetProperty(ref _status, value); }

        private string _statusName="";
        public string StatusName { get => _statusName; set => SetProperty(ref _statusName, value); }
        public bool IsChecked { get => _isChecked; set => SetProperty(ref _isChecked,value); }
        public int No { get => _no; set => SetProperty(ref _no,value); }

        private bool _isChecked;

        private int _no;

    }
}
