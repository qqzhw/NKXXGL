using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class FilesService : IFilesService
    {
        private readonly IWebApiClient _webApiClient;
        private readonly string BaseUrl = "/api/services/app/FilesManage/";
        public FilesService(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public async Task<ResultData<List<FilesManage>>> GetAllFiles(int entityId, string entityKey, string entityName = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new
            {
                entityId,
                entityKey,
                entityName,
                MaxResultCount = pageSize,
                SkipCount = pageIndex * pageSize
            };
            var items= await _webApiClient.GetAsync<ResultData<List<FilesManage>>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetPaged"), filter);
            return items;
        }

        public async Task<FilesManage> UploadFileAsync(List<KeyValuePair<string, string>> keyValuePairs, string filePath, string fileName)
        {
            var item = await _webApiClient.UploadFileAsync<FilesManage>(Path.Combine(_webApiClient.BaseUrl, "api/FilesManage/UploadFileAsync"),keyValuePairs, filePath, fileName);
            return item;
        }

        public async Task Delete(long id)
        {
            await _webApiClient.DeleteAsync<object>(Path.Combine(_webApiClient.BaseUrl, "api/services/app/FilesManage/Delete"), id);
        }
    }
}
