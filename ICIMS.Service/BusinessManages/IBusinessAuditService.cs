using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IBusinessAuditService
    {
        Task<ResultData<List<BusinessAudit>>> GetAllBusinessAudits(int? BusinessTypeId=null, string BusinessTypeName = "", int pageIndex = 0, int pageSize = int.MaxValue);


        Task<BusinessAudit> GetById(int input);


        Task<BusinessAudit> CreateOrUpdate(BusinessAudit input);


        Task Delete(int input);
    }
}
