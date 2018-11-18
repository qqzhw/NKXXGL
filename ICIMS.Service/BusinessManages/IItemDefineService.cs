using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IItemDefineService
    {
        Task<ResultData<List<ItemDefine>>> GetAllItemDefines(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue);


        Task<ItemDefine> GetById(int input);


        Task CreateOrUpdate(ItemDefine input);


        Task Delete(int input);

    }

}
