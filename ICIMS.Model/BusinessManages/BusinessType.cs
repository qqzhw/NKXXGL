 
using System;
using System.Collections.Generic;
using System.Text;

namespace ICIMS.Model.BusinessManages
{
    public class BusinessType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public int? TenantId { get; set ; }
        public bool IsDeleted { get ; set ; }
    }
}
