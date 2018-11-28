
using ICIMS.Model.BaseData;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
 
using System.Text;

namespace ICIMS.Model.BusinessManages
{
    /// <summary>
    /// 立项登记
    /// </summary>
    public class ItemDefine: BindableBase
    {
        public int Id { get; set; }
        public int? TenantId { get; set; }

        /// <summary>
        /// GUID
        /// </summary>
        public string SysGuid { get; set; }

        /// <summary>
        /// 立项状态
        /// </summary>
        private int _status;
        public int Status { get => _status; set { SetProperty(ref _status, value); OnPropertyChanged(() => StatusText); } }

        /// <summary>
        /// 部门ID
        /// </summary>
        private long _unitId;
        public long UnitId { get => _unitId; set => SetProperty(ref _unitId, value); }
        private string _unitName;
        public string UnitName { get => _unitName; set => SetProperty(ref _unitName, value); }

        private int? _budgetId;
        public int? BudgetId { get => _budgetId; set => SetProperty(ref _budgetId, value); }
        private string _budgetName;
        public string BudgetName { get => _budgetName; set => SetProperty(ref _budgetName, value); }
        /// <summary>
        /// 项目立项文号
        /// </summary>
        private string _itemDocNo;
        public string ItemDocNo { get => _itemDocNo; set => SetProperty(ref _itemDocNo, value); }

        /// <summary>
        /// 立项日期
        /// </summary>
        private DateTime _defineTime;
        public DateTime DefineDate { get => _defineTime; set => SetProperty(ref _defineTime, value); }

        /// <summary>
        /// 项目编号
        /// </summary>
        private string _itemNo;
        public string ItemNo { get => _itemNo; set => SetProperty(ref _itemNo, value); }
        /// <summary>
        /// 项目名称
        /// </summary>
        private string _itemName;
        public string ItemName { get => _itemName; set => SetProperty(ref _itemName, value); }

        private int _itemcategoryId;
        public int ItemCategoryId { get => _itemcategoryId; set => SetProperty(ref _itemcategoryId, value); }//项目类型 
        private string _itemcategoryName;
        public string ItemCategoryName { get => _itemcategoryName; set => SetProperty(ref _itemcategoryName, value); }
        /// <summary>
        /// 立项金额
        /// </summary>
        private decimal _defineAmount;
        public decimal DefineAmount { get => _defineAmount; set => SetProperty(ref _defineAmount, value); }

        private string _itemAddress;
        public string ItemAddress { get => _itemAddress; set => SetProperty(ref _itemAddress, value); }//项目地址   

        private string _itemdescription;
        public string ItemDescription { get => _itemdescription; set => SetProperty(ref _itemdescription, value); }//项目描述  

        private string _remark;
        public string Remark { get => _remark; set => SetProperty(ref _remark, value); }//备注 


        private string _statusText;
        public string StatusText
        {
            get
            {
                _statusText = "制单";
                switch (_status)
                {
                    case 1:
                        _statusText = "提交审核";
                        break;
                    case 2:
                        _statusText = "审核中";
                        break;
                    case 3:
                        _statusText = "已审核";
                        break;
                    default:
                        break;
                }
                return _statusText;
            }
        }

        /// <summary>
        /// 是否决算
        /// </summary>
        private bool _isfinal;
        public bool IsFinal { get => _isfinal; set => SetProperty(ref _isfinal, value); }

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
        public string AuditUserName{ get => _audituserName; set => SetProperty(ref _audituserName, value); }
    }
}
