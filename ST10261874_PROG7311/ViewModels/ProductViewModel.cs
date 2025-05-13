using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ST10261874_PROG7311.ViewModels
{
    public class ProductViewModel
    {
        [Required(ErrorMessage = "Product Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Production Date is required")]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        [Required(ErrorMessage = "Farmer selection is required")]
        public int SelectedFarmerDbId { get; set; }

        public List<SelectListItem>? Farmers { get; set; } 

        public string? FarmerEmail { get; set; } 
    }
}
