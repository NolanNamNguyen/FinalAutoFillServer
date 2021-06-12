using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAutoFillServer.Entities
{
    public class Admin
    {
        public int AdminId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
