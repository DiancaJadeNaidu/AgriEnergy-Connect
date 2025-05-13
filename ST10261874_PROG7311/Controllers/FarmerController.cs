using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ST10261874_PROG7311.Data;
using ST10261874_PROG7311.Models;
using ST10261874_PROG7311.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace ST10261874_PROG7311.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<FarmerController> _logger;

        public FarmerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<FarmerController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("Farmer/AddProduct")]
        public IActionResult AddProduct()
        {
            ViewData["ActivePage"] = "Products";

            var farmers = _context.Farmers
                .Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = $"{f.UserName} ({f.Email})"
                }).ToList();

            var model = new ProductViewModel
            {
                Farmers = farmers
            };

            return View(model);
        }

        [HttpPost]
        [Route("Farmer/AddProduct")]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var selectedFarmer = await _context.Farmers.FindAsync(model.SelectedFarmerDbId);
                if (selectedFarmer == null)
                {
                    ModelState.AddModelError("", "Selected farmer not found.");
                }
                else
                {
                    var currentUser = await _userManager.GetUserAsync(User);

                    var product = new Product
                    {
                        FarmerDbId = model.SelectedFarmerDbId,
                        Name = model.Name,
                        Category = model.Category,
                        ProductionDate = model.ProductionDate
                    };

                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Product added successfully.");

                    //add Toastr Success Message in TempData
                    TempData["SuccessMessage"] = "Product added successfully! Do you want to add another product?";

                    //redirect to AskForNextAction to ask the farmer if they want to add another product
                    return RedirectToAction(nameof(AskForNextAction));
                }
            }

            //re-populate dropdown if validation fails
            model.Farmers = _context.Farmers
                .Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = $"{f.UserName} ({f.Email})"
                }).ToList();

            ViewData["ActivePage"] = "Products";
            return View(model);
        }

        //action to ask the farmer if they want to add another product or go to the home page
        [HttpGet]
        public IActionResult AskForNextAction()
        {
            //retrieve the success message passed through TempData
            var message = TempData["SuccessMessage"]?.ToString();
            ViewData["Message"] = message;

            return View();
        }

        //POST action to handle user choice (Add Another or go to Home)
        [HttpPost]
        public IActionResult AskForNextAction(string action)
        {
            if (action == "AddAnother")
            {
                return RedirectToAction(nameof(AddProduct)); //redirect back to Add Product page
            }
            else
            {
                return RedirectToAction(nameof(Index), "Farmer"); //redirect to Home page
            }
        }

        [HttpGet]
        [Route("Farmer/Index")]
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Home"; //highlights "Home" tab
            return View();
        }

        public IActionResult Support()
        {
            ViewData["ActivePage"] = "Support"; //highlights "Support" tab
            return View();
        }

        public IActionResult CollaborateAndGetFunded()
        {
            ViewData["ActivePage"] = "CollaborateAndGetFunded"; //highlights "Collab..." tab
            return View();
        }

        public IActionResult LearnAndGrow()
        {
            ViewData["ActivePage"] = "LearnAndGrow"; //highlights "learnandgrow" tab
            return View();
        }
    }
}
