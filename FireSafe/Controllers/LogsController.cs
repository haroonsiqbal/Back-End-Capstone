using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FireSafe.Data;
using FireSafe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FireSafe.Controllers
{
    [Authorize]
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHostingEnvironment hostingEnvironment;

        public LogsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            hostingEnvironment = environment;
        }

        // GET: Logs
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userLogs = await _context.Logs.Where(l => l.UserId == user.Id)
                                            .Include(l => l.Category)
                                            .Include(l => l.Seller)
                                            .ToListAsync();
            return View(userLogs);
        }

        // GET: Logs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Logs
                .Include(l => l.Category)
                .Include(l => l.Seller)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // GET: Logs/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Type");
            ViewData["SellerId"] = new SelectList(_context.Sellers, "SellerId", "Name");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View(new CreateLogViewModel());
        }

        // POST: Logs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLogViewModel model)
        {

            ModelState.Remove("Log.UserId");
            ModelState.Remove("Log.User");
            if (ModelState.IsValid)
            {
                if (model.MyImage != null)
                {
                    var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    using (var myFile = new FileStream(filePath, FileMode.Create))
                    {
                        model.MyImage.CopyTo(myFile);
                    }
                    model.Log.FileName = uniqueFileName;

                    //to do : Save uniqueFileName  to your db table   
                }
                var user = await _userManager.GetUserAsync(HttpContext.User);
                model.Log.UserId = user.Id;
                
                _context.Add(model.Log);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Type", model.Log.CategoryId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "SellerId", "Name", model.Log.SellerId);
    
            return View(model);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        // GET: Logs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }

            var viewModel = new CreateLogViewModel()
            {
                Log = log
            };
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Type", log.CategoryId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "SellerId", "Name", log.SellerId);
            
            return View(viewModel);
        }

        // POST: Logs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateLogViewModel model)
        {
            if (id != model.Log.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Log.UserId");
            ModelState.Remove("Log.User");
            if (ModelState.IsValid)
            {
                var currentFileName = model.Log.FileName;
                if (model.MyImage != null && model.MyImage.FileName != currentFileName)
                {
                    if (currentFileName != null)
                    {
                        var images = Directory.GetFiles("wwwroot/images");
                        var fileToDelete = images.First(i => i.Contains(currentFileName));
                        System.IO.File.Delete(fileToDelete);
                    }
                    var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                    var imageDirectory = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    var filePath = Path.Combine(imageDirectory, uniqueFileName);
                    using (var myFile = new FileStream(filePath, FileMode.Create))
                    {
                        model.MyImage.CopyTo(myFile);
                    }
                    model.Log.FileName = uniqueFileName;
                }
                try
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    model.Log.UserId = user.Id;
                    _context.Update(model.Log);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogExists(model.Log.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Type", model.Log.CategoryId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "SellerId", "Name", model.Log.SellerId);
            
            return View(model);
        }

        // GET: Logs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Logs
                .Include(l => l.Category)
                .Include(l => l.Seller)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var log = await _context.Logs.FindAsync(id);
            var currentFileName = log.FileName;
            
                if (currentFileName != null)
                {
                    var images = Directory.GetFiles("wwwroot/images");
                    var fileToDelete = images.First(i => i.Contains(currentFileName));
                    System.IO.File.Delete(fileToDelete);
                }
            
            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogExists(int id)
        {
            return _context.Logs.Any(e => e.Id == id);
        }
    }
}
