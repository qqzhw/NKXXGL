using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class BusinessTypeService : IBusinessTypeService
    {
        public Task CreateOrUpdate(BusinessType input)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int input)
        {
            throw new NotImplementedException();
        }

        public Task<ResultData<BusinessType>> GetAllBusinessTypes(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public Task<BusinessType> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
