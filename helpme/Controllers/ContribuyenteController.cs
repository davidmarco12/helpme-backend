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
    public class ContribuyenteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContribuyenteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Contribuyente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contribuyente>>> GetContribuyente()
        {
            return await _context.Contribuyente.ToListAsync();
        }

        // GET: api/Contribuyente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contribuyente>> GetContribuyente(int id)
        {
            var contribuyente = await _context.Contribuyente.FindAsync(id);

            if (contribuyente == null)
            {
                return NotFound();
            }

            return contribuyente;
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<Contribuyente>> GetContribuyenteByUsuarioId(int idUsuario)
        {
            // Buscar la organización por IdUsuario
            var contribuyente = await _context.Contribuyente
                .FirstOrDefaultAsync(o => o.IdUsuario == idUsuario);

            if (contribuyente == null)
            {
                return NotFound("No se encontró un contribuyente para el usuario especificado.");
            }

            return Ok(contribuyente);
        }

        // PUT: api/Contribuyente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContribuyente(int id, Contribuyente contribuyente)
        {
            if (id != contribuyente.Id)
            {
                return BadRequest();
            }

            _context.Entry(contribuyente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContribuyenteExists(id))
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

        // POST: api/Contribuyente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contribuyente>> PostContribuyente(Contribuyente contribuyente)
        {
            _context.Contribuyente.Add(contribuyente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContribuyente", new { id = contribuyente.Id }, contribuyente);
        }

        // DELETE: api/Contribuyente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContribuyente(int id)
        {
            var contribuyente = await _context.Contribuyente.FindAsync(id);
            if (contribuyente == null)
            {
                return NotFound();
            }

            _context.Contribuyente.Remove(contribuyente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContribuyenteExists(int id)
        {
            return _context.Contribuyente.Any(e => e.Id == id);
        }
    }
}
