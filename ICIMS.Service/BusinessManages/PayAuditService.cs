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
        public async Task<PayAudit> CreateOrUpdate(PayAudit input)
        {
            var param = new { PayAudit = input };
            var item = await _webApiClient.PostAsync<PayAudit>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "CreateOrUpdate"), param);
            return item;
    
        }
       

        public async Task Delete(int input)
        {
            await _webApiClient.DeleteAsync<object>($"{_webApiClient.BaseUrl}{BaseUrl}/Delete", new { Id = input  });
        }

        public async Task<ResultData<List<PayAuditList>>> GetAllPayAudits(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new { No, Name, MaxResultCount = pageSize, SkipCount = pageIndex * pageSize };
            var items = await _webApiClient.GetAsync<ResultData<List<PayAuditList>>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetPaged"), filter);
            return items;
        }

        public async Task<PayAudit> GetById(int input)
        {
            var param = new { Id = input };
            var item = await _webApiClient.GetAsync<PayAudit>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetById"), param);
            return item;
        }

        public async Task<List<PayAudit>> SearchPayCount(int input)
        {
            var param = new { Id= input }; 
             var item = await _webApiClient.GetAsync<List<PayAudit>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetSearchPayCount"), param);
            return item;
        }
    }
}
