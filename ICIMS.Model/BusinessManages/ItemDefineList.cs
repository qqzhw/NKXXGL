using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ICIMS.Model.BusinessManages
{
    public class ItemDefineList : BindableBase
    {
        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        private string _sysGuid;
        public string SysGuid { get => _sysGuid; set => SetProperty(ref _sysGuid, value); }

        /// <summary>
        /// Status
        /// </summary> 
        private int _status;
        public int Status { get => _status; set { SetProperty(ref _status, value);
                switch (_status)
                {
                    case 0:
                        StatusText = "制单";
                        StatusColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF00"));
                        break;
                    case 1:
                        StatusText = "审核中";
                        StatusColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3cb371"));
                        break;
                    case 2:
                        StatusText = "结审";
                        StatusColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f08080"));
                        break;
                    default:
                        StatusText = "制单";
                        StatusColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF00"));
                        break;
                }
            } }
        private string _statusText;
        public string StatusText { get => _statusText; set => SetProperty(ref _statusText, value); }

        private Brush _statuscolor;
        public Brush StatusColor
        {
            get => _statuscolor; set => SetProperty(ref _statuscolor, value);           
        }

        private long _unitId;
        public long UnitId { get => _unitId; set => SetProperty(ref _unitId, value); }
        private string _unitName;
        public string UnitName { get => _unitName; set => SetProperty(ref _unitName, value); }


        private int? _budgetId;
        public int? BudgetId { get => _budgetId; set => SetProperty(ref _budgetId, value); }

        private string _budgetNo;
        public string BudgetNo { get => _budgetNo; set => SetProperty(ref _budgetNo, value); }
        private string _budgetName;
        public string BudgetName { get => _budgetName; set => SetProperty(ref _budgetName, value); }
        /// <summary>
        /// ItemDocNo
        /// </summary>
        private string _itemdocNo;
        public string ItemDocNo { get => _itemdocNo; set => SetProperty(ref _itemdocNo, value); }

        /// <summary>
        /// DefineDate
        /// </summary>
        private DateTime _defineDate;
        public DateTime DefineDate { get => _defineDate; set => SetProperty(ref _defineDate, value); }

        private string _itemNo;
        public string ItemNo { get => _itemNo; set => SetProperty(ref _itemNo, value); }

        private string _itemName;
        public string ItemName { get => _itemName; set => SetProperty(ref _itemName, value); }



        /// <summary>
        /// ItemType
        /// </summary>
        private int _itemcategoryId;
        public int ItemCategoryId { get => _itemcategoryId; set => SetProperty(ref _itemcategoryId, value); }

        private string _itemcategoryName;
        public string ItemCategoryName { get => _itemcategoryName; set => SetProperty(ref _itemcategoryName, value); }

        private decimal _defineAmount;
        public decimal DefineAmount { get => _defineAmount; set => SetProperty(ref _defineAmount, value); }

        /// <summary>
        /// ItemAddress
        /// </summary>
        private string _itemaddress;
        public string ItemAddress { get => _itemaddress; set => SetProperty(ref _itemaddress, value); }



        /// <summary>
        /// ItemDescription
        /// </summary>
        private string _itemdescription;
        public string ItemDescription { get => _itemdescription; set => SetProperty(ref _itemdescription, value); }



        /// <summary>
        /// Remark
        /// </summary>
        private string _remark;
        public string Remark { get => _remark; set => SetProperty(ref _remark, value); }



        /// <summary>
        /// IsFinal
        /// </summary>
        private bool _isfinal;
        public bool IsFinal { get => _isfinal; set { SetProperty(ref _isfinal, value);
                if (_isfinal)
                {
                    IsFinalText = "是";
                }
                else
                {
                    IsFinalText = "否";
                }
            } }

        private string _isfinalText;
        public string IsFinalText { get => _isfinalText; set => SetProperty(ref _isfinalText, value); }

        /// <summary>
        /// IsAudit
        /// </summary>
        private bool _isAudit;
        public bool IsAudit { get => _isAudit; set => SetProperty(ref _isAudit, value); }

        private long? creatoruserid;
        public long? CreatorUserId { get => creatoruserid; set => SetProperty(ref creatoruserid, value); }

        private string _creatorname;
        public string CreatorName { get => _creatorname; set => SetProperty(ref _creatorname, value); }
        private DateTime _creationtime;
        public DateTime CreationTime { get => _creationtime; set => SetProperty(ref _creationtime, value); }
        /// <summary>
        /// AuditDate
        /// </summary>
        private DateTime? _auditdate;
        public DateTime? AuditDate { get => _auditdate; set => SetProperty(ref _auditdate, value); }

        /// <summary>
        /// AuditUserId
        /// </summary>
        private long? _audituserId;
        public long? AuditUserId { get => _audituserId; set => SetProperty(ref _audituserId, value); }
        /// <summary>
        /// AuditUser
        /// </summary>
        private string _auditUsername;
        public string AuditUserName { get => _auditUsername; set => SetProperty(ref _auditUsername, value); }

        

    }
}
