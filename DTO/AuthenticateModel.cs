using System.ComponentModel.DataAnnotations;


namespace FinalAutoFillServer.DTO
{
    public class AuthenticateModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
