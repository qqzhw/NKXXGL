using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BusinessManages
{
    public class BuinessAudit : BindableBase
    {

        /// <summary>
       /// Name
       /// </summary>
        public string Name { get; set; }
         

        /// <summary>
        /// DisplayOrder
        /// </summary>
        public int DisplayOrder { get; set; }
         

        /// <summary>
        /// TenantId
        /// </summary>
        public int? TenantId { get; set; }
         
         
        public long RoleId { get; set; }
        public string RoleName { get; set; }


     
        public int? BuinessTypeId { get; set; }

        public string BuinessTypeName { get; set; }
 
         

        /// <summary>
        /// BuinessType
        /// </summary>
        public virtual BusinessType BuinessType { get; set; }
 
    }
}
