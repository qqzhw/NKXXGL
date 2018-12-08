using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BusinessManages
{
    public class AuditMapping : BindableBase
    {

        /// <summary>
        /// Id
        /// </summary>
        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }



        /// <summary>
        /// DisplayOrder
        /// </summary>
        private int _displayOrder;
        public int DisplayOrder { get => _displayOrder; set => SetProperty(ref _displayOrder, value); }



        /// <summary>
        /// TenantId
        /// </summary>
        private int? _tenantId;
        public int? TenantId { get => _tenantId; set => SetProperty(ref _tenantId, value); }


        private int _businessauditid;
        public int BusinessAuditId { get => _businessauditid; set => SetProperty(ref _businessauditid, value); }

        private int _businesstypeid;
        public int BusinessTypeId { get => _businesstypeid; set => SetProperty(ref _businesstypeid, value); }

        private string _businesstypeName;
        public string BusinessTypeName { get => _businesstypeName; set => SetProperty(ref _businesstypeName, value); }

        private int _itemid;
        public int ItemId { get => _itemid; set => SetProperty(ref _itemid, value); }



        /// <summary>
        /// Status
        /// </summary>
        private int _status;
        public int Status
        {
            get => _status; set
            {
                SetProperty(ref _status, value);

                switch (_status)
                {
                    case 0:
                        StatusText = "未审核";
                        StatusColor = "#FFFF00";
                        break;
                    case 1:
                        StatusText = "已审核";
                        StatusColor = "#3cb371";
                        break;
                    case 2:
                        StatusText = "驳回";
                        StatusColor ="#ff8c00";
                        break; 
                    default:
                        StatusText = "未审核";
                        StatusColor = "#FFFF00";
                        break;
                }
            }
        }
        private string _statuscolor;
        public string StatusColor
        {
            get => _statuscolor; set => SetProperty(ref _statuscolor, value);
        }
        private string _statustext;
        public string StatusText { get => _statustext; set => SetProperty(ref _statustext, value); }

        /// <summary>
        /// AuditOpinion审核意见
        /// </summary>
        private string _auditOpinion;
        public string AuditOpinion { get => _auditOpinion; set => SetProperty(ref _auditOpinion, value); }



        /// <summary>
        /// CreatorUserId
        /// </summary>
        private long? _creatoruserId;
        public long? CreatorUserId { get => _creatoruserId; set => SetProperty(ref _creatoruserId, value); }

         
        /// <summary>
        /// CreationTime
        /// </summary>
        private DateTime _creationTime;
        public DateTime CreationTime { get => _creationTime; set => SetProperty(ref _creationTime, value); }



        /// <summary>
        /// AuditTime
        /// </summary>
        private DateTime? _auditTime;
        public DateTime? AuditTime { get => _auditTime; set => SetProperty(ref _auditTime, value); }

        private string _auditusername;
        public string AuditUserName { get => _auditusername; set => SetProperty(ref _auditusername, value); }

       
        private int _roleId;
        public int RoleId { get => _roleId; set => SetProperty(ref _roleId, value); }
        private string _rolename;
        public string RoleName { get => _rolename; set => SetProperty(ref _rolename, value); }
    }
}