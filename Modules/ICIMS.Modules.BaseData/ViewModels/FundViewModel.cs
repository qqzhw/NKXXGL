using ICIMS.Model.BaseData;
using ICIMS.Service.BaseData;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Modules.BaseData.ViewModels
{
    public class FundViewModel : BindableBase, INavigationAware
    {
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public ObservableCollection<FundItem> Items { get; set; }

        public FundViewModel(IFundFromService service)
        {
            this.Items = new ObservableCollection<FundItem>();
            List<FundItem> datas = service.GetPageItems().Result;
            foreach (var data in datas)
            {
                if(data.GroupNo != data.No)
                {
                    data.Parent = datas.FirstOrDefault(a => a.No == data.GroupNo);
                    data.Parent.Children.Add(data);
                }
                else
                {
                    this.Items.Add(data);
                }
            }
        }




    }
}
