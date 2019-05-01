using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastWpfGridTest
{
    public class DataItem
    {
        public string A
        {
            get;
            set;
        }

        public string B
        {
            get;
            set;
        }
        public string C
        {
            get;
            set;
        }

        public string D
        {
            get;
            set;
        }

        public string E
        {
            get;
            set;
        }

        public string F
        {
            get;
            set;
        }

        public static IList<DataItem> Items()
        {
            var ret = new List<DataItem>();

            ret.Add(new DataItem(){ A = "1aaa", B = "1bbb", C = "1ccc", D = "1d" ,E = "1e", F = "1f" });
            //ret.Add(new DataItem() { A = "2aaa", B = "2bbb" });
            //ret.Add(new DataItem() { A = "3aaa", B = "3bbb" });
            //ret.Add(new DataItem() { A = "4aaa", B = "4bbb" });
            return ret;
        }
    }
}
