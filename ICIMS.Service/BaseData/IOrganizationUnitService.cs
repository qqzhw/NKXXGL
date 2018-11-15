using ICIMS.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BaseData
{
    public interface IOrganizationUnitService
    {
        Task<(int totalCount, List<OrganizationUnitItem> datas)> GetPageItems(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task Delete(int id);
        Task<OrganizationUnitItem> CreateOrUpdate(OrganizationUnitItem fundFrom);
    }
}
