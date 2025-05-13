using Microsoft.AspNetCore.Identity;

namespace ST10261874_PROG7311.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int FarmerID { get; set; } //link to farmer table
        public string Role { get; set; } = "Farmer"; //default role set to farmer
    }
}
