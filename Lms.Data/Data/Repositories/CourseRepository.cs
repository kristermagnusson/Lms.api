using Lms.Core.Entities;
using Lms.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    { 
        private readonly LmsapiContext db = null!;
        public CourseRepository(LmsapiContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void Add(Course course)
        {
            db.Course.Add(course);
        }

        public async Task<bool> AnyAsync(int? id)
        {
            return  (db.Course?.Any(e => e.Id == id)).GetValueOrDefault();           
        }

        public Task<Course> FindAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Course>> GetAllCourses(bool include)           
        {
            if (include)
            {
                return await db.Course
                    .Include(c=>c.Modules)
                    .ToListAsync();
            }
            return await db.Course.ToListAsync();
        }

        public async Task<Course?> GetCourse(int? id)
        {
           return await db.Course.FindAsync(id);
        }

        public  void Remove(Course course)
        {
            db.Course.Remove(course);
        }

        public void Update(Course course)
        {
            db.Entry(course).State = EntityState.Modified;
        }
    }
}
