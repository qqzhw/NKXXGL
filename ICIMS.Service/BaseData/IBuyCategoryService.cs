using ICIMS.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BaseData
{
    public interface IBuyCategoryService
    {
        Task<ResultData<BuyCategory>>  GetAllCategorys(string No="",string Name="", int pageIndex = 0, int pageSize = int.MaxValue);

         
        Task<BuyCategory> GetById(int input);

         
        Task CreateOrUpdate(BuyCategory input);

 
        Task Delete(int input);

    }
}
