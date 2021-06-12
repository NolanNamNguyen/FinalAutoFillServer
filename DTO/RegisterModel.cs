using System.ComponentModel.DataAnnotations;


namespace FinalAutoFillServer.DTO
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }
}
