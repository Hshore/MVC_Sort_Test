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

        //POST Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrigonalCSV,SortOrder")] SortEntry sortEntry)
        {

            //
            sortEntry.DateAdded = DateTime.Now;
            List<int> OGList = new List<int>();
            List<int> SortedList = new List<int>();

            //Confirm string is csv and add items to list
            foreach (var item in sortEntry.OrigonalCSV.Split(','))
            {
                try
                {
                    int thisInt = int.Parse(item);
                    OGList.Add(thisInt);
                }
                catch (Exception)
                {
                    throw;
                }
            }


            //Sort
            var acend = from num in OGList
                        orderby num
                        select num;
            var decend = from num in OGList
                         orderby num descending
                         select num;
            var watch = new Stopwatch();

            if (sortEntry.SortOrder == 1)
            {
                watch.Restart();
                foreach (var n in acend)
                {
                    SortedList.Add(n);
                    sortEntry.SortedCSV += $"{n},";
                }
                watch.Stop();
                sortEntry.SortTime = watch.Elapsed.TotalMilliseconds;

            }
            else
            {
                watch.Restart();
                foreach (var n in decend)
                {
                    SortedList.Add(n);
                    sortEntry.SortedCSV += $"{n},";
                }
                watch.Stop();
                sortEntry.SortTime = watch.Elapsed.TotalSeconds;
            }

            if (ModelState.IsValid)
            {
                _context.Add(sortEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),nameof(SortEntries));
            }

            return View(sortEntry);
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