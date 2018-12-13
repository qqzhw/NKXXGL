using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BusinessManages
{

    public class BusinessAuditStatus : BindableBase
    {
        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }        
        /// <summary>
        /// DisplayOrder
        /// </summary>
        private int _displayOrder;
        public int DisplayOrder { get => _displayOrder; set => SetProperty(ref _displayOrder, value); }


        private long _roleId;
        public long RoleId { get => _roleId; set => SetProperty(ref _roleId, value); }

        private string _roleName;
        public string RoleName { get => _roleName; set => SetProperty(ref _roleName, value); }

        private int _businessAuditId;
        public int BusinessAuditId
        {
            get => _businessAuditId; set => SetProperty(ref _businessAuditId, value);
        }


        public int _entityId;
        public int EntityId { get => _entityId; set => SetProperty(ref _entityId, value); }

        private string _businessTypeName;
        public string BusinessTypeName { get => _businessTypeName; set => SetProperty(ref _businessTypeName, value); }

        private int _status = 0;
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
                        StatusColor = "#ff8c00";
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

    }
}
