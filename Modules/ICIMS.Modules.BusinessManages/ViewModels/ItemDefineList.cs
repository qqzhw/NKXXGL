using ICIMS.Model.BaseData;
using ICIMS.Model.BusinessManages;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Modules.BusinessManages.ViewModels
{
    public class ItemDefineList : BindableBase
    {
        private ItemDefine _itemdefine;
        public ItemDefine ItemDefine
        {
            get => _itemdefine; set { SetProperty(ref _itemdefine, value); }
        }
        public Budget _budget;
        public Budget Budget
        {
            get => _budget; set { SetProperty(ref _budget, value); }
        }
        private ItemCategoryItem _itemcategory;
        public ItemCategoryItem ItemCategory 
        {
            get => _itemcategory; set { SetProperty(ref _itemcategory, value); }
        }
    }
     
}
