using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BaseData
{
    public class VendorItem
    {
        public string No { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string LinkPerson { get; set; }
        public string LinkPhone { get; set; }
        public string AccountName { get; set; }
        public string OpenBank { get; set; }
        public string Remark { get; set; }
        public int? TenantId { get; set; }
        public bool Published { get; set; }

        public int DisplayOrder { get; set; }
    }
}
