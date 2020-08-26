using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DaySix.Data;
using DaySix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DaySix.Controllers
{
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            var blogs = _context.Blogs.Include(b => b.Category).ToList();

            return View(blogs);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Blog blog)
        {
            if (blog.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (blog.Upload.ContentType != "image/jpeg" && blog.Upload.ContentType != "image/png" && blog.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (blog.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }
            }
            

            if (ModelState.IsValid)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmssff") + Path.GetExtension(blog.Upload.FileName);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    blog.Upload.CopyTo(stream);
                }

                blog.Photo = fileName;

                _context.Blogs.Add(blog);
                _context.SaveChanges();

                return RedirectToAction("index");
            }

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            return View(blog);
        }

        public IActionResult Edit(int id)
        {
            Blog blog = _context.Blogs.Find(id);

            if (blog == null) return NotFound();

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            return View(blog);
        }

        [HttpPost]
        public IActionResult Edit(Blog blog)
        {
            if (blog.Upload != null)
            {
                if (blog.Upload.ContentType != "image/jpeg" && blog.Upload.ContentType != "image/png" && blog.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (blog.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }
            }

            if (ModelState.IsValid)
            {
                if(blog.Upload != null)
                {
                    var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", blog.Photo);

                    System.IO.File.Delete(oldFile);

                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssff") + Path.GetExtension(blog.Upload.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        blog.Upload.CopyTo(stream);
                    }

                    blog.Photo = fileName;
                }

                _context.Entry(blog).State = EntityState.Modified;

                _context.SaveChanges();

                return RedirectToAction("index");
            }

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            return View(blog);
        }

        public IActionResult Delete(int id)
        {
            Blog blog = _context.Blogs.Find(id);

            if (blog == null) return NotFound();

            var photo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", blog.Photo);

            System.IO.File.Delete(photo);

            _context.Blogs.Remove(blog);

            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
