using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.API.Models
{
    public class PagingParameterModel
    {
        const int maxPageSize = 10000;

        public int pageNumber { get; set; } = 1;

        public int _pageSize { get; set; } = 10;

        public int pageSize
        {

            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
    public class NewModel : NewPagingParameterModel
    {
        public string mdaName { get; set; } = null;
    }  
    public class NewPagingParameterModel : PagingParameterModel
    {
        public string filter { get; set; } = null;
        public int year { get; set; }
    } 
    public class NewPagingParameterModelII : PagingParameterModel
    {
        public int year { get; set; }
    }
}