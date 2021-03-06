using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAutoFillServer.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime PaidDay { get; set; }
        public string VerificationToken { get; set; }
        public string CurrentToken { get; set; }
        public string Role { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
