using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class BusinessAuditService : IBusinessAuditService
    {
        private readonly string BaseUrl = "/api/services/app/BusinessAudit/";
        private readonly IWebApiClient _webApiClient;
        public BusinessAuditService(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public Task CreateOrUpdate(BusinessAudit input)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int input)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultData<List<BusinessAudit>>> GetAllBusinessAudits(int? BuinessTypeId = null, string BuinessTypeName = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new
            {
                BuinessTypeId,
                BuinessTypeName, 
                MaxResultCount = pageSize,
                SkipCount = pageIndex * pageSize
            };
            var items = await _webApiClient.GetAsync<ResultData<List<BusinessAudit>>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetPaged"), filter);
            return items;
        }

        public Task<BusinessAudit> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
