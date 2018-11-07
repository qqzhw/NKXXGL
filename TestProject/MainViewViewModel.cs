using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class MainViewViewModel
    {
        public MainViewViewModel()
        {
            this.Items = new ObservableCollection<Item>();
            for (int i = 0; i < 10; i++)
            {
                Items.Add(new Item {  Id = i, No = $"1-{i+1}",Name=(i+10).ToString()});
            }
        }

        public ObservableCollection<Item> Items { get; set; }


    }

    public class Item
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }

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
    }
}
