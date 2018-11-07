using ICIMS.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private const string baseService = "services/app/";
        private IWebApiClient _webApiClient;
        public UserService(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
            BaseUrl = baseService; 
        }
        public string BaseUrl { get; set; }
        public string Token { get; set; }
         
       
        public async Task<string> Authenticate(string userName, string pwd)
        {
            var url = new Uri("http://120.78.240.7:8088/TokenAuth/Authenticate");
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None, UseCookies = true };

            var param = new
            {
                UserNameOrEmailAddress = userName,
                Password = pwd,
                RememberClient = true, 
            };
          var postStr = JsonConvert.SerializeObject(param);
            using (var client = new HttpClient(handler))
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Accept.Add(new  MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = url;
                var content = new StringContent(postStr );
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string loginResult = await response.Content.ReadAsStringAsync();

                        var getCookies = handler.CookieContainer.GetCookies(url);

                        foreach (Cookie cookie in getCookies)
                        {

                        }
                        return loginResult;
                    }
                 
                }
                catch (Exception ex)
                { 
                    return string.Empty;
                }
                return string.Empty;
            }
             

        }

        public async Task<string> GetCurrentLoginInfo()
        {
            return null;
        }


        public  async Task<string> GetUserInfoAsync(long userId)
        {
            _webApiClient.TenancyName = "Default";
            _webApiClient.UserName = "admin";
            _webApiClient.Password = "123qwe";
            _webApiClient.TokenBasedAuth();
            var ss1 = await _webApiClient.GetAsync<User>(_webApiClient.BaseUrl + "api/services/app/Session/GetCurrentLoginInformations", null);
            var user =await _webApiClient.GetAsync<ResultData<List<User>>>(_webApiClient.BaseUrl+ "api/services/app/Role/GetAll", null, null);
            return "";
        }

        
       
        public void SetToken(string token)
        {
            Token = token;
        }

       
    }
}
