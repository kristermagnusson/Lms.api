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
using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.api.Controllers
{
     [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        //private readonly LmsapiContext _context;
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;
        public ModulesController(/*LmsapiContext context*/IMapper mapper, IUnitOfWork uow)
        {
         //   _context = context;
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModule()
        {
            // if (_context.Module == null)
            // {
            //    return NotFound();
            //}
            // return await _context.Module.ToListAsync();
            var module = await uow.ModuleRepository.GetAllModules();
            if (module ==null) return NotFound();
            var moduleDto= mapper.Map<IEnumerable<ModuleDto>>(module);
            return Ok(moduleDto);

        }

        // GET: api/Modules/5
        [HttpGet("{title}")]
        public async Task<ActionResult<Module>> GetModule(string title)
        {
         // if (_context.Module == null)
          //{
             // return NotFound();
         // }
            var @module = await uow.ModuleRepository.GetModules(title);

            if (@module == null)
            {
                return NotFound();
            }
            var moduleDto = mapper.Map<ModuleDto>(@module);
            return Ok (moduleDto);
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, Module @module)
        {
            if (id != @module.Id)
            {
                return BadRequest();
            }

            //_context.Entry(@module).State = EntityState.Modified;
            uow.ModuleRepository.Update(module);
            try
            {
                // await _context.SaveChangesAsync();
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await ModuleExists(id))
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

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(ModulePostDto modulePostDto)
        {
            // if (_context.Module == null)
            if (uow.CourseRepository == null)
            {
              return Problem("Entity set 'LmsapiContext.Module'  is null.");
          }
            //_context.Module.Add(@module);
            var module=mapper.Map<Module>(modulePostDto);
            uow.ModuleRepository.Add(module);
            //await _context.SaveChangesAsync();
            await uow.CompleteAsync();
            return CreatedAtAction("GetModule", new { id = module.Id }, modulePostDto);
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            //if (_context.Module == null)
            if (uow.ModuleRepository == null)
            {
                return NotFound();
            }
           // var @module = await _context.Module.FindAsync(id);
           var module=await uow.ModuleRepository.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            //_context.Module.Remove(@module);
            uow.ModuleRepository.Remove(module);
            // await _context.SaveChangesAsync();
            await uow.CompleteAsync();
            return NoContent();
        }

        private async Task<bool> ModuleExists(int id)
        {
            return await uow.ModuleRepository.AnyAsync(id);
            //return (_context.Module?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPatch("{modulesId}")]
        public async Task<ActionResult<ModuleDto>> PatchCourse(int moduleId, JsonPatchDocument<ModuleDto> patchDocument)
        {
            var module = await uow.ModuleRepository.AnyAsync(moduleId);
            if (module == null) return NotFound();
            var moduleDto = mapper.Map<ModuleDto>(module);
            patchDocument.ApplyTo(moduleDto, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            mapper.Map(moduleDto, module);
            await uow.CompleteAsync();
            return Ok(mapper.Map<ModuleDto>(module));
        }



    }
}
