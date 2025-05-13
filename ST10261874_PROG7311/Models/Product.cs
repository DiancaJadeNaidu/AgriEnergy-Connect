using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10261874_PROG7311.Models
{
    public class Product
    {
        public int ProductID { get; set; } //primary key for product

        [Required]
        public string FarmerID { get; set; } = string.Empty; //string identifier for farmer (could be application user id)

        [Required]
        public string Name { get; set; } = string.Empty; //name of the product

        [Required]
        public string Category { get; set; } = string.Empty; //product category (e.g., fruit, vegetable)

        [Required]
        public DateTime ProductionDate { get; set; } = DateTime.Now; //date when the product was produced

        public int? FarmerDbId { get; set; } //foreign key reference to farmer table

        [ForeignKey("FarmerDbId")]
        public Farmer? Farmer { get; set; } //navigation property to associated farmer
    }
}
