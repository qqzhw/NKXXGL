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
        private readonly string BaseUrl = "api/services/app/BusinessAudit/";
        private readonly IWebApiClient _webApiClient;
        public BusinessAuditService(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public async Task<BusinessAudit> CreateOrUpdate(BusinessAudit businessAudit)
        {
            var data = await _webApiClient.PostAsync<BusinessAudit>($"{_webApiClient.BaseUrl}{BaseUrl}CreateOrUpdate", new { businessAudit });

            return data;
        }

        public async Task Delete(int Id)
        {
            await _webApiClient.DeleteAsync<BusinessAudit>($"{_webApiClient.BaseUrl}{BaseUrl}CreateOrUpdate", new { Id });
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
            var url = $"{_webApiClient.BaseUrl}{BaseUrl}GetPaged";
            var items = await _webApiClient.GetAsync<ResultData<List<BusinessAudit>>>(url, filter);
            return items;
        }

        public Task<BusinessAudit> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
