using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model
{
    public class User
    {
        public int UserId { get; set; }
        public int? TenantId { get; set; }
        public string TenantName { get; set; }
        public string UserName { get; set; }

        public string Name { get; set; }

        public string AccessToken { get; set; }
        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }
    }
}
