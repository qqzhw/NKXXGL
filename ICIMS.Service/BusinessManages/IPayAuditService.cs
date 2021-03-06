﻿using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BusinessManages
{
    public interface IPayAuditService
    {
        Task<ResultData<List<PayAuditList>>> GetAllPayAudits(string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue);


        Task<PayAudit> GetById(int input);


        Task<PayAudit> CreateOrUpdate(PayAudit input);

        Task<List<PayAudit>> SearchPayCount(int itemdefineId);

        Task Delete(int input);

    }
}
