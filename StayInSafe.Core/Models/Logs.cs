using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Models
{
    public class Logs
    {
        public int idLog { get; set; }
        public string usuario { get; set; }
        public string accion { get; set; }
        public string nombreMetodo { get; set; }
    }
}
