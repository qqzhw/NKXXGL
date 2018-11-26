using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.User
{
    public class PermissionModel: BindableBase
    {
        private string _name;
        private string _displayName;
        private string _description;
        private int _id;
        private bool _isChecked;

        public string Name { get => _name; set => SetProperty(ref _name,value); }
        public string DisplayName { get => _displayName; set => SetProperty(ref _displayName, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public int Id { get => _id; set => SetProperty(ref _id, value); }
        public bool IsChecked { get => _isChecked; set => SetProperty(ref _isChecked,value); }
        public int No { get; set; }
    }
}
