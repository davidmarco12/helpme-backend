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
    public class OrganizacionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrganizacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Organizacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organizacion>>> GetOrganizacion()
        {
            return await _context.Organizacion.ToListAsync();
        }

        // GET: api/Organizacion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Organizacion>> GetOrganizacion(int id)
        {
            var organizacion = await _context.Organizacion.FindAsync(id);

            if (organizacion == null)
            {
                return NotFound();
            }

            return organizacion;
        }

        // PUT: api/Organizacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganizacion(int id, Organizacion organizacion)
        {
            if (id != organizacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(organizacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizacionExists(id))
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

        // POST: api/Organizacion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Organizacion>> PostOrganizacion(Organizacion organizacion)
        {
            _context.Organizacion.Add(organizacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrganizacion", new { id = organizacion.Id }, organizacion);
        }

        // DELETE: api/Organizacion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganizacion(int id)
        {
            var organizacion = await _context.Organizacion.FindAsync(id);
            if (organizacion == null)
            {
                return NotFound();
            }

            _context.Organizacion.Remove(organizacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrganizacionExists(int id)
        {
            return _context.Organizacion.Any(e => e.Id == id);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<Organizacion>> GetOrganizacionByUsuarioId(int idUsuario)
        {
            // Buscar la organización por IdUsuario
            var organizacion = await _context.Organizacion
                .FirstOrDefaultAsync(o => o.IdUsuario == idUsuario);

            if (organizacion == null)
            {
                return NotFound("No se encontró una organización para el usuario especificado.");
            }

            return Ok(organizacion);
        }
    }
}
