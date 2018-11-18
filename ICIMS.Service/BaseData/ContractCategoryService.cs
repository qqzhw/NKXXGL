using ICIMS.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BaseData
{
    public class ContractCategoryService: IContractCategoryService
    {
        private IWebApiClient _webApiClient;
        private string _baseUrl = "api/services/app/ContractCategory";
        public ContractCategoryService(IWebApiClient webApiClient)
        {
            this._webApiClient = webApiClient;
        }
        public async Task<(int totalCount, List<ContractItem> datas)> GetPageItems(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var skipCount = (pageIndex - 1) * pageSize;
            var para = new { No, Name, MaxResultCount = pageSize, SkipCount = skipCount > 0 ? skipCount : 0 };
            var data = await _webApiClient.GetAsync<ResultData<List<ContractItem>>>($"{_webApiClient.BaseUrl}{_baseUrl}/GetPaged", para);

            return (totalCount: data.TotalCount, datas: data.Items);
        }

        public async Task Delete(int id)
        {
            await _webApiClient.DeleteAsync<object>($"{_webApiClient.BaseUrl}{_baseUrl}/Delete", new { Id = id });
        }

        public async Task<ContractItem> CreateOrUpdate(ContractItem contractCategory)
        {
            return await _webApiClient.PostAsync<ContractItem>($"{_webApiClient.BaseUrl}{_baseUrl}/CreateOrUpdate", new { contractCategory });
        }
    }
}
