using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IBudgetService
    {
        Task<ResultData<Budget>> GetAllBudgets(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue);


        Task<Budget> GetById(int input);


        Task CreateOrUpdate(Budget input);


        Task Delete(int input);


    }
}
