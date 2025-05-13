using System.ComponentModel.DataAnnotations;

namespace ST10261874_PROG7311.Models
{
    public class Farmer
    {
        public int Id { get; set; } //primary key for farmer

        [Required]
        public string Name { get; set; } = string.Empty; //farmer's full name

        public string Location { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; //farmer's email address

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty; //farmer's phone number

        public string Address { get; set; } = string.Empty; //full address (optional)

        public string UserName { get; set; } = string.Empty; //username used for login or display
    }
}
