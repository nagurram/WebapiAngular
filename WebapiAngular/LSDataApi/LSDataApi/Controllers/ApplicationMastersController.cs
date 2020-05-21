using LSDataApi.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSDataApi.Controllers
{
    /// <summary>
    /// ApplicationMasters ananymous controller
    /// </summary>
    [Route("api/[controller]")]
    public class ApplicationMastersController : ControllerBase
    {
        private readonly TicketTrackerContext _context;

        /// <summary>
        /// ApplicationMastersController Constructor Dependency injection
        /// </summary>
        public ApplicationMastersController(TicketTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list of applications
        /// </summary>
        /// <returns></returns>
        // GET: api/ApplicationMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationMaster>>> GetApplicationMaster()
        {
            return await _context.ApplicationMaster.ToListAsync();
        }

        /// <summary>
        /// get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/ApplicationMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationMaster>> GetApplicationMaster(int id)
        {
            var applicationMaster = await _context.ApplicationMaster.FindAsync(id);

            if (applicationMaster == null)
            {
                return NotFound();
            }

            return applicationMaster;
        }

        /// <summary>
        /// update application details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="applicationMaster"></param>
        /// <returns></returns>
        // PUT: api/ApplicationMasters/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationMaster(int id, ApplicationMaster applicationMaster)
        {
            if (id != applicationMaster.ApplicationId)
            {
                return BadRequest();
            }

            _context.Entry(applicationMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationMasterExists(id))
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

        /// <summary>
        /// insert application
        /// </summary>
        /// <param name="applicationMaster"></param>
        /// <returns></returns>
        // POST: api/ApplicationMasters
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ApplicationMaster>> PostApplicationMaster(ApplicationMaster applicationMaster)
        {
            _context.ApplicationMaster.Add(applicationMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplicationMaster", new { id = applicationMaster.ApplicationId }, applicationMaster);
        }

        /// <summary>
        /// Delete Applications
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApplicationMasters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationMaster>> DeleteApplicationMaster(int id)
        {
            var applicationMaster = await _context.ApplicationMaster.FindAsync(id);
            if (applicationMaster == null)
            {
                return NotFound();
            }

            _context.ApplicationMaster.Remove(applicationMaster);
            await _context.SaveChangesAsync();

            return applicationMaster;
        }

        private bool ApplicationMasterExists(int id)
        {
            return _context.ApplicationMaster.Any(e => e.ApplicationId == id);
        }
    }
}