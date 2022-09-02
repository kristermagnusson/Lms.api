using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Core.Entities;
using Lms.Data.Data;
using Lms.Data.Data.Repositories;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Lms.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
       // private readonly LmsapiContext _context;
        private readonly IUnitOfWork uow;
        public CoursesController(/*LmsapiContext context*/ IUnitOfWork uow)
        {
           // _context = context;
            this.uow = uow;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            //if (_context.Course == null)
            //if (uow.CourseRepository == null)
            //{
            //    return NotFound();
            //}
            //return await _context.Course.ToListAsync();
            var course = await uow.CourseRepository.GetAllCourses();
            return Ok (course);

        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            //if (_context.Course == null)
           
            
             // return NotFound();
          
            //var course = await _context.Course.FindAsync(id);
            var course = uow.CourseRepository.GetCourse(id);
            if (course == null)
            {
                return NotFound();
            }

            return await course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            //_context.Entry(course).State = EntityState.Modified;
            uow.CourseRepository.Update(course);

            try
            {
               // await _context.SaveChangesAsync();
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            //if (_context.Course == null)
            if (uow.CourseRepository == null)
            {
              return Problem("Entity set 'LmsapiContext.Course'  is null.");
          }
            //_context.Course.Add(course);
            uow.CourseRepository.Add(course);
            //await _context.SaveChangesAsync();
            await uow.CompleteAsync();
            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            //if (_context.Course == null)
            if(uow.CourseRepository == null)
            {
                return NotFound();
            }
            //var course = await _context.Course.FindAsync(id);
            var course = await uow.CourseRepository.GetCourse(id);
            if (course == null)
            {
                return NotFound();
            }

            //_context.Course.Remove(course);
            uow.CourseRepository.Remove(course);
            //await _context.SaveChangesAsync();
           await uow.CompleteAsync();
            return NoContent();
        }

        private async Task<bool> CourseExists(int id)
        {
            return await uow.CourseRepository.AnyAsync(id);
            //return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
