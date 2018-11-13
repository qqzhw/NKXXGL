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
        public Task CreateOrUpdate(ItemDefine input)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int input)
        {
            throw new NotImplementedException();
        }

        public Task<ResultData<ItemDefine>> GetAllItemDefines(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public Task<ItemDefine> GetById(int input)
        {
            throw new NotImplementedException();
        }
    }
}
