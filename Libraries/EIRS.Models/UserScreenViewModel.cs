using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class UserScreenViewModel
    {
        public int UserID { get; set; }

        public int ScreenID { get; set; }

        public string UserIds { get; set; }

        public string ScreenIds { get; set; }

    }
}
