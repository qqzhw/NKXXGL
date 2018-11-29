using ICIMS.Model;
using ICIMS.Model.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service
{
    public class UserService:IUserService
    {
        private const string BaseUrl = "api/services/app/User/";
        private IWebApiClient _webApiClient;
        public UserService(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient; 
        }
         
        public string Token { get; set; }
         
       
        public   UserModel LoginAsync(string userName, string password,string tenantName="")
        {
            _webApiClient.UserName = userName;
            _webApiClient.Password = password;
            _webApiClient.TenancyName = tenantName;
            var resultDto = _webApiClient.TokenBasedAuth();
            var user = new UserModel()
            {
                AccessToken = resultDto.AccessToken,
                Id = resultDto.UserId,
                Name = resultDto.Name,
                UnitId = resultDto.UnitId,
                UnitName = resultDto.UnitName,
                RoleNames=resultDto.RolesName
            };
            return user;
        }

        public async Task<string> GetCurrentLoginInfo()
        {
            return null;
        }


        public  async Task<string> GetUserInfoAsync(long userId)
        {
            //_webApiClient.TenancyName = "Default";
            //_webApiClient.UserName = "admin";
            //_webApiClient.Password = "123qwe";
            //_webApiClient.TokenBasedAuth();
            //var ss1 = await _webApiClient.GetAsync<UserModel>(_webApiClient.BaseUrl + "api/services/app/Session/GetCurrentLoginInformations", null);
            //var user =await _webApiClient.GetAsync<ResultData<List<UserModel>>>(_webApiClient.BaseUrl+ "api/services/app/Role/GetAll", null, null);
            return "";
        }

        
       
        public void SetToken(string token)
        {
            Token = token;
        }

        public async Task<ResultData<List<RoleModel>>> GetUserRoles()
        {
            
              var items = await _webApiClient.GetAsync<ResultData<List<RoleModel>>>($"{_webApiClient.BaseUrl}{BaseUrl}GetRoles", null);
            return items;
        }

        public async Task<List<UserModel>> GetAll(int SkipCount = 0, int MaxResultCount = int.MaxValue)
        {
            var para = new { MaxResultCount, SkipCount };
            var data = await _webApiClient.GetAsync<ResultData<List<UserModel>>>($"{_webApiClient.BaseUrl}{BaseUrl}GetAll", para);

            return data.Items;
        }

        public async Task<UserModel> Create(UserModel user)
        {
            var data = await _webApiClient.PostAsync<UserModel>($"{_webApiClient.BaseUrl}{BaseUrl}Create", user);

            return data;
        }

        public async Task<UserModel> Update(UserModel user)
        {
            var data = await _webApiClient.PutAsync<UserModel>($"{_webApiClient.BaseUrl}{BaseUrl}Update", user);

            return data;
        }

        public async Task Delete(int Id)
        {
            await _webApiClient.DeleteAsync<UserModel>($"{_webApiClient.BaseUrl}{BaseUrl}Delete",new { Id });
        }
    }
}
