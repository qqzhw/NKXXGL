using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IBuinessAuditService
    {
        Task<ResultData<List<BuinessAudit>>> GetAllBuinessAudits(int? BuinessTypeId=null, string BuinessTypeName = "", int pageIndex = 0, int pageSize = int.MaxValue);


        Task<BuinessAudit> GetById(int input);


        Task CreateOrUpdate(BuinessAudit input);


        Task Delete(int input);
    }
}
