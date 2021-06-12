using System.ComponentModel.DataAnnotations;

namespace FinalAutoFillServer.DTO
{
    public class CheckAppUser
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
