using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BusinessManages
{
    public class PayAuditDetail : BindableBase
    {
        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        /// <summary>
        /// 资金来源名称
        /// </summary>
        private string _fundname;
        public string FundName { get => _fundname; set => SetProperty(ref _fundname, value); }

        private string _remark;
        public string Remark { get => _remark; set => SetProperty(ref _remark, value); }

        private bool _isdelete;
        public bool IsDeleted { get => _isdelete; set => SetProperty(ref _isdelete, value); }

        private decimal _amount;
        public decimal Amount { get => _amount; set => SetProperty(ref _amount, value); }
         
        private DateTime _creationTime;
        public DateTime CreationTime { get => _creationTime; set => SetProperty(ref _creationTime, value); }
        private int _payauditId;
        public int PayAuditId { get => _payauditId; set => SetProperty(ref _payauditId, value); }

        private PayAudit _payaudit;
        public virtual PayAudit PayAudit { get => _payaudit; set => SetProperty(ref _payaudit, value); }
    }
}
