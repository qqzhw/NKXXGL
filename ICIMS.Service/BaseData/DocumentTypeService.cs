using ICIMS.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BaseData
{
   public  class DocumentTypeService: IDocumentTypeService
    {
        private IWebApiClient _webApiClient;
        private string _baseUrl = "api/services/app/DocumentCategory/GetPaged";
        public DocumentTypeService(IWebApiClient webApiClient)
        {
            this._webApiClient = webApiClient;
        }
        public async Task<List<DocumentTypeItem>> GetPaged()
        {
            
            var para = new { MaxResultCount = 200 };
            var data = await _webApiClient.GetAsync<ResultData<List<DocumentTypeItem>>>($"{_webApiClient.BaseUrl}{_baseUrl}", para);

            return data.Items;
        }
    }
}
