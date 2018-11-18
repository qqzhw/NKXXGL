
using ICIMS.Model.BaseData;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICIMS.Model.BusinessManages
{
    public class Budget :BindableBase
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
        public int Status { get => _status; set => SetProperty(ref _status, value); }

        /// <summary>
        /// 预算年度
        /// </summary>
        private int _year;
        public int Year { get => _year; set => SetProperty(ref _year, value); }

        private string _budgetNo;
        public string BudgetNo { get => _budgetNo; set => SetProperty(ref _budgetNo, value); }
        private string _budgetName;
        public string BudgetName { get => _budgetName; set => SetProperty(ref _budgetName, value); }

        private string _according;
        public string According { get => _according; set => SetProperty(ref _according, value); }
        /// <summary>
        /// 测算标准
        /// </summary>
        private string _measureStandard;
        public string MeasureStandard { get => _measureStandard; set => SetProperty(ref _measureStandard, value); }
        /// <summary>
        /// 预算金额
        /// </summary>
        private decimal _budgetamount;
        public decimal BudgetAmount { get => _budgetamount; set => SetProperty(ref _budgetamount, value); }

        private long _unitId;
        public long UnitId { get => _unitId; set => SetProperty(ref _unitId, value); }
        private string _unitName;
        public string  UnitName { get => _unitName; set => SetProperty(ref _unitName, value); }
        /// <summary>
        /// 预算分类ID
        /// </summary>
        private int _yscategoryId;
        public int YsCategoryId { get => _yscategoryId; set => SetProperty(ref _yscategoryId, value); }

        private YsCategoryItem _ysCategoryItem;
        public virtual YsCategoryItem YsCategory { get => _ysCategoryItem; set => SetProperty(ref _ysCategoryItem, value); }
        /// <summary>
        /// 采购分类ID
        /// </summary>
        private int _buyCategoryId;
        public int BuyCategoryId { get => _buyCategoryId; set => SetProperty(ref _buyCategoryId, value); }
        private BuyCategory _buyCategory;
        public virtual BuyCategory BuyCategory { get => _buyCategory; set => SetProperty(ref _buyCategory, value); }
        /// <summary>
        /// 功能科目ID
        /// </summary>
        private int _subjectId;
        public int SubjectId { get => _subjectId; set => SetProperty(ref _subjectId, value); }
        private SubjectItem _subjectItem;
        public  SubjectItem Subject { get => _subjectItem; set => SetProperty(ref _subjectItem, value); }

        private decimal _oneamount;
        public decimal OneAmount { get => _oneamount; set => SetProperty(ref _oneamount, value); }
        private decimal _secondamount;
        public decimal SecondAmount { get => _secondamount; set => SetProperty(ref _secondamount, value); }
        private decimal _initamount;
        public decimal InitAmount { get => _initamount; set => SetProperty(ref _initamount, value); }
        private decimal _middleamount;
        public decimal MiddleAmount { get => _middleamount; set => SetProperty(ref _middleamount, value); }
        private bool _ismiddle;
        public bool IsMiddle { get => _ismiddle; set => SetProperty(ref _ismiddle, value); }
        private decimal _middleReplyAmount;
        public decimal MiddleReplyAmount { get => _middleReplyAmount; set => SetProperty(ref _middleReplyAmount, value); }
        private string _remark;
        public string Remark { get => _remark; set => SetProperty(ref _remark, value); }
    }
}
