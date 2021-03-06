﻿
using ICIMS.Model.BaseData;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ICIMS.Model.BusinessManages
{
    public class PayAudit : BindableBase
    {
        public PayAudit()
        {
            _payAuditDetails = new ObservableCollection<PayAuditDetail>();
        }
        public int Id { get; set; }
        public int? TenantId { get; set; }

        public string SysGuid { get; set; }

        private int _status;
        public int Status
        {
            get => _status; set
            {
                SetProperty(ref _status, value); 
                switch (_status)
                {
                    case 0:
                        StatusText = "制单";
                        StatusColor = "#FFFF00";
                        break;
                    case 1:
                        StatusText = "审核中";
                        StatusColor = "#3cb371";
                        break;
                    case 2:
                        StatusText = "驳回";
                        StatusColor = "#ff8c00";
                        break;
                    case 3:
                        StatusText = "结审";
                        StatusColor = "#f08080";
                        break;
                    default:
                        StatusText = "制单";
                        StatusColor ="#FFFF00";
                        break;
                }
            }
        }
        private string _statuscolor;
        public string StatusColor
        {
            get => _statuscolor; set => SetProperty(ref _statuscolor, value);
        }
        /// <summary>
        /// 部门ID
        /// </summary>
        private long? _unitId;
        public long? UnitId { get => _unitId; set => SetProperty(ref _unitId, value); }

        /// <summary>
        /// 合同ID
        /// </summary>
        private int _contractId;
        public int ContrctId { get => _contractId; set => SetProperty(ref _contractId, value); }

        /// <summary>
        /// 立项登记ID
        /// </summary>
        private int _itemdefineId;
        public int ItemDefineId { get => _itemdefineId; set => SetProperty(ref _itemdefineId, value); }
        /// <summary>
        ///支付类型ID
        /// </summary>
        private int paymenttypeid;
        public int PaymentTypeId { get => paymenttypeid; set => SetProperty(ref paymenttypeid, value); }
        /// <summary>
        /// 支付编号
        /// </summary>
        private string _paymentNo;
        public string PaymentNo { get => _paymentNo; set => SetProperty(ref _paymentNo, value); }
        private string _paymentName;
        public string PaymentName { get => _paymentName; set => SetProperty(ref _paymentName, value); }

        private string _paymentmethod;
        public string PaymentMethod { get => _paymentmethod; set => SetProperty(ref _paymentmethod, value); }

        private decimal _payamount;
        public decimal PayAmount { get => _payamount; set => SetProperty(ref _payamount, value); }
        /// <summary>
        /// 支付比例
        /// </summary>
        private int _paymentper;
        public int PaymentPer { get => _paymentper; set => SetProperty(ref _paymentper, value); }
        /// <summary>
        /// 支付次数
        /// </summary>
        private int _paymentcount;
        public int PaymentCount { get => _paymentcount; set => SetProperty(ref _paymentcount, value); }

        /// <summary>
        /// 项目预算总额
        /// </summary>
        private decimal _itemysttotalamount;
        public decimal ItemYsTotalAmount { get => _itemysttotalamount; set => SetProperty(ref _itemysttotalamount, value); }

        /// <summary>
        /// 项目总额
        /// </summary>
        private decimal _itemtotalamount;
        public decimal ItemTotalAmount { get => _itemtotalamount; set => SetProperty(ref _itemtotalamount, value); }

        /// <summary>
        /// 合同总额
        /// </summary>
        private decimal _contractTotalAmount;
        public decimal ContractTotalAmount { get => _contractTotalAmount; set => SetProperty(ref _contractTotalAmount, value); }
        private string _remark;
        public string Remark { get => _remark; set => SetProperty(ref _remark, value); }

        private decimal _initpayAmount;
        public decimal InitPayAmount { get => _initpayAmount; set => SetProperty(ref _initpayAmount, value); }

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

        private ObservableCollection<PayAuditDetail> _payAuditDetails;
        public ObservableCollection<PayAuditDetail> PayAuditDetails
        {
            get { return _payAuditDetails; }
            set { SetProperty(ref _payAuditDetails, value); }
        }
        private string _payDetail;
        public string PayDetail
        {
            get { return _payDetail; }
            set { SetProperty(ref _payDetail, value); }
        }
        private string _statusText;
        public string StatusText
        {
            get
            { 
                return _statusText;
            }
            set { SetProperty(ref _statusText, value); }
        }
        #region 导航属性 
        private Contract _contract;
        public virtual Contract Contract { get => _contract; set => SetProperty(ref _contract, value); }
        private PaymentTypeItem _paymentType;
        public virtual PaymentTypeItem PaymentType { get => _paymentType; set => SetProperty(ref _paymentType, value); }
        #endregion

    }
}
