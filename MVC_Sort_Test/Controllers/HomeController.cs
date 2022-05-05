using Microsoft.AspNetCore.Mvc;
using MVC_Sort_Test.Data;
using MVC_Sort_Test.Models;
using System.Diagnostics;

namespace MVC_Sort_Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MVC_Sort_TestContext _context;

        public HomeController(ILogger<HomeController> logger, MVC_Sort_TestContext context)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Home
        public IActionResult Index()
        {
            return View();
        }

        //GET: Home/SortEntries
        public IActionResult SortEntries()
        {
            return View();
        }

        //GET: Home/About
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}