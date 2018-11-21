using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IAuditMappingService
    {
        Task<ResultData<List<BuinessAudit>>> GetAllAuditMappings(int? BuinessTypeId = null, string BuinessTypeName = "", int pageIndex = 0, int pageSize = int.MaxValue);


        Task<BuinessAudit> GetById(int input);


        Task CreateOrUpdate(BuinessAudit input);


        Task Delete(int input);
    }
}
