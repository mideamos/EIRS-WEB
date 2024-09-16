using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Common
{
    public class DropDownListResult
    {
        public int id { get; set; }

        public string text { get; set; }

        public object AdditionalData { get; set; }
    }

    public class DropDownListResult<T>
    {
        public T Id { get; set; }

        public string Text { get; set; }

        public object AdditionalData { get; set; }
    }
}
