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
        private string _baseUrl = "api/services/app/OrganizationUnit/GetPaged";
        public OrganizationUnitService(IWebApiClient webApiClient)
        {
            this._webApiClient = webApiClient;
        }
        public async Task<List<OrganizationUnitItem>> GetPaged()
        {
           
            var para = new { MaxResultCount = 200 };
            var data = await _webApiClient.GetAsync<ResultData<List<OrganizationUnitItem>>>($"{_webApiClient.BaseUrl}{_baseUrl}", para);

            return data.Items;
        }
    }
}
