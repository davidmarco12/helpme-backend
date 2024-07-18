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
    public class PublicacionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PublicacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Publicacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publicacion>>> GetPublicacion()
        {
            return await _context.Publicacion.ToListAsync();
        }

        // GET: api/Publicacion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publicacion>> GetPublicacion(int id)
        {
            var publicacion = await _context.Publicacion.FindAsync(id);

            if (publicacion == null)
            {
                return NotFound();
            }

            return publicacion;
        }

        // PUT: api/Publicacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublicacion(int id, Publicacion publicacion)
        {
            if (id != publicacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(publicacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicacionExists(id))
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

        // POST: api/Publicacion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Publicacion>> PostPublicacion(Publicacion publicacion)
        {
            _context.Publicacion.Add(publicacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPublicacion", new { id = publicacion.Id }, publicacion);
        }

        // DELETE: api/Publicacion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublicacion(int id)
        {
            var publicacion = await _context.Publicacion.FindAsync(id);
            if (publicacion == null)
            {
                return NotFound();
            }

            _context.Publicacion.Remove(publicacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PublicacionExists(int id)
        {
            return _context.Publicacion.Any(e => e.Id == id);
        }
    }
}
