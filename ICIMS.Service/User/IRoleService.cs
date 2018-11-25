using ICIMS.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.User
{
    public interface IRoleService
    {
       Task<List<RoleModel>> GetAllRole(int SkipCount = 0, int MaxResultCount = int.MaxValue);
        Task<List<PermissionModel>> GetAllPermissions();
        Task<RoleModel> Update(RoleModel role);
        Task<RoleModel> Create(RoleModel item);
        Task Delete(int id);
    }
}
