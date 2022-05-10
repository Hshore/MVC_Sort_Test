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
        public async Task<IActionResult> Create([Bind("Id,OriginalCSV,SortOrder")] SortEntry sortEntry)
        {
            
            sortEntry.Sort();
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
        public async Task<IActionResult> CreateRandom(int? id)
        {          
           //Build new entries
            for (int i = 0; i < id; i++)
            {              
                SortEntry newEntry = new SortEntry();
                newEntry.GenerateRandomOriginalCSV();
                newEntry.Sort();
                //Add new entry to list of new unsorted entries
                if (ModelState.IsValid)
                {
                    _context.Add(newEntry);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // UNUSED
       /* // GET: SortEntries/Edit/5
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
        }*/

        // UNUSED
       /* // POST: SortEntries/Edit/5
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
*/

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
            
            await foreach (var item in _context.SortEntry.AsAsyncEnumerable<SortEntry>())
            {
                _ = _context.SortEntry.Remove(item);       
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
        // GET: SortEntries/DownloadAllFile
        public async Task<IActionResult> DownloadAllFile(int id)
        {
            // Get items            
            List<SortEntry> sortEntriesList = new List<SortEntry>();
            await foreach (var item in _context.SortEntry.AsAsyncEnumerable<SortEntry>())
            {
                sortEntriesList.Add(item);
            }

            //Build JSON
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(sortEntriesList, options);

            //Read the JSON data into Byte Array.
            var utf8 = new UTF8Encoding();
            byte[] bytes = utf8.GetBytes(json);

            //Send the File to Download.
            return File(bytes, "application/octet-stream",  "allSortEntries.json");
        }

        private bool SortEntryExists(int id)
        {
            return _context.SortEntry.Any(e => e.Id == id);
        }
    }
}
