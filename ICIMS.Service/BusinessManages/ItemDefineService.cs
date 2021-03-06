﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICIMS.Model.BusinessManages;

namespace ICIMS.Service.BusinessManages
{
    public class ItemDefineService : IItemDefineService
    {
        private readonly WebApiClient _webApiClient;
        private readonly string BaseUrl = "api/services/app/ItemDefine/";
        public ItemDefineService(WebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }
        public async Task<ItemDefine> CreateOrUpdate(ItemDefine input)
        {
            var param = new { ItemDefine=input };
            var item= await _webApiClient.PostAsync<ItemDefine>(Path.Combine(_webApiClient.BaseUrl,BaseUrl,"CreateOrUpdate"), param);
            return item;
        }

        public async Task Delete(int input)
        {
            await _webApiClient.DeleteAsync<object>($"{_webApiClient.BaseUrl}{BaseUrl}/Delete", new { Id = input });
        }

        public async Task<ResultData<List<ItemDefineList>>> GetAllItemDefines(int? status=null,string No = "", string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = new {Status=status,No,Name,MaxResultCount = pageSize ,SkipCount =pageIndex*pageSize };
            var items =await _webApiClient.GetAsync<ResultData<List<ItemDefineList>>>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetPaged"), filter);
            return items;
        }

        public async Task<ItemDefine> GetById(int input)
        {
            var param = new { Id = input };
            var items = await _webApiClient.GetAsync<ItemDefine>(Path.Combine(_webApiClient.BaseUrl, BaseUrl, "GetById"),param);
            return items;
        }
    }
}
