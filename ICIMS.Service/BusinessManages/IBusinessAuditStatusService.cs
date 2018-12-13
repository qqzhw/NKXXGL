using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IBusinessAuditStatusService
    {
        Task<ResultData<List<BusinessAuditStatus>>> GetAll(int EntityId,string buinesstypeName, int pageIndex = 0, int pageSize = int.MaxValue);


        Task<BusinessAuditStatus> GetById(int input);


        Task<BusinessAuditStatus> CreateOrUpdate(BusinessAuditStatus input);


        Task Delete(int input);

    }

}
