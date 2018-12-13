using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class BusinessAuditStatusService : IBusinessAuditStatusService
    {
        private readonly WebApiClient _webApiClient;
        private readonly string BaseUrl = "api/services/app/BusinessAuditStatus/";
        public BusinessAuditStatusService(WebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public async Task<BusinessAuditStatus> CreateOrUpdate(BusinessAuditStatus input)
        {
            var param = new { BusinessAuditStatus = input };
            var item= await _webApiClient.PostAsync<BusinessAuditStatus>(Path.Combine(_webApiClient.BaseUrl,BaseUrl,"CreateOrUpdate"), param);
            return item;
        }

        public async Task Delete(int input)
        {
            await _webApiClient.DeleteAsync<object>($"{_webApiClient.BaseUrl}{BaseUrl}/Delete", new { Id = input });
        }

        public async Task<ResultData<List<BusinessAuditStatus>>> GetAll(int entityId,string businesstypename, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new { entityId,businesstypename,MaxResultCount = pageSize ,SkipCount =pageIndex*pageSize };
            var items =await _webApiClient.GetAsync<ResultData<List<BusinessAuditStatus>>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetPaged"), filter);
            return items;
        }

        public async Task<BusinessAuditStatus> GetById(int input)
        {
            var param = new { Id = input };
            var items = await _webApiClient.GetAsync<BusinessAuditStatus>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetById"),param);
            return items;
        }
    }
}
