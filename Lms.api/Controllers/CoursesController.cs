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
using AutoMapper;
using Lms.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        // private readonly LmsapiContext _context;
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;
        public CoursesController(/*LmsapiContext context*/ IUnitOfWork uow, IMapper mapper)
        {
           // _context = context;
            this.uow = uow;
            this.mapper = mapper;
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
            var course =  await uow.CourseRepository.GetAllCourses();
            if (course==null) return NotFound();
            var courseDto=mapper.Map<IEnumerable<CourseDto>>(course);
            return Ok (courseDto);

        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            //if (_context.Course == null)
           
            
             // return NotFound();
          
            //var course = await _context.Course.FindAsync(id);
            var course = await uow.CourseRepository.GetCourse(id);
            if (course == null)
            {
                return NotFound();
            }
         var  courseDto= mapper.Map<CourseDto>(course);

            return Ok (courseDto);
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
        public async Task<ActionResult<Course>> PostCourse(CourseDto courseDto)
        {
            //if (_context.Course == null)
            if (uow.CourseRepository == null)
            {
              return Problem("Entity set 'LmsapiContext.Course'  is null.");
            }
            if (courseDto == null) return BadRequest();
            //_context.Course.Add(course);
            var course=mapper.Map<Course>(courseDto);
            uow.CourseRepository.Add(course);
            //await _context.SaveChangesAsync();
            await uow.CompleteAsync();
            return CreatedAtAction("GetCourse", new { id = course.Id }, courseDto);
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

        [HttpPatch("{courseId}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int courseId, JsonPatchDocument<CourseDto> patchDocument)
        {
            var course = await uow.CourseRepository.GetCourse(courseId);
            if (course== null)  return NotFound();
            var courseDto = mapper.Map<CourseDto>(course);
            patchDocument.ApplyTo(courseDto,ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            mapper.Map(courseDto, course);
            await uow.CompleteAsync();
            return Ok(mapper.Map<CourseDto>(course));
        }

    }
}
