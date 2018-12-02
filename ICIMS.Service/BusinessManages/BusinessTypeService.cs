using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class BusinessTypeService : IBusinessTypeService
    {
        private readonly string BaseUrl = "/api/services/app/BusinessType/";
        private readonly IWebApiClient _webApiClient;
        public BusinessTypeService(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public async Task CreateOrUpdate(BusinessType input)
        {
           await Task.CompletedTask;
        }

        public async Task Delete(int input)
        {
            await _webApiClient.DeleteAsync<object>($"{_webApiClient.BaseUrl}{BaseUrl}/Delete", new { Id = input });
        }

        public async Task<ResultData<List<BusinessType>>> GetAllBusinessTypes(string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new
            {  Name,
                MaxResultCount = pageSize,
                SkipCount = pageIndex * pageSize
            };
            var items = await _webApiClient.GetAsync<ResultData<List<BusinessType>>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetPaged"), filter);
            return items;
        }

        public Task<BusinessType> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
