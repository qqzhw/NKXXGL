﻿using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IAuditMappingService
    {
        Task<ResultData<List<AuditMapping>>> GetAllAuditMappings(int itemId =0, int BusinessTypeId=0,string BusinessTypeName="", int pageIndex = 0, int pageSize = int.MaxValue);


        Task<AuditMapping> GetById(int input);


        Task<AuditMapping> CreateOrUpdate(AuditMapping input);


        Task Delete(int input);
    }
}
