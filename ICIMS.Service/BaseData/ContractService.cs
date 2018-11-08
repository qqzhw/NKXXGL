using ICIMS.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BaseData
{
    public class ContractService: IContractService
    {
        private IWebApiClient _webApiClient;
        private string _baseUrl = "api/services/app/ContractCategory/GetPaged";
        public ContractService(IWebApiClient webApiClient)
        {
            this._webApiClient = webApiClient;
        }
        public async Task<List<ContractItem>> GetPaged()
        {
            _webApiClient.TenancyName = "Default";
            _webApiClient.UserName = "admin";
            _webApiClient.Password = "123qwe";
            _webApiClient.TokenBasedAuth();
            var para = new { MaxResultCount = 200 };
            var data = await _webApiClient.GetAsync<ResultData<List<ContractItem>>>($"{_webApiClient.BaseUrl}{_baseUrl}", para);

            return data.Items;
        }
    }
}
