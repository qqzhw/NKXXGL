using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class BuinessAuditService : IBuinessAuditService
    {
        private readonly string BaseUrl = "/api/services/app/BuinessAudit/";
        private readonly IWebApiClient _webApiClient;
        public BuinessAuditService(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public Task CreateOrUpdate(BuinessAudit input)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int input)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultData<List<BuinessAudit>>> GetAllBuinessAudits(int? BuinessTypeId = null, string BuinessTypeName = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new
            {
                BuinessTypeId,
                BuinessTypeName, 
                MaxResultCount = pageSize,
                SkipCount = pageIndex * pageSize
            };
            var items = await _webApiClient.GetAsync<ResultData<List<BuinessAudit>>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetPaged"), filter);
            return items;
        }

        public Task<BuinessAudit> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
