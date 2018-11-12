using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BaseData
{
    public class FundItem : BindableBase
    {
        private string _no;
        private string _name;
        private string _description;
        private bool _published;

        public string No { get => _no; set { _no = value; this.RaisePropertyChanged(nameof(No)); } }

        public string Name { get => _name; set { _name = value; this.RaisePropertyChanged(nameof(Name)); } }

        public string Description { get => _description; set { _description = value; this.RaisePropertyChanged(nameof(Description)); } }

        //上级编号
        public int ParentId { get; set; }


        public bool Published { get => _published; set { _published = value; this.RaisePropertyChanged(nameof(Published)); } }

        public int DisplayOrder { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public FundItem Parent { get; set; }

        public ObservableCollection<FundItem> Children { get; set; } = new ObservableCollection<FundItem>();

        public string GroupNo
        {
            get
            {
                if (string.IsNullOrEmpty(this.No))
                {
                    return "";
                }
                var idx = this.No.LastIndexOf('-');
                if (idx != -1)
                {
                    return this.No.Substring(0, idx);
                }

                return this.No;
            }
        }

        public int Level
        {
            get
            {
                var cnt = this.GroupNo.Split('-').Length;

                return 1;
            }
        }

        public string IsLast
        {
            get
            {
                return this.Children.Count == 0 ? "是" : "否";
            }
        }

        public string IsForbiddened
        {
            get
            {
                return Published ? "是" : "否";
            }
        }

    }
}
