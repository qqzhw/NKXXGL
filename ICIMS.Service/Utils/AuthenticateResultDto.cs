using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service
{
    public class AuthenticateResultDto
    {
        public string AccessToken { get; set; }
        public int UserId { get; set; }
        public string EncryptedAccessToken { get; set; }
        public int ExpireInSeconds { get; set; }

        public long? UnitId { get; set; }
        public string UnitName { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public List<string> RolesName{get;set;}
    }
}
