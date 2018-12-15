using AutoMapper;
using ICIMS.Model.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Modules.BusinessManages
{
    public class BuinessMapperProfile:Profile
    {
         
        public BuinessMapperProfile()
        {
            CreateMap<ItemDefineList, ItemDefine>();
            CreateMap<ItemDefine, ItemDefineList>();

            CreateMap<ContractList, Contract>();
            CreateMap<BusinessAudit, BusinessAuditList>();
        }
    }
}
