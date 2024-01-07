using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class MenuScreenViewModel
    {
        public int MenuID { get; set; }

        public int ScreenID { get; set; }
        
        public string MenuIds { get; set; }

        public string ScreenIds { get; set; }

        public int MainScreenID { get; set; }
    }
}
