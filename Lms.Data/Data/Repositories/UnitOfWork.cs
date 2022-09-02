using Lms.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly LmsapiContext  db;


        public ICourseRepository CourseRepository { get; }

        public IModuleRepository ModuleRepository { get; }

         public UnitOfWork(LmsapiContext db)
        {
            this.db = db;
            CourseRepository = new CourseRepository(db);
            ModuleRepository = new ModuleRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
