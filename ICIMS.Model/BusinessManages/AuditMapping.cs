using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BusinessManages
{
    public class AuditMapping : BindableBase
    {

        /// <summary>
        /// Id
        /// </summary>
        public int? Id { get; set; }



        /// <summary>
        /// DisplayOrder
        /// </summary>
        public int DisplayOrder { get; set; }



        /// <summary>
        /// TenantId
        /// </summary>
        public int? TenantId { get; set; }



        public int BuinessAuditId { get; set; }


        public int BuinessTypeId { get; set; }


        public string BuinessTypeName { get; set; }

        public int ItemId { get; set; }



        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }



        /// <summary>
        /// AuditOpinion
        /// </summary>
        public string AuditOpinion { get; set; }



        /// <summary>
        /// CreatorUserId
        /// </summary>
        public long? CreatorUserId { get; set; }



        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }



        /// <summary>
        /// AuditTime
        /// </summary>
        public DateTime? AuditTime { get; set; }


    }
}