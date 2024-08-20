using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
    public class RevenueValues
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SearchValue { get; set; }
        public List<Column> Columns { get; set; }
        public List<Order1> Order { get; set; }

        public class Column
        {
            public string Data { get; set; }
            public bool Searchable { get; set; }
            public bool Orderable { get; set; }
            public Search Search { get; set; }
        }

        public class Search
        {
            public string Value { get; set; }
            public bool Regex { get; set; }
        }

        public class Order1
        {
            public int Column { get; set; }
            public string Dir { get; set; } // "asc" or "desc"
        }
    }

}
