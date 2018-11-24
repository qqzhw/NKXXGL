using ICIMS.Model.User;
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
        UserModel LoginAsync(string userName, string password,string tenantName="");
        Task<string> GetCurrentLoginInfo();
        Task<string> GetUserInfoAsync(long userId);
        Task<ResultData<List<RoleModel>>> GetUserRoles();
        
    }
}
