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

        public async Task<ResultData<List<ItemDefineList>>> GetAllItemDefines(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new {No,Name,MaxResultCount = pageSize ,SkipCount =pageIndex*pageSize };
            var items =await _webApiClient.GetAsync<ResultData<List<ItemDefineList>>>("http://localhost:21025/api/services/app/ItemDefine/GetPaged", filter);
            return items;
        }

        public Task<ItemDefine> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
