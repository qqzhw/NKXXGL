﻿using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    /// <summary>
    /// 合同接口
    /// </summary>
    public interface IContractService
    {
        Task<ResultData<Contract>> GetAllContracts(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue);


        Task<Contract> GetById(int input);


        Task CreateOrUpdate(Contract input);


        Task Delete(int input);

    }
}