using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mission6.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mission6.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieFormContext _context;

        public HomeController(MovieFormContext context)
        {
            _context = context;
        }

        // GET: Home/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: Home/Form (Create new movie)
        [HttpGet]
        public IActionResult Form()
        {
            // Populate the ViewBag with the list of categories for the dropdown
            ViewBag.CategoryList = new SelectList(_context.Categories, "CategoryId", "CategoryName");

            return View();
        }

        // POST: Home/Form (Process new movie)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Form(Movie response)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Add(response);
                _context.SaveChanges();
                return RedirectToAction("MovieList");
            }
            // If validation fails, repopulate the dropdown and show the form again.
            ViewBag.CategoryList = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(response);
        }

        // GET: Home/GetToKnow (Informational)
        public IActionResult GetToKnow()
        {
            return View();
        }

        // GET: Home/MovieList (List movies)
        public IActionResult MovieList()
        {
            var movies = _context.Movies
                .Include(m => m.Category)
                .Where(x => x.Director != null)
                .ToList();
            return View(movies);
        }

        // GET: Home/Details/5 (View details for a movie)
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _context.Movies
                .Include(m => m.Category)
                .FirstOrDefault(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = _context.Movies.Find(id);
            if (movie == null)
                return NotFound();

            // If needed, populate the category dropdown for the edit form
            ViewBag.CategoryList = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(movie);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Movie movie)
        {
            if (id != movie.MovieId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Movies.Any(e => e.MovieId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("MovieList");
            }
            // Repopulate the category list if ModelState is invalid.
            ViewBag.CategoryList = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(movie);
        }

        // DELETE: Home/Delete/5 (Handled directly without a confirmation page)
        // Note: We use a POST action here to follow best practices.
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = _context.Movies
                .Include(m => m.Category)
                .FirstOrDefault(m => m.MovieId == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
            return RedirectToAction("MovieList");
        }
       
        
        
    }
}