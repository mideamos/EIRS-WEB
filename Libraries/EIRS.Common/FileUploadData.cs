using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Common
{
    public class FileUploadData
    {
        public int FileID { get; set; }

        public int TablePKID { get; set; }

        public EnumList.Track IntTrack { get; set; }

        public string DocumentPath { get; set; }

        public string DocumentName { get; set; }

        public string DocumentLink { get; set; }
    }

    public class ExcelColumnList
    {
        public string ColumnName { get; set; }

        public string SortOrder { get; set; }
    }
}
