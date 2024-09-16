using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.API.Models
{
    public class ApiResponse
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}