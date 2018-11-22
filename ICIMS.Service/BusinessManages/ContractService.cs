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
        public Task CreateOrUpdate(Contract input)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int input)
        {
            throw new NotImplementedException();
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
