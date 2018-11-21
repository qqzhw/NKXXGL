using AutoMapper;
using ICIMS.Modules.BusinessManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Client
{
    public class ConfigurationMapper
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<BuinessMapperProfile>();
                //cfg.AddProfile<Profiles.SourceProfile>();
                //cfg.AddProfile<Profiles.OrderProfile>();
                //cfg.AddProfile<Profiles.CalendarEventProfile>();
            });
        }
    }
}
