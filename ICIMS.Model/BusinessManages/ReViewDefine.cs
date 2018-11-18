
using Prism.Mvvm;
using System;
using System.Collections.Generic;
 
using System.Text;

namespace ICIMS.Model.BusinessManages
{
    /// <summary>
    /// 评审登记
    /// </summary>
    public class ReViewDefine: BindableBase
    {
        public int Id { get; set; }
        public int? TenantId { get; set; }

        public string SysGuid { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        private int _status;
        public int Status { get => _status; set => SetProperty(ref _status, value); }
        /// <summary>
        /// 立项登记 ID
        /// </summary>
        private int _itemDefineId;
        public int ItemDefineId { get => _itemDefineId; set => SetProperty(ref _itemDefineId, value); }


        /// <summary>
        /// 评审编号
        /// </summary>
        private string _reviewNo;
        public string ReViewNo { get => _reviewNo; set => SetProperty(ref _reviewNo, value); }

        /// <summary>
        /// 评审名称
        /// </summary>
        private string _reviewName;
        public string ReViewName { get => _reviewName; set => SetProperty(ref _reviewName, value); }

        /// <summary>
        /// 评审文号
        /// </summary>
        private string _reviewdocNo;
        public string ReViewDocNo { get => _reviewdocNo; set => SetProperty(ref _reviewdocNo, value); }

        /// <summary>
        /// 评审金额
        /// </summary>
        private decimal _reviewAmount;
        public decimal ReViewAmount { get => _reviewAmount; set => SetProperty(ref _reviewAmount, value); }

        /// <summary>
        /// 备注
        /// </summary>
        private string _remark;
        public string Remark { get => _remark; set => SetProperty(ref _remark, value); }

        /// <summary>
        /// 是否评审
        /// </summary>
        private bool _isaudit;
        public bool IsAudit { get => _isaudit; set => SetProperty(ref _isaudit, value); }

        /// <summary>
        /// 结审日期
        /// </summary>
        private DateTime? _auditDate;
        public DateTime? AuditDate { get => _auditDate; set => SetProperty(ref _auditDate, value); }

        private long? _audituserId;
        public long? AuditUserId { get => _audituserId; set => SetProperty(ref _audituserId, value); }

        private string _audituserName;
        public string AuditUserName { get => _audituserName; set => SetProperty(ref _audituserName, value); }

        

        #region 导航属性
        private ItemDefine _itemdefine;
        public ItemDefine ItemDefine { get => _itemdefine; set => SetProperty(ref _itemdefine, value); }
        #endregion
    }
}
