using System;

namespace FinalAutoFillServer.DTO
{
    public class UserDTO
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
        public string Role { get; set; }
    }
}
