using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FireSafe.Data;
using FireSafe.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;

namespace FireSafe.Controllers
{
    public class UploadImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UploadImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UploadImages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UploadImages.Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UploadImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadImage = await _context.UploadImages
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (uploadImage == null)
            {
                return NotFound();
            }

            return View(uploadImage);
        }

        // GET: UploadImages/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: UploadImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,ImageFile,ImageName,ImageDescription,UserId")] UploadImage uploadImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uploadImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", uploadImage.UserId);
            return View(uploadImage);
        }

        // GET: UploadImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadImage = await _context.UploadImages.FindAsync(id);
            if (uploadImage == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", uploadImage.UserId);
            return View(uploadImage);
        }

        // POST: UploadImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageId,ImageFile,ImageName,ImageDescription,UserId")] UploadImage uploadImage)
        {
            if (id != uploadImage.ImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uploadImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UploadImageExists(uploadImage.ImageId))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", uploadImage.UserId);
            return View(uploadImage);
        }

        // GET: UploadImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadImage = await _context.UploadImages
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (uploadImage == null)
            {
                return NotFound();
            }

            return View(uploadImage);
        }

        // POST: UploadImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uploadImage = await _context.UploadImages.FindAsync(id);
            _context.UploadImages.Remove(uploadImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UploadImageExists(int id)
        {
            return _context.UploadImages.Any(e => e.ImageId == id);
        }

        
        private IHostingEnvironment _hostingEnv;
        public UploadImagesController(IHostingEnvironment hostingEnv)
            {
                _hostingEnv = hostingEnv;
            }
        [HttpPost]
        public async Task<IActionResult> UploadImage([Bind("ImageId,ImageFile,ImageName,ImageDescription,UserId")]UploadImage model, IFormFile ImageFile)
            {
                if (ModelState.IsValid)
                {
                    var filename = ContentDispositionHeaderValue.Parse(ImageFile.ContentDisposition).FileName.Trim('"');
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", ImageFile.FileName);
                    using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    model.ImageFile = filename;
                    _context.Add(model);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }


        }
    }
