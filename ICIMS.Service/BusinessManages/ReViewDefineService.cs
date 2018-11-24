using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class ReViewDefineService : IReViewDefineService
    {
        private readonly WebApiClient _webApiClient;
        private readonly string BaseUrl = "api/services/app/ReViewDefine/";
        public ReViewDefineService(WebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public async Task<ReViewDefine>   CreateOrUpdate(ReViewDefine input)
        {
            var param = new { ReViewDefine = input };
            var item = await _webApiClient.PostAsync<ReViewDefine>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "CreateOrUpdate"), param);
            return item;
        }

        public async Task Delete(int input)
        {
            var filter = new { Id=input};
            var items = await _webApiClient.DeleteAsync<object>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "Delete"), filter);
            
        }

        public async Task<ResultData<List<ReViewDefineList>>> GetAllReViewDefines(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new { No, Name, MaxResultCount = pageSize, SkipCount = pageIndex * pageSize };
            var items = await _webApiClient.GetAsync<ResultData<List<ReViewDefineList>>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetPaged"), filter);
            return items;
        }

        public Task<ReViewDefine> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
