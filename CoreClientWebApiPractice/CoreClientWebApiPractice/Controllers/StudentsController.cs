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
    public class StudentsController : Controller
    {
        private readonly DBcollageContext _context;
        HttpClient client = new HttpClient();
        string url = "http://localhost:40349/api/Students/";
        string courseurl = "http://localhost:40349/api/Courses/";

        public StudentsController(DBcollageContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchby, string search)
        {
            //var dBcollageContext = _context.Student.Include(s => s.Course);
            if (searchby == "Name")
            {
                return View(JsonConvert.DeserializeObject<List<Student>>
                    (await client.GetStringAsync(url + "searchbyname?name=" + search)).ToList());
            }
            //else if (searchby == "Age")
            //{
            //    return View(JsonConvert.DeserializeObject<List<Student>>
            //        (await client.GetStringAsync(url + "searchbyage?age=" + search)).ToList());
            //}
            //else if (searchby == "Gender")
            //{
            //    return View(JsonConvert.DeserializeObject<List<Student>>
            //        (await client.GetStringAsync(url + "searchbygender?gender=" + search)).ToList());
            //}
            //else if (searchby == "Year")
            //{
            //    return View(JsonConvert.DeserializeObject<List<Student>>
            //        (await client.GetStringAsync(url + "searchbyyear?year=" + search)).ToList());
            //}
            else if (searchby == "Course")
            {
                return View(JsonConvert.DeserializeObject<List<Student>>
                    (await client.GetStringAsync(url + "searchbycourse?course=" + search)).ToList());
            }
            return View(JsonConvert.DeserializeObject<List<Student>>(
                await client.GetStringAsync(url)).ToList());
            
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var student = await _context.Student
            //    .Include(s => s.Course)
            //    .FirstOrDefaultAsync(m => m.StudentId == id);
            var student = JsonConvert.DeserializeObject<Student>(
               await client.GetStringAsync(url + id));
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,StudentName,CourseId")] Student student)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(student);
                await client.PostAsJsonAsync<Student>(url, student);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseName", student.CourseId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var student = await _context.Student.FindAsync(id);
            var student = JsonConvert.DeserializeObject<Student>(
               await client.GetStringAsync(url + id));
            if (student == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseName", student.CourseId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,StudentName,CourseId")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(student);
                    //await _context.SaveChangesAsync();
                    await client.PutAsJsonAsync<Student>(url + id.ToString(), student);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            var course = JsonConvert.DeserializeObject<IEnumerable<Student>>
                (await client.GetStringAsync(courseurl));
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseName", student.CourseId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var student = await _context.Student
            //    .Include(s => s.Course)
            //    .FirstOrDefaultAsync(m => m.StudentId == id);
            var student = JsonConvert.DeserializeObject<Student>(
                await client.GetStringAsync(url + id));
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //var student = await _context.Student.FindAsync(id);
            //_context.Student.Remove(student);
            //await _context.SaveChangesAsync();
            await client.DeleteAsync(url + id);
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
