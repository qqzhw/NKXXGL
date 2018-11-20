using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BusinessManages
{
    public class ItemDefineList : BindableBase
    {


        /// <summary>
        /// TenantId
        /// </summary>
        public int? TenantId { get; set; }

        public string SysGuid { get; set; }

        /// <summary>
        /// Status
        /// </summary> 
        public int Status { get; set; }

        public long UnitId { get; set; }
        public string UnitName { get; set; }



        public int? BudgetId { get; set; }
        public string BudgetNo { get; set; }
        public string BudgetName { get; set; }
        /// <summary>
        /// ItemDocNo
        /// </summary>
        public string ItemDocNo { get; set; }

        /// <summary>
        /// DefineDate
        /// </summary>
        public DateTime DefineDate { get; set; }

        public string ItemNo { get; set; }

        public string ItemName { get; set; }



        /// <summary>
        /// ItemType
        /// </summary>
        public int ItemCategoryId { get; set; }

        public string ItemCategoryName { get; set; }

        public decimal DefineAmount { get; set; }

        /// <summary>
        /// ItemAddress
        /// </summary>
        public string ItemAddress { get; set; }



        /// <summary>
        /// ItemDescription
        /// </summary>
        public string ItemDescription { get; set; }



        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }



        /// <summary>
        /// IsFinal
        /// </summary>
        public bool IsFinal { get; set; }



        /// <summary>
        /// IsAudit
        /// </summary>
        public bool IsAudit { get; set; }

        public long? CreatorUserId { get; set; }
        public string CreatorName { get; set; }

        /// <summary>
        /// AuditDate
        /// </summary>
        public DateTime? AuditDate { get; set; }

        /// <summary>
        /// AuditUserId
        /// </summary>
        public long? AuditUserId { get; set; }
        /// <summary>
        /// AuditUser
        /// </summary>
        public string AuditUserName { get; set; }

    }
}
