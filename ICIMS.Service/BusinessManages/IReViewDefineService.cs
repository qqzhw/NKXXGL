using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IReViewDefineService
    {
        Task<ResultData<ReViewDefine>> GetAllReViewDefines(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue);


        Task<ReViewDefine> GetById(int input);


        Task CreateOrUpdate(ReViewDefine input);


        Task Delete(int input);

    }
}
