 
using System;
using System.Collections.Generic;
 
using System.Text;

namespace ICIMS.Model.BusinessManages
{
    /// <summary>
    /// 评审登记
    /// </summary>
    public class ReViewDefine
    {
        public int Id { get; set; }
        public int? TenantId { get; set; }

        public string SysGuid { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 立项登记 ID
        /// </summary>
        public int ItemDefineId { get; set; }

       
        /// <summary>
        /// 评审编号
        /// </summary>
        public string ReViewNo { get; set; }

        /// <summary>
        /// 评审名称
        /// </summary>
        public string ReViewName { get; set; }

        /// <summary>
        /// 评审文号
        /// </summary>
        public string ReViewDocNo { get; set; }

        /// <summary>
        /// 评审金额
        /// </summary>
        public decimal ReViewAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 结审日期
        /// </summary>
        public DateTime AuditDate { get; set; }

        public long? AuditUserId { get; set; }       
        public virtual User AuditUser { get; set; }

        #region 导航属性
        public virtual ItemDefine ItemDefine { get; set; }
        #endregion
    }
}
