﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BaseData
{
    public partial class BuyCategory
    {
        public string No { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //上级编号
        public int ParentId { get; set; }


        public bool Published { get; set; }

        public int DisplayOrder { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public BuyCategory Parent { get; set; }

        public ObservableCollection<BuyCategory> Children { get; set; } = new ObservableCollection<BuyCategory>();

        public string GroupNo
        {
            get
            {
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
