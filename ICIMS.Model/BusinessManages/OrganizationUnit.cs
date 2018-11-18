using System;
using System.Collections.Generic; 
using System.Linq;
using ICIMS.Model;
using Prism.Mvvm;
using System.Collections.ObjectModel;
namespace ICIMS.Model.BusinessManages
{

    public class OrganizationUnit: BindableBase
    {

        public long Id { get; set; } 
      
        public   int? TenantId { get; set; }

        private OrganizationUnit _parent;
        public  OrganizationUnit Parent { get => _parent; set => SetProperty(ref _parent, value); }

        private long? _parentId;
        public  long? ParentId { get => _parentId; set => SetProperty(ref _parentId, value); }
        private string  _code;
        public   string Code { get => _code; set => SetProperty(ref _code, value); }

        private string _displayName;
        public   string DisplayName { get => _displayName; set => SetProperty(ref _displayName, value); }

        private ObservableCollection<OrganizationUnit> _children;
        public  ObservableCollection<OrganizationUnit> Children { get => _children; set => SetProperty(ref _children, value); }


        public OrganizationUnit()
        {
            _children = new ObservableCollection<OrganizationUnit>();
        }
  
    }
}
