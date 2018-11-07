using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BaseData;

namespace ICIMS.Service.BaseData
{
    public class FundFromService : IFundFromService
    {
        private IWebApiClient _webApiClient;
        private string _baseUrl = "api/services/app/FundFrom/GetPaged";
        private FundFromService(IWebApiClient webApiClient)
        {
            this._webApiClient = webApiClient;
        }
        public async Task<List<FundItem>> GetPageItems()
        {
            _webApiClient.TenancyName = "Default";
            _webApiClient.UserName = "admin";
            _webApiClient.Password = "123qwe";
            _webApiClient.TokenBasedAuth();
            var para = new{ MaxResultCount=200 };
            var datas = await _webApiClient.GetAsync<List<FundItem>>($"{_webApiClient.BaseUrl}/{_baseUrl}", para);

            return datas;
        }
    }
}
