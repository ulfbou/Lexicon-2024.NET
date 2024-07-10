using AutoMapper;
using LMS.api.Data;
using LMS.api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        public ModulesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModule()
        {
            try
            {
                return await _context.Module.ToListAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDTO>> GetModule(int id)
        {
            try
            {
                var @module = await _context.Module.FindAsync(id);

                if (@module == null)
                {
                    return NotFound();
                }

                return _mapper.Map<ModuleDTO>(@module);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Modules/Course/5
        [HttpGet("Course/{id}")]
        public async Task<ActionResult<IEnumerable<Module>>> GetModuleByCourseID(int id)
        {
            try
            {
                var @modules = await _context.Module.Where(m => m.CourseID == id).ToListAsync();

                if (modules == null)
                {
                    return NotFound();
                }

                return modules;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, ModuleDTO @module)
        {
            if (id != @module.Id)
            {
                return BadRequest();
            }

            try
            {
                var moduleEntity = await _context.Module.FindAsync(id);
                if (moduleEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(@module, moduleEntity);

                _context.Entry(moduleEntity).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(id))
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(ModuleDTO @module)
        {
            try
            {
                var @course = await _context.Course.FindAsync(@module.CourseId);
                if (@course == null)
                {
                    return NotFound();
                }

                var finalModule = _mapper.Map<Module>(@module);

                _context.Module.Add(finalModule);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetModule", new { id = @module.Id }, @module);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            try
            {
                var @module = await _context.Module.FindAsync(id);
                if (@module == null)
                {
                    return NotFound();
                }

                _context.Module.Remove(@module);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.Id == id);
        }
    }
}
