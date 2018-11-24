using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class AuditMappingService : IAuditMappingService
    {
        private readonly IWebApiClient _webApiClient;
        private readonly string BaseUrl = "/api/services/app/AuditMapping/";
        public AuditMappingService(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public async Task<AuditMapping> CreateOrUpdate(AuditMapping input)
        {
            var param = new { AuditMapping = input };
            var item = await _webApiClient.PostAsync<AuditMapping>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "CreateOrUpdate"), param);
            return item;
        }

        public async Task Delete(int input)
        {
            await Task.CompletedTask;
        }

        public async Task<ResultData<List<AuditMapping>>> GetAllAuditMappings(int ItemId=0, int BuinessTypeId =0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new
            {
                ItemId,
                BuinessTypeId,
                MaxResultCount = pageSize,
                SkipCount = pageIndex * pageSize
            };
            var items = await _webApiClient.GetAsync<ResultData<List<AuditMapping>>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetPaged"), filter);
            return items;
        }

        public async Task<AuditMapping> GetById(int input)
        {
            return null;
        }
    }
}
