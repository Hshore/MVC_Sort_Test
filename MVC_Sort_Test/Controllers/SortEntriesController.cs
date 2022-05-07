#nullable disable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Sort_Test.Data;
using MVC_Sort_Test.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace MVC_Sort_Test.Controllers
{
    public class SortEntriesController : Controller
    {
        private readonly MVC_Sort_TestContext _context;

        public SortEntriesController(MVC_Sort_TestContext context)
        {
            _context = context;
        }

        // GET: SortEntries
        public async Task<IActionResult> Index()
        {
           
            return View(await _context.SortEntry.ToListAsync());
        }

        // GET: SortEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sortEntry = await _context.SortEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sortEntry == null)
            {
                return NotFound();
            }

            return View(sortEntry);
        }

        // GET: SortEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SortEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrigonalCSV,SortOrder")] SortEntry sortEntry)
        {

            //
           

            sortEntry.DateAdded = DateTime.Now;
            string[] split = sortEntry.OrigonalCSV.Split(',');          
            List<int> OGList = new List<int>();
            List<int> SortedList = new List<int>();

            //Confirm items are ints
            //This try catch block may be redundant because of the regx validation in SortEntries.cs
            foreach (var item in split)
            {
                try
                {
                    int thisInt = int.Parse(item);
                    OGList.Add(thisInt);
                }
                catch (Exception)
                {
                    return View(sortEntry);
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
                }                
                watch.Stop();
                sortEntry.SortedCSV = string.Join<int>(",", SortedList);
                sortEntry.SortTime = watch.Elapsed.TotalMilliseconds;

            }
            else
            {
                watch.Restart();
                foreach (var n in decend)
                {
                    SortedList.Add(n);
                }
                watch.Stop();
                sortEntry.SortedCSV = string.Join<int>(",", SortedList);
                sortEntry.SortTime = watch.Elapsed.TotalMilliseconds;
            }

            var newEntry = new SortEntry
            {
                DateAdded = DateTime.Now,
                SortedCSV = "1,2,3,4",
                SortTime = 0.1,
                OrigonalCSV = "4,3,2,1",
                SortOrder = -1,
            };
           
            if (ModelState.IsValid)
            {                
                _context.Add(sortEntry);
               // _context.Add(newEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = sortEntry.Id });
            }
           
            return View(sortEntry);
        }

        // POST: SortEntries/CreateRandom
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRandom()
        {
            //List of new entries with empty sortedCSV
            List<SortEntry> UnsortedEntries = new List<SortEntry>();
            var watch = new Stopwatch();

           //Build new entries
            for (int i = 0; i < 1000; i++)
            {
                List<int> OGList = new List<int>();                             
              
                var randIntCount = Extensions.ThreadSafeRandom.Next(2, 400);
                var randDirection = (Extensions.ThreadSafeRandom.Next(0, 2) == 1) ? 1 : -1;
                for (int j = 0; j < randIntCount; j++)
                {
                    OGList.Add(Extensions.ThreadSafeRandom.Next(0,5000));
                }

                var newEntry = new SortEntry
                {
                    DateAdded = DateTime.Now,
                    SortedCSV = "",
                    OrigonalCSV = string.Join<int>(",", OGList),
                    SortOrder = randDirection
                };

                UnsortedEntries.Add(newEntry);              
            }

            List<int> ints = new List<int>();
            List<int> sortedInts = new List<int>();
            

            //Generate SortedCVS
            foreach (var item in UnsortedEntries)
            {
                ints.Clear();
                var split = item.OrigonalCSV.Split(',');
               
                foreach (var s in split)
                {
                    ints.Add(int.Parse(s));
                }

              //  ints.Sort();
               // ints.Reverse();
                if (item.SortOrder == 1)
                {
                   
                    watch.Restart();
                  //  var sorted = ints.OrderBy(num => num).ToArray();                   
                    ints.Sort();                   
                    watch.Stop();
                    item.SortedCSV = string.Join<int>(",", ints);
                    item.SortTime = watch.Elapsed.TotalMilliseconds;

                }
                else
                {
                    watch.Restart();
                   // var sorted = ints.OrderByDescending(num => num).ToArray();
                    ints.Sort();
                    ints.Reverse();
                    watch.Stop();
                    item.SortedCSV = string.Join<int>(",", ints);
                    item.SortTime = watch.Elapsed.TotalMilliseconds;
                }

                if (ModelState.IsValid)
                {
                    _context.Add(item);


                    // return RedirectToAction(nameof(Index));
                }

            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: SortEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sortEntry = await _context.SortEntry.FindAsync(id);
            if (sortEntry == null)
            {
                return NotFound();
            }
            return View(sortEntry);
        }

        // POST: SortEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateAdded,OrigonalCSV,SortedCSV")] SortEntry sortEntry)
        {
            if (id != sortEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sortEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SortEntryExists(sortEntry.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sortEntry);
        }

        // GET: SortEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sortEntry = await _context.SortEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sortEntry == null)
            {
                return NotFound();
            }

            return View(sortEntry);
        }

        // POST: SortEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sortEntry = await _context.SortEntry.FindAsync(id);
            _context.SortEntry.Remove(sortEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: SortEntries/DeleteAll
        public IActionResult DeleteAll()
        {
            

            return View();
        }



        // POST: SortEntries/DeleteAll
        [HttpPost, ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAllConfirmed(int id)
        {
           IAsyncEnumerable<SortEntry> sortEntry = _context.SortEntry.AsAsyncEnumerable<SortEntry>();
            //  _context.SortEntry.Remove(sortEntry);
            //  await _context.SaveChangesAsync(); 
            await foreach (var item in sortEntry)
            {
              
                _context.SortEntry.Remove(item);

            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: SortEntries/DownloadFile
        public async Task<IActionResult> DownloadFile(int id)
        {
            // Get item
            var sortEntry = await _context.SortEntry
               .FirstOrDefaultAsync(m => m.Id == id);
            if (sortEntry == null)
            {
                return NotFound();
            }

            //Build the File Path.
            // string path = @"C:\Users\Hayden\source\repos\MVC_Sort_Test\MVC_Sort_Test\Data\TextFile.txt";

            //Build JSON
            var json = JsonSerializer.Serialize(sortEntry);

            //Read the JSON data into Byte Array.
            var utf8 = new UTF8Encoding();
            byte[] bytes = utf8.GetBytes(json);           
            
            //Send the File to Download.
            return File(bytes, "application/octet-stream", sortEntry.Id+".json");
        }



        //GET SorEntries/Chart
        public IActionResult Chart()
        {
            return PartialView("Chart");
        }

        private bool SortEntryExists(int id)
        {
            return _context.SortEntry.Any(e => e.Id == id);
        }
    }
}
