using ICIMS.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.User
{
    public class RoleService : IRoleService
    {
        private IWebApiClient _webApiClient;
        private string _baseUrl = "api/services/app/Role";
        public RoleService(IWebApiClient webApiClient)
        {
            this._webApiClient = webApiClient;
        }
        public async Task<List<RoleModel>> GetAllRole(int SkipCount = 0, int MaxResultCount = int.MaxValue)
        {
            var para = new { MaxResultCount, SkipCount };
            var data = await _webApiClient.GetAsync<ResultData<List<RoleModel>>>($"{_webApiClient.BaseUrl}{_baseUrl}/GetAll", para);
            int no = 1;
            data.Items?.ForEach(a=>a.No = no++);

            return data.Items;
        }

        public async Task<List<PermissionModel>> GetAllPermissions()
        {
            var data = await _webApiClient.GetAsync<ResultData<List<PermissionModel>>>($"{_webApiClient.BaseUrl}{_baseUrl}/GetAllPermissions", null);
            int no = 1;
            data.Items?.ForEach(a => a.No = no++);
            return data.Items;
        }

        public async Task<RoleModel> Update(RoleModel role)
        {
            var data = await _webApiClient.PutAsync<RoleModel> ($"{_webApiClient.BaseUrl}{_baseUrl}/Update", role);

            return data;
        }

        public async Task<RoleModel> Create(RoleModel item)
        {
            var data = await _webApiClient.PostAsync<RoleModel>($"{_webApiClient.BaseUrl}{_baseUrl}/Create", item);

            return data;
        }

        public async Task Delete(int id)
        {
             await _webApiClient.DeleteAsync<object>($"{_webApiClient.BaseUrl}{_baseUrl}/Delete", new { Id=id });

        }
    }
}
