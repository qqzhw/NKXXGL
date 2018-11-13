using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class PayAuditService : IPayAuditService
    {
        public Task CreateOrUpdate(PayAudit input)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int input)
        {
            throw new NotImplementedException();
        }

        public Task<ResultData<PayAudit>> GetAllPayAudits(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public Task<PayAudit> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
