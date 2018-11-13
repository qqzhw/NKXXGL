using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BaseData
{
    public class YsCategoryItem : BindableBase
    {
        private string _no;
        private string _name;
        private string _description;
        private int _parentId;
        private bool _published;
        private int _displayOrder;
        private DateTime _creationTime;
        private DateTime? _lastModificationTime;
        private YsCategoryItem _parent;

        public YsCategoryItem()
        {
            this.Children = new ObservableCollection<YsCategoryItem>();
            this.Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertyChanged(nameof(IsLast));
        }
        public int Id { get; set; }

        public string No { get => _no; set { SetProperty(ref _no, value); RaisePropertyChanged(nameof(GroupNo)); RaisePropertyChanged(nameof(Level)); } }

        public string Name { get => _name; set => SetProperty(ref _name, value); }

        public string Description { get => _description; set => SetProperty(ref _description, value); }

        //上级编号
        public int ParentId { get => _parentId; set => SetProperty(ref _parentId, value); }


        public bool Published { get => _published; set { SetProperty(ref _published, value); RaisePropertyChanged(nameof(IsForbiddened)); } }

        public int DisplayOrder { get => _displayOrder; set => SetProperty(ref _displayOrder, value); }
        public DateTime CreationTime { get => _creationTime; set => SetProperty(ref _creationTime, value); }
        public DateTime? LastModificationTime { get => _lastModificationTime; set => SetProperty(ref _lastModificationTime, value); }

        public YsCategoryItem Parent { get => _parent; set => _parent = value; }

        public ObservableCollection<YsCategoryItem> Children { get; set; } = new ObservableCollection<YsCategoryItem>();

        public string GroupNo
        {
            get
            {
                if (string.IsNullOrEmpty(_no))
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
