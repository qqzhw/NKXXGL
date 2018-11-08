using ICIMS.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BaseData
{
    public class BuyCategoryService : IBuyCategoryService
    {
        private IWebApiClient _webApiClient;
        public BuyCategoryService(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public async Task CreateOrUpdate(BuyCategory input)
        {
             await Task.CompletedTask;
        }

        public async Task Delete(int input)
        {
            await Task.CompletedTask;
        }

        public Task<ResultData<BuyCategory>> GetAllCategorys(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return null;
        }

        public async Task<BuyCategory> GetById(int input)
        {
            
            return null;
        }

         
    }
}
