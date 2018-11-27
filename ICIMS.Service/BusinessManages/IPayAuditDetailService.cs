using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IPayAuditDetailService
    {

        Task<ResultData<List<PayAuditDetail>>> GetAllContracts(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue);


        Task<PayAuditDetail> GetById(int input);


        Task<PayAuditDetail> CreateOrUpdate(PayAuditDetail input);


        Task Delete(int input);
    }
}
