using AutoMapper;
using LMS.api.Data;
using LMS.api.DTO;
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
    public class ActivitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;


        public ActivitiesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivity()
        {
            return await _context.Activity.ToListAsync();
        }

        [HttpGet("/module/{moduleId}")]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivitiesByCourseId(int moduleId)
        {
            try
            {
                var activities = await _context.Activity.Where(a => a.ModuleID == moduleId).ToListAsync();
                return activities;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(int id)
        {
            try
            {
                var activity = await _context.Activity.FindAsync(id);

                if (activity == null)
                {
                    return NotFound();
                }

                return activity;
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Activities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(int id, ActivityUpdateDTO activity)
        {
            if (id != activity.Id)
            {
                return BadRequest();
            }

            try
            {
                var activityEntity = await _context.Activity.FindAsync(activity.Id);
                if (activityEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(activity, activityEntity);

                _context.Entry(activityEntity).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }

        // POST: api/Activities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(ActivityCreateDTO activity)
        {
            try
            {
                var module = await _context.Module.FindAsync(activity.ModuleID);
                if (module == null)
                {
                    return NotFound();
                }

                var finalActivity = _mapper.Map<Activity>(activity);

                _context.Activity.Add(finalActivity);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetActivity", new { id = finalActivity.Id }, activity);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            try
            {
                var activity = await _context.Activity.FindAsync(id);
                if (activity == null)
                {
                    return NotFound();
                }

                _context.Activity.Remove(activity);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }

        // GET: api/Activities/Module/5
        [HttpGet("Module/{id}")]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivitiesByModuleID(int id)
        {
            try
            {
                var @activties = await _context.Activity.Where(m => m.ModuleID == id).ToListAsync();

                if (activties == null)
                {
                    return NotFound();
                }

                return activties;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
