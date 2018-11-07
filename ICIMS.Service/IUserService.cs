using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service
{
    public interface IUserService
    {
        string Token { get; set; }
        void SetToken(string token);
        Task<string> Authenticate(string userName, string password);
        Task<string> GetCurrentLoginInfo();
        Task<string> GetUserInfoAsync(long userId);
        
        
    }
}
