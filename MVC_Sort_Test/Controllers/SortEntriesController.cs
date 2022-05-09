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

        // GET: SortEntries/Details/#
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
        public async Task<IActionResult> Create([Bind("Id,OriginalCSV")] SortEntry sortEntry)
        {
            var watch = new Stopwatch();
            List<int> ints = new List<int>();
            List<int> sortedInts = new List<int>();
            string[] split = sortEntry.OriginalCSV.Split(',');
            sortEntry.DateAdded = DateTime.Now;
            //Parse ints and skip int if not a int.
            //regex catchs all formatting issuses except for number being <||> max/min for a int.
            //try cath block filers out the unusable numbers
            foreach (var s in split)
            {
                try
                {
                    ints.Add(int.Parse(s));
                }
                catch (Exception)
                {
                   //skip this entry
                }
            }

            //Do the sort depending on the direction selected
            if (sortEntry.SortOrder == 1)
            {
                watch.Restart();
                // Sort with LINQ
                /* var sorted = ints.OrderBy(num => num).ToArray(); */
                // Sort with array.sort()
                ints.Sort();
                watch.Stop();

                sortEntry.SortedCSV = string.Join<int>(",", ints);
                sortEntry.SortTime = watch.Elapsed.TotalMilliseconds;
            }
            else
            {
                watch.Restart();
                // Sort with LINQ
                /* var sorted = ints.OrderByDescending(num => num).ToArray(); */
                // Sort with array.sort()
                ints.Sort();
                ints.Reverse();
                watch.Stop();
                sortEntry.SortedCSV = string.Join<int>(",", ints);
                sortEntry.SortTime = watch.Elapsed.TotalMilliseconds;
            }
            
            //Check final sortEntry is valid and insert into databse.      
            if (ModelState.IsValid)
            {                
                _context.Add(sortEntry);               
                await _context.SaveChangesAsync();
                //redirect to details of newly added entry
                return RedirectToAction(nameof(Details), new { id = sortEntry.Id });
            }
           
            //Returns to create view is something goes wrong
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
            for (int i = 0; i < 1010; i++)
            {
                //Build originalCSV
                List<int> OGList = new List<int>();                                         
                var randIntCount = Extensions.ThreadSafeRandom.Next(2, 1000);
                var randDirection = (Extensions.ThreadSafeRandom.Next(0, 2) == 1) ? 1 : -1;
                for (int j = 0; j < randIntCount; j++)
                {
                    OGList.Add(Extensions.ThreadSafeRandom.Next(0,5000));
                }
                //Build new entry with this originalCSV
                var newEntry = new SortEntry
                {
                    DateAdded = DateTime.Now,
                    SortedCSV = "",
                    OriginalCSV = string.Join<int>(",", OGList),
                    SortOrder = randDirection
                };
                //Add new entry to list of new unsorted entries
                UnsortedEntries.Add(newEntry);              
            }


            //Generate SortedCVS
            List<int> ints = new List<int>(); 
            var count = 0;
            foreach (var item in UnsortedEntries)
            {
                ints.Clear();
                var split = item.OriginalCSV.Split(',');               
                foreach (var s in split)
                {
                    ints.Add(int.Parse(s));
                }

                //Sort in selected direction
                if (item.SortOrder == 1)
                {                 
                    watch.Restart();
                    // Sort with LINQ
                    /* var sorted = ints.OrderBy(num => num).ToArray(); */
                    // Sort with array.sort()             
                    ints.Sort();                   
                    watch.Stop();
                    item.SortedCSV = string.Join<int>(",", ints);
                    item.SortTime = watch.Elapsed.TotalMilliseconds;

                }
                else
                {
                    watch.Restart();
                    // Sort with LINQ
                    /* var sorted = ints.OrderByDescending(num => num).ToArray(); */
                    // Sort with array.sort()
                    ints.Sort();
                    ints.Reverse();
                    watch.Stop();
                    item.SortedCSV = string.Join<int>(",", ints);
                    item.SortTime = watch.Elapsed.TotalMilliseconds;
                }
                //Check model is valid
                //The count check is there to skip over the first 10 entries
                //The first few sorts always seem to be slow and this is a effort to filter them out
                if (ModelState.IsValid && count > 10)
                {
                    _context.Add(item);
                }
                count++;
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
            var sortEntry = _context.SortEntry.AsAsyncEnumerable<SortEntry>();
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

            //Build JSON
            var json = JsonSerializer.Serialize(sortEntry);

            //Read the JSON data into Byte Array.
            var utf8 = new UTF8Encoding();
            byte[] bytes = utf8.GetBytes(json);
            
            //Send the File to Download.
            return File(bytes, "application/octet-stream", sortEntry.Id+".json");
        }

        private bool SortEntryExists(int id)
        {
            return _context.SortEntry.Any(e => e.Id == id);
        }
    }
}
