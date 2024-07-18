using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using helpme.Models;
using Microsoft.AspNetCore.Authorization;

namespace helpme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DonacionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DonacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Donacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donacion>>> GetDonacion()
        {
            return await _context.Donacion.ToListAsync();
        }

        // GET: api/Donacion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Donacion>> GetDonacion(int id)
        {
            var donacion = await _context.Donacion.FindAsync(id);

            if (donacion == null)
            {
                return NotFound();
            }

            return donacion;
        }

        // PUT: api/Donacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonacion(int id, Donacion donacion)
        {
            if (id != donacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(donacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonacionExists(id))
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

        // POST: api/Donacion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Donacion>> PostDonacion(Donacion donacion)
        {
            _context.Donacion.Add(donacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDonacion", new { id = donacion.Id }, donacion);
        }

        // DELETE: api/Donacion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonacion(int id)
        {
            var donacion = await _context.Donacion.FindAsync(id);
            if (donacion == null)
            {
                return NotFound();
            }

            _context.Donacion.Remove(donacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DonacionExists(int id)
        {
            return _context.Donacion.Any(e => e.Id == id);
        }
    }
}
