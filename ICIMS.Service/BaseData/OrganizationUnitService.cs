using ICIMS.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BaseData
{
    public class OrganizationUnitService: IOrganizationUnitService
    {
        private IWebApiClient _webApiClient;
        private string _baseUrl = "api/services/app/Department";
        public OrganizationUnitService(IWebApiClient webApiClient)
        {
            this._webApiClient = webApiClient;
        }
        public async Task<(int totalCount, List<OrganizationUnitItem> datas)> GetPageItems(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var skipCount = (pageIndex - 1) * pageSize;
            var para = new { No, Name, MaxResultCount = pageSize, SkipCount = skipCount > 0 ? skipCount : 0 };
            var data = await _webApiClient.GetAsync<ResultData<List<OrganizationUnitItem>>>($"{_webApiClient.BaseUrl}{_baseUrl}/GetPaged", para);

            return (totalCount: data.TotalCount, datas: data.Items);
        }

        public async Task Delete(int id)
        {
            await _webApiClient.DeleteAsync<object>($"{_webApiClient.BaseUrl}{_baseUrl}/Delete", new { Id = id });
        }

        public async Task<OrganizationUnitItem> CreateOrUpdate(OrganizationUnitItem fundFrom)
        {
            return await _webApiClient.PostAsync<OrganizationUnitItem>($"{_webApiClient.BaseUrl}{_baseUrl}/CreateOrUpdate", new { fundFrom });
        }

        public async Task<OrganizationUnitItem> Create(OrganizationUnitItem fundFrom)
        {
            return await _webApiClient.PostAsync<OrganizationUnitItem>($"{_webApiClient.BaseUrl}{_baseUrl}/CreateAsync",  new { fundFrom.Code, fundFrom.DisplayName});
        }

        public async Task<OrganizationUnitItem> Update(OrganizationUnitItem fundFrom)
        {
            return await _webApiClient.PutAsync<OrganizationUnitItem>($"{_webApiClient.BaseUrl}{_baseUrl}/UpdateAsync",  fundFrom);
        }
    }
}
