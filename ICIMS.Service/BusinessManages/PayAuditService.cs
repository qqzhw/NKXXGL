using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class PayAuditService : IPayAuditService
    {
        private readonly WebApiClient _webApiClient;
        private readonly string BaseUrl = "api/services/app/PayAudit/";
        public PayAuditService(WebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public async Task CreateOrUpdate(PayAudit input)
        {
            var param = new { ItemDefine = input };
            await _webApiClient.PostAsync(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "CreateOrUpdate"), param);
        }
       

        public Task Delete(int input)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultData<List<PayAuditList>>> GetAllPayAudits(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new { No, Name, MaxResultCount = pageSize, SkipCount = pageIndex * pageSize };
            var items = await _webApiClient.GetAsync<ResultData<List<PayAuditList>>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetPaged"), filter);
            return items;
        }

        public Task<PayAudit> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
