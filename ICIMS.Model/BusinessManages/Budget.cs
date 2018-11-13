
using ICIMS.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICIMS.Model.BusinessManages
{
    public class Budget 
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
        public int Status { get; set; }

        /// <summary>
        /// 预算年度
        /// </summary>
        public int Year { get; set; }

        public string BudgetNo { get; set; }
        public string BudgetName { get; set; }

        public string According { get; set; }
        /// <summary>
        /// 测算标准
        /// </summary>
        public string MeasureStandard { get; set; }
        /// <summary>
        /// 预算金额
        /// </summary>
        public decimal BudgetAmount { get; set; }

        public long UnitId { get; set; }
        public virtual OrganizationUnit Unit { get; set; }
        /// <summary>
        /// 预算分类ID
        /// </summary>
        public int YsCategoryId { get; set; }
        public virtual YsCategoryItem YsCategory { get; set; }
        /// <summary>
        /// 采购分类ID
        /// </summary>
	    public int BuyCategoryId { get; set; }
        public virtual BuyCategory BuyCategory { get; set; }
        /// <summary>
        /// 功能科目ID
        /// </summary>
        public int SubjectId { get; set; }
        public virtual SubjectItem FunctionSubject { get; set; }

        public decimal OneAmount { get; set; }
        public decimal SecondAmount { get; set; }
        public decimal InitAmount { get; set; }
        public decimal MiddleAmount { get; set; }
        public bool IsMiddle { get; set; }
        public decimal MiddleReplyAmount { get; set; }
        public string Remark { get; set; }
    }
}
