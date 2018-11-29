using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BusinessManages
{
    public class ReViewDefineList : BindableBase
    {
        private ReViewDefine _reviewDefine;
        public ReViewDefine ReViewDefine { get => _reviewDefine; set { SetProperty(ref _reviewDefine, value);
                switch (_reviewDefine?.Status)
                {
                    case 1:
                        _statusText = "提交审核";
                        break;
                    case 2:
                        _statusText = "审核中";
                        break;
                    case 3:
                        _statusText = "已审核";
                        break;
                    default:
                        break;
                }
                SetProperty(ref _statusText, _statusText);
            }
        }

        private string _itemNo;
        public string ItemNo { get => _itemNo; set => SetProperty(ref _itemNo, value); }
        private string _itemdefinename;
        public string ItemDefineName { get => _itemdefinename; set => SetProperty(ref _itemdefinename, value); }
        private decimal _itemdefineamount;
        public decimal ItemDefineAmount { get => _itemdefineamount; set => SetProperty(ref _itemdefineamount, value); }
        private string _unitName;
        public string UnitName { get => _unitName; set => SetProperty(ref _unitName, value); }

        private string _creatorusername;
        public string CreatorUserName { get => _creatorusername; set => SetProperty(ref _creatorusername, value); }

        private string _auditusername;
        public string AuditUserName { get => _auditusername; set => SetProperty(ref _auditusername, value); }

        private bool _isdeleted;
        public bool IsDeleted { get => _isdeleted; set => SetProperty(ref _isdeleted, value); }

        private long? _creatoruserId;
        public long? CreatorUserId { get => _creatoruserId; set => SetProperty(ref _creatoruserId, value); }
        private DateTime _creationtime;
        public DateTime CreationTime { get => _creationtime; set => SetProperty(ref _creationtime, value); }

        private string _statusText;
        public string StatusText
        {
            get
            { 
                return _statusText;
            }
            set { SetProperty(ref _statusText, value); }
        }
    }
}
