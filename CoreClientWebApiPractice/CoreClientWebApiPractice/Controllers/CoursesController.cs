using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreClientWebApiPractice.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CoreClientWebApiPractice.Controllers
{
    public class CoursesController : Controller
    {
        private readonly DBcollageContext _context;
        HttpClient client = new HttpClient();
        string url = "http://localhost:40349/api/Courses/";

        public CoursesController(DBcollageContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchby, string search)
        {
            if (searchby == "CourseName")
            {
                return View(JsonConvert.DeserializeObject<List<Course>>
                (await client.GetStringAsync(url + "searchbyname?name=" + search)).ToList());
            }
            //else if (searchby == "Duration")
            //{
            //    return View(JsonConvert.DeserializeObject<List<Course>>
            //    (await client.GetStringAsync(url + "searchbyduration?duration=" + search)).ToList());
            //}
            return View(JsonConvert.DeserializeObject<List<Course>>
               (await client.GetStringAsync(url)).ToList());
            
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var course = await _context.Course
            //    .FirstOrDefaultAsync(m => m.CourseId == id);
            var course = JsonConvert.DeserializeObject<Course>
                (await client.GetStringAsync(url + id));
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName")] Course course)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(course);
                //await _context.SaveChangesAsync();
                await client.PostAsJsonAsync<Course>(url, course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var course = await _context.Course.FindAsync(id);
            var course = JsonConvert.DeserializeObject<Course>
                (await client.GetStringAsync(url + id));
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(course);
                    //await _context.SaveChangesAsync();
                    await client.PutAsJsonAsync<Course>(url + id.ToString(), course);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var course = await _context.Course
            //    .FirstOrDefaultAsync(m => m.CourseId == id);
            var course = JsonConvert.DeserializeObject<Course>
                (await client.GetStringAsync(url + id));
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //var course = await _context.Course.FindAsync(id);
            //_context.Course.Remove(course);
            //await _context.SaveChangesAsync();
            await client.DeleteAsync(url + id);
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
