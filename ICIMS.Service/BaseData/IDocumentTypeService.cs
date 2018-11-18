using ICIMS.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BaseData
{
    public interface IDocumentTypeService
    {
        Task<(int totalCount, List<DocumentTypeItem> datas)> GetPageItems(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task Delete(int id);
        Task<DocumentTypeItem> CreateOrUpdate(DocumentTypeItem fundFrom);
    }
}
