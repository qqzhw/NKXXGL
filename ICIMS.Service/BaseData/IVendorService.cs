using ICIMS.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BaseData
{
    public interface IVendorService
    {
        Task<(int totalCount, List<VendorItem> datas)> GetPageItems(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task Delete(int id);
        Task<VendorItem> CreateOrUpdate(VendorItem fundFrom);
         
        Task<VendorItem> GetById(int input);
    }
}
