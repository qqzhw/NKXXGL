﻿using ICIMS.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service.BaseData
{
    public interface ISubjectService
    {
        Task<List<SubjectItem>> GetPaged();
    }
}
