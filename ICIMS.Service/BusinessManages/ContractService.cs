using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class ContractService : IContractService
    {
        private readonly WebApiClient _webApiClient;
        private readonly string BaseUrl = "api/services/app/Contract/";
        public ContractService(WebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public async Task<Contract> CreateOrUpdate(Contract input)
        {
            var param = new { Contract = input };
            var item = await _webApiClient.PostAsync<Contract>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "CreateOrUpdate"), param);
            return item;
        }

        public async Task Delete(int input)
        {
            await _webApiClient.DeleteAsync<object>($"{_webApiClient.BaseUrl}{BaseUrl}/Delete", new { Id = input });
        }

        public async Task<ResultData<List<ContractList>>> GetAllContracts(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new { No, Name, MaxResultCount = pageSize, SkipCount = pageIndex * pageSize };
            var items = await _webApiClient.GetAsync<ResultData<List<ContractList>>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetPaged"), filter);
            return items;
        }

        public Task<Contract> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
