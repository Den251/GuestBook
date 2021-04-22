using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class RecordsController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Record Record { get; set; }
        public RecordsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Record = new Record();
            if (id == null)
            {
                //create
                return View(Record);
            }
            //update
            Record = _db.Records.FirstOrDefault(u => u.Id == id);
            if (Record == null)
            {
                return NotFound();
            }
            return View(Record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Record.Id == 0)
                {
                    //create
                    _db.Records.Add(Record);
                }
                
                else
                {
                    _db.Records.Update(Record);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Record);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Records.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Records.FirstOrDefaultAsync(u => u.Id == id);
            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Records.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
