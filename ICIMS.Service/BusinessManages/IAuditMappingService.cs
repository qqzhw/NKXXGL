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
        Task<ResultData<List<AuditMapping>>> GetAllAuditMappings(int itemId =0, int BuinessTypeId=0, int pageIndex = 0, int pageSize = int.MaxValue);


        Task<AuditMapping> GetById(int input);


        Task CreateOrUpdate(AuditMapping input);


        Task Delete(int input);
    }
}
