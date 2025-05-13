using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ST10261874_PROG7311.Data;
using ST10261874_PROG7311.Models;
using ST10261874_PROG7311.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ST10261874_PROG7311.Controllers
{
    [Authorize(Roles = "Employee")] //only allow access to users in employee role
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context; //inject application db context
        }

        public IActionResult Index()
        {
            return View(); //load employee dashboard view
        }

        [HttpGet]
        public IActionResult AddFarmer()
        {
            return View(); //display form to add a farmer
        }

        [HttpPost]
        public async Task<IActionResult> AddFarmer(FarmerViewModel model)
        {
            if (ModelState.IsValid) //check if form data is valid
            {
                //create new farmer entity from view model
                var farmer = new Farmer
                {
                    UserName = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address
                };
                _context.Farmers.Add(farmer); //add to database
                await _context.SaveChangesAsync(); //save changes
                return RedirectToAction(nameof(ViewFarmers)); //redirect to farmers list
            }
            return View(model); //if invalid, return same view with errors
        }

        public IActionResult ViewFarmers()
        {
            //get all farmers and map to view model list
            var farmers = _context.Farmers
                .Select(farmer => new FarmerViewModel
                {
                    FarmerId = farmer.Id,
                    Name = farmer.UserName,
                    Email = farmer.Email,
                    PhoneNumber = farmer.PhoneNumber,
                    Address = farmer.Address
                })
                .ToList();

            return View(farmers); //return view with farmer list
        }

        public IActionResult ViewFarmerProducts(string email)
        {
            //get all products belonging to farmer by email
            var products = _context.Products
                .Include(p => p.Farmer) //include farmer relationship
                .Where(p => p.Farmer.Email == email)
                .Select(product => new ProductViewModel
                {
                    Name = product.Name,
                    Category = product.Category,
                    ProductionDate = product.ProductionDate,
                    FarmerEmail = product.Farmer != null ? product.Farmer.Email : null //handle null farmer
                })
                .ToList();

            return View(products); //show products of selected farmer
        }

        public IActionResult AllProducts(string productName, string FarmerEmail, string category, DateTime? startDate, DateTime? endDate)
        {
            //start with all products including related farmer data
            var query = _context.Products
                .Include(p => p.Farmer)
                .AsQueryable();

            //filter by product name if provided
            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(p => p.Name.Contains(productName));
            }

            //filter by farmer email or name if provided
            if (!string.IsNullOrWhiteSpace(FarmerEmail))
            {
                string lower = FarmerEmail.ToLower();
                query = query.Where(p =>
                    p.Farmer != null &&
                    (
                        p.Farmer.UserName.ToLower().Contains(lower) ||
                        p.Farmer.Email.ToLower().Contains(lower)
                    ));
            }

            //filter by category if provided
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }

            //filter by start date if provided
            if (startDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= startDate.Value);
            }

            //filter by end date if provided
            if (endDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= endDate.Value);
            }

            //project filtered results into view model
            var products = query.Select(p => new ProductViewModel
            {
                Name = p.Name,
                Category = p.Category,
                ProductionDate = p.ProductionDate,
                FarmerEmail = p.Farmer != null ? p.Farmer.Email : null
            }).ToList();

            return View(products); //return filtered product list
        }

        public IActionResult SearchFarmers(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction(nameof(ViewFarmers)); //return all if query is empty
            }

            var normalizedQuery = query.Trim().ToLower(); //make query case insensitive

            //filter farmers by matching any of the fields
            var farmers = _context.Farmers
                .Where(f =>
                    f.UserName.ToLower().Contains(normalizedQuery) ||
                    f.Email.ToLower().Contains(normalizedQuery) ||
                    f.PhoneNumber.ToLower().Contains(normalizedQuery) ||
                    f.Address.ToLower().Contains(normalizedQuery))
                .Select(farmer => new FarmerViewModel
                {
                    FarmerId = farmer.Id,
                    Name = farmer.UserName,
                    Email = farmer.Email,
                    PhoneNumber = farmer.PhoneNumber,
                    Address = farmer.Address
                })
                .ToList();

            return View("ViewFarmers", farmers); //return filtered farmer list
        }

        public IActionResult SearchProducts(string productName, string farmerName, string category, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Products.AsQueryable(); //start base query

            //apply product name filter
            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(p => p.Name.Contains(productName));
            }

            //apply farmer name filter (note: uses any match on farmer table)
            if (!string.IsNullOrWhiteSpace(farmerName))
            {
                query = query.Where(p => _context.Farmers
                    .Any(f => f.UserName.Contains(farmerName)));
            }

            //apply category filter
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }

            //apply production date range filters
            if (startDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= endDate.Value);
            }

            //convert result to view model
            var products = query.Select(p => new ProductViewModel
            {
                Name = p.Name,
                Category = p.Category,
                ProductionDate = p.ProductionDate,
                FarmerEmail = p.Farmer.Email
            }).ToList();

            return View("AllProducts", products); //return filtered products in AllProducts view
        }
    }
}
