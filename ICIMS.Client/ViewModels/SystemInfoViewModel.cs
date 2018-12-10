using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace ICIMS.Client.ViewModels
{ 
    public class SystemInfoViewModel : BindableBase
    {

        public string Id { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public InitializationMode InitMode { get; set; }

        private bool _isDefaultShow;
        public bool IsDefaultShow
        {
            get { return _isDefaultShow; }
            set { SetProperty(ref _isDefaultShow, value); }
        }
        private bool _isreadOnly;
        public bool IsReadOnly
        {
            get { return _isreadOnly; }
            set { SetProperty(ref _isreadOnly, value); }
        }
        private ModuleState _state;
        public ModuleState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }
        private int _msgCount;
        public int MsgCount
        {
            get { return _msgCount; }
            set { SetProperty(ref _msgCount, value); }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
        private ModuleInfo _moduleInfo;
        public ModuleInfo ModuleInfo
        {
            get { return _moduleInfo; }
            set { SetProperty(ref _moduleInfo, value); }
        }

        public string _icon;
        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }
        private ObservableCollection<SystemInfoViewModel> _childs;
        public ObservableCollection<SystemInfoViewModel> Childs
        {
            get { return _childs; }
            set { SetProperty(ref _childs, value); }
        }
        private double _opacity;
        public double Opacity
        {
            get { return _opacity; }
            set { SetProperty(ref _opacity, value); }
        }
      
    }
   
}
 
