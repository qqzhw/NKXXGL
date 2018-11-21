using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IBusinessTypeService
    {
        Task<ResultData<List<BusinessType>>> GetAllBusinessTypes(string Name = "", int pageIndex = 0, int pageSize = int.MaxValue);


        Task<BusinessType> GetById(int input);


        Task CreateOrUpdate(BusinessType input);


        Task Delete(int input);

    }
}
