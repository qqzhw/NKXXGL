﻿
using ICIMS.Model.BaseData;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICIMS.Model.BusinessManages
{
    /// <summary>
    /// 合同类
    /// </summary>
   public class Contract: BindableBase
    {
        private int _id;
        public int Id { get => _status; set => SetProperty(ref _status, value); }
        
        /// <summary>
        /// GUID
        /// </summary>
        public string SysGuid { get; set; }

        private int _status;
        public int Status { get => _status; set => SetProperty(ref _status, value); }

        /// <summary>
        /// 立项登记ID
        /// </summary>
        private int _itemdefineId;
        public int ItemDefineId { get => _itemdefineId; set => SetProperty(ref _itemdefineId, value); }

        /// <summary>
        /// 部门ID
        /// </summary>
        private long _unitId;
        public long UnitId { get => _unitId; set => SetProperty(ref _unitId, value); }

        /// <summary>
        /// 合同类型
        /// </summary>
        private int _contractcategoryId;
        public int ContractCategoryId { get => _contractcategoryId; set => SetProperty(ref _contractcategoryId, value); }

        private DateTime _begintime;
        public  DateTime BeginTime { get => _begintime; set => SetProperty(ref _begintime, value); }
        private DateTime _endtime;
        public DateTime EndTime { get => _endtime; set => SetProperty(ref _endtime, value); }
        private string _contractno;
        public string ContractNo { get => _contractno; set => SetProperty(ref _contractno, value); }
        private string _contractname;
        public string ContractName { get => _contractname; set => SetProperty(ref _contractname, value); }

        private decimal _contractamount;
        public decimal ContractAmount { get => _contractamount; set => SetProperty(ref _contractamount, value); }

        private decimal _paidamount;
        public decimal PaidAmount { get => _paidamount; set => SetProperty(ref _paidamount, value); }

        /// <summary>
        /// 暂列金额
        /// </summary>
        private decimal _provisionalAmount;
        public decimal ProvisionalAmount { get => _provisionalAmount; set => SetProperty(ref _provisionalAmount, value); }

        private decimal _tax;
        public decimal Tax { get => _tax; set => SetProperty(ref _tax, value); }
        private decimal _taxamount;
        public decimal TaxAmount { get => _taxamount; set => SetProperty(ref _taxamount, value); }

        private DateTime _identifyDate;
        public DateTime IdentifyDate { get => _identifyDate; set => SetProperty(ref _identifyDate, value); }

        private int _vendorId;
        public int VendorId { get => _vendorId; set => SetProperty(ref _vendorId, value); }

        /// <summary>
        /// 是否结算
        /// </summary>
        private bool _isclearing;
        public bool IsClearing { get => _isclearing; set => SetProperty(ref _isclearing, value); }
        private decimal _clearingAmount;
        public decimal ClearingAmount { get => _clearingAmount; set => SetProperty(ref _clearingAmount, value); }

        /// <summary>
        /// 结算前支付比例
        /// </summary>
        private decimal _clearingper;
        public decimal ClearingPer { get => _clearingper; set => SetProperty(ref _clearingper, value); }

        /// <summary>
        /// 决算前支付比例
        /// </summary>
        private decimal _finalper;
        public decimal FinalPer { get => _finalper; set => SetProperty(ref _finalper, value); }


        /// <summary>
        /// 预警
        /// </summary>
        private string _warning;
        public  string Warining { get => _warning; set => SetProperty(ref _warning, value); }

        /// <summary>
        /// 预警日期
        /// </summary>
        private DateTime _warnTime;
        public  DateTime WariningDate { get => _warnTime; set => SetProperty(ref _warnTime, value); }

        /// <summary>
        /// 付款方式及比例
        /// </summary>
        private string _paymentMethod;
        public string PaymentMethod { get => _paymentMethod; set => SetProperty(ref _paymentMethod, value); }

        private string _remark;
        public string Remark { get => _remark; set => SetProperty(ref _remark, value); }

       
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
        //立项登记
        private ItemDefine _itemdefine;
        public ItemDefine ItemDefine { get => _itemdefine; set => SetProperty(ref _itemdefine, value); }

        //合同分类
        private ContractItem _contractCategory;
        public  ContractItem ContractCategory { get => _contractCategory; set => SetProperty(ref _contractCategory, value); }
        //供应商
        private VendorItem _vendor;
        public  VendorItem Vendor { get => _vendor; set => SetProperty(ref _vendor, value); }
        #endregion
    }
}
