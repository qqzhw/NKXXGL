using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BaseData
{
    public class VendorItem : BindableBase
    {
        private string _no;
        private string _email;
        private string _name;
        private string _address;
        private string _linkPerson;
        private string _linkPhone;
        private string _accountName;
        private string _openBank;
        private string _remark;
        private int? _tenantId;
        private bool _published;
        private int _displayOrder;

        public int Id { get; set; }

        public string No { get => _no; set => SetProperty(ref _no, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string Address { get => _address; set => SetProperty(ref _address, value); }
        public string LinkPerson { get => _linkPerson; set => SetProperty(ref _linkPerson, value); }
        public string LinkPhone { get => _linkPhone; set => SetProperty(ref _linkPhone, value); }
        public string AccountName { get => _accountName; set => SetProperty(ref _accountName, value); }
        public string OpenBank { get => _openBank; set => SetProperty(ref _openBank, value); }
        public string Remark { get => _remark; set => SetProperty(ref _remark, value); }
        public int? TenantId { get => _tenantId; set => SetProperty(ref _tenantId, value); }
        public bool Published { get => _published; set => SetProperty(ref _published, value); }

        public int DisplayOrder { get => _displayOrder; set => SetProperty(ref _displayOrder, value); }
    }
}
