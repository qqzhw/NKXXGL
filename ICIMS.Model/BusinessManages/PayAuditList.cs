using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BusinessManages
{
    public class PayAuditList:BindableBase
    {
        private PayAudit _payaudit;
        private string _itemNo;
        private decimal _defineAmount;
        private string  _itemDefineName;
        private string _unitName;
        private string _creatoruserName;
        private string _auditusername;
        private string _paymenttypename;
        private string _contractname;
        private decimal _contractamount;
        private bool _isdelete;
        private long? creatoruserid;
        public DateTime _creationTime;
        private string _vendorname;
        private string _accountname;
        private string _openback;
        private string _linkphone; 
        public PayAudit PayAudit { get => _payaudit; set => SetProperty(ref _payaudit, value); }
        public string ItemNo { get => _itemNo; set => SetProperty(ref _itemNo, value); }
        public decimal DefineAmount { get => _defineAmount; set => SetProperty(ref _defineAmount, value); }//项目金额
        public string ItemDefineName { get => _itemDefineName; set => SetProperty(ref _itemDefineName, value); }
        public string UnitName { get => _unitName; set => SetProperty(ref _unitName, value); }
        public string CreatorUserName { get => _creatoruserName; set => SetProperty(ref _creatoruserName, value); }
        public string AuditUserName { get => _auditusername; set => SetProperty(ref _auditusername, value); }
        public string PaymentTypeName { get => _paymenttypename; set => SetProperty(ref _paymenttypename, value); }//支付类型
        public string ContractName { get => _contractname; set => SetProperty(ref _contractname, value); }
        public decimal ContractAmount { get => _contractamount; set => SetProperty(ref _contractamount, value); }//合同金额
        public bool IsDeleted { get => _isdelete; set => SetProperty(ref _isdelete, value); }

        public long? CreatorUserId { get => creatoruserid; set => SetProperty(ref creatoruserid, value); }
        public DateTime CreationTime { get => _creationTime; set => SetProperty(ref _creationTime, value); }


        public string VendorName { get => _vendorname; set => SetProperty(ref _vendorname, value); } //承建单位
        public string AccountName { get => _accountname; set => SetProperty(ref _accountname, value); }//账户
        public string OpenBank { get => _openback; set => SetProperty(ref _openback, value); }//开户行
        public string LinkPhone { get => _linkphone; set => SetProperty(ref _linkphone, value); }//联系人电话

    }
}
