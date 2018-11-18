using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class ItemDefineService : IItemDefineService
    {
        private readonly WebApiClient _webApiClient;
        public ItemDefineService(WebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public async Task CreateOrUpdate(ItemDefine input)
        {
            var param = new { ItemDefine=input };
           await _webApiClient.PostAsync("http://localhost:21025/api/services/app/ItemDefine/CreateOrUpdate", param);
        }

        public Task Delete(int input)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultData<List<ItemDefine>>> GetAllItemDefines(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var p = new {MaxResultCount = 1 ,SkipCount = 0 };
            var ss =await _webApiClient.GetAsync<object>("http://localhost:21025/api/services/app/ItemDefine/GetPaged",p);
            return null;
        }

        public Task<ItemDefine> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
