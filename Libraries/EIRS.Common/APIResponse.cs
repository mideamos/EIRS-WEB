using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Common
{
    public class APIResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }
    }

    //public class APIPaginationResponse<T>
    //{
    //    public bool Success { get; set; }

    //    public string Message { get; set; }

    //    public T Result { get; set; }

    //    public int TotalCount { get; set; }

    //    public int TotalPages { get; set; }
    //}
}