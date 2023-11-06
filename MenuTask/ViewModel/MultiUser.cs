using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenuTask.Models;

namespace MenuTask.Models
{
    public class MultiUser
    {
        public IEnumerable<USER_PERMISSIONS> USER_PERMISSIONS { get; set; }
        public IEnumerable<Menu> Menus { get; set; }
        public IEnumerable<TELE_CALLER_MASTER> TELE_CALLER_MASTERS { get; set; }
    }


}
