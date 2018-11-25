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
        Task<List<UserModel>> GetAll(int SkipCount=0,int MaxResultCount = int.MaxValue);
        Task<UserModel> Create(UserModel user);
        Task<UserModel> Update(UserModel user);
        Task Delete(int Id);

    }
}
