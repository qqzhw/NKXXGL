using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BaseData
{
    public class OrganizationUnitItem : BindableBase
    {
        private string _code;
        private string _displayName;
        private string _name;
        private string _description;
        private int? _parentId;
        private bool _published;
        private int _displayOrder;
        private DateTime _creationTime;
        private DateTime? _lastModificationTime;
        private OrganizationUnitItem _parent;
        private bool _isChecked;

        public OrganizationUnitItem()
        {
            this.Children = new ObservableCollection<OrganizationUnitItem>();
            this.Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertyChanged(nameof(IsLast));
        }
        private long _id;
        public long Id
        {
            get => _id; set
            {
                SetProperty(ref _id, value);
            }
        }
        public string Code { get => _code; set { SetProperty(ref _code, value); RaisePropertyChanged(nameof(GroupNo)); RaisePropertyChanged(nameof(Level)); } }

        public string DisplayName { get => _displayName; set => SetProperty(ref _displayName, value); }

        public string Description { get => _description; set => SetProperty(ref _description, value); }

        //上级编号
        public int? ParentId { get => _parentId; set => SetProperty(ref _parentId, value); }


        public bool Published { get => _published; set { SetProperty(ref _published, value); RaisePropertyChanged(nameof(IsForbiddened)); } }

        public int DisplayOrder { get => _displayOrder; set => SetProperty(ref _displayOrder, value); }
        public DateTime CreationTime { get => _creationTime; set => SetProperty(ref _creationTime, value); }
        public DateTime? LastModificationTime { get => _lastModificationTime; set => SetProperty(ref _lastModificationTime, value); }

        [JsonIgnore]
        public OrganizationUnitItem Parent { get => _parent; set => _parent = value; }

        public ObservableCollection<OrganizationUnitItem> Children { get; set; } = new ObservableCollection<OrganizationUnitItem>();

        public string GroupNo
        {
            get
            {
                if (string.IsNullOrEmpty(_code))
                {
                    return "";
                }
                var idx = this.Code.LastIndexOf('-');
                if (idx != -1)
                {
                    return this.Code.Substring(0, idx);
                }

                return this.Code;
            }
        }

        public int Level
        {
            get
            {
                var cnt = this.GroupNo.Split('-').Length;

                return cnt;
            }
        }

        public string IsLast
        {
            get
            {
                return this.Children?.Count == 0 ? "是" : "否";
            }
        }

        public string IsForbiddened
        {
            get
            {
                return Published ? "是" : "否";
            }
        }

        public bool IsChecked { get => _isChecked; set => SetProperty(ref _isChecked,value); }
        public string Name { get => _name; set => SetProperty(ref _name,value); }
    }
}
