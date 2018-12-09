using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IFilesService
    {
        Task<ResultData<List<FilesManage>>> GetAllFiles(int entityId, string entityKey, string entityName = "", int pageIndex = 0, int pageSize = int.MaxValue);

        Task<FilesManage> UploadFileAsync(List<KeyValuePair<string, string>> keyValuePairs, string filePath, string fileName);
        Task Delete(long id);

    }
}
