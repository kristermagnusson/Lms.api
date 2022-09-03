using Bogus.DataSets;
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
    public class ModuleRepository : IModuleRepository
    {
        private readonly LmsapiContext db = null!;
        public ModuleRepository(LmsapiContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void Add(Module module)
        {
            db.Module.Add(module);
        }

        public async Task<bool> AnyAsync(int? id)
        {
            return (db.Module?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<Module> FindAsync(int? id)
        {
            var module = await db.Module.FindAsync(id);
            return module;
        }
    

        public async Task<IEnumerable<Module>> GetAllModules()
        {
            return await db.Module.ToListAsync();
            
        }
        //public async Task<Module> GetModules(int? id) 
        //{
        //  var  module= await db.Module.FindAsync(id);
        //    return module;
        //}


        public async Task<Module> GetModules(string title)
            
        {
            var module =  db.Module.FirstOrDefault(l => l.Title == title);
            return module;
        }

        public void Remove(Module module)
        {
            db.Module.Remove(module);
        }

        public void Update(Module module)
        {
            db.Entry(module).State = EntityState.Modified;
        }

        
        
    }
}
