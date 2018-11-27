
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ICIMS.Model.BusinessManages
{
    public class BusinessType: BindableBase
    {
        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }
        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public int DisplayOrder { get; set; }
        public int? TenantId { get; set ; }
        public bool IsDeleted { get ; set ; }
        public int No { get; set; }
        public ObservableCollection<BusinessAudit> Audits { get => _audits; set =>SetProperty(ref _audits,value); }

        private ObservableCollection<BusinessAudit> _audits;
    }
}
