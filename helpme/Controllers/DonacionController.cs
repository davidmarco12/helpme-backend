using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using helpme.Models;
using Microsoft.AspNetCore.Authorization;
using helpme.Migrations;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;

namespace helpme.Controllers
{
    [Route("api/donacion")]
    [ApiController]
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

        [HttpPost("crear-donacion")]
        public async Task<ActionResult> CreateDonacion([FromBody] RequestDonacionDTO donacion)
        {

            var organizacion = await _context.Organizacion
                    .FirstOrDefaultAsync(o => o.Id == donacion.OrganizacionID);
            if (organizacion == null)
            {
                return BadRequest("La organización especificada no existe.");
            }

            var donacionNueva = await _context.Donacion.AddAsync(new Donacion
            {
                Cantidad = donacion.Cantidad,
                ContribuyenteId = donacion.ContribuyenteID,
                MetodoPago = "MercadoPago",
                PublicacionId = donacion.PublicacionID,
                Mensaje = donacion.Mensaje,
                Estado = false,
            });

            await _context.SaveChangesAsync();

            var donacionId = donacionNueva.Entity.Id;

            MercadoPagoConfig.AccessToken = organizacion.MercadoPagoCode;
            PreferenceRequest request = new PreferenceRequest
            {
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://dexus-web.com:3000/publication/" + donacionId,
                    Failure = "http://test.com/failure",
                    Pending = "http://test.com/pending"
                },
                DifferentialPricing = new PreferenceDifferentialPricingRequest
                {
                    Id = 1
                },
                Expires = false,
                Items = new List<PreferenceItemRequest>
                    {
                    new PreferenceItemRequest
                        {
                            Id = "1234",
                            Title = donacion.Titulo,
                            Description = "Donacion para ONG",
                            PictureUrl = "http://www.myapp.com/myimage.jpg",
                            CategoryId = "car_electronics",
                            Quantity = 1,
                            CurrencyId = "ARS",
                            UnitPrice = donacion.Cantidad
                        }
                    },
            };
            PreferenceClient client = new PreferenceClient();
            Preference preference = client.Create(request);

            var publicacion = await _context.Publicacion.FirstOrDefaultAsync(p => p.Id == donacion.PublicacionID.Value);

            publicacion.ReferenciaDePago = preference.InitPoint;

            await _context.SaveChangesAsync();

            return Ok(preference.InitPoint);
        }

        // PUT: api/Donacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("actualizar-donacion")]
        public async Task<IActionResult> PutDonacion([FromQuery] int id)
        {
            var donacion = _context.Donacion.Find(id);

            if (donacion == null) return Problem("No existe la donacion");

            donacion.Estado = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Problem("Problema con la donacion");
            }

            return Ok("Donacion Creada");
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


        public class RequestDonacionDTO 
        { 
            public string? Mensaje  { get; set; }
            public decimal Cantidad { get; set; }
            public string? Titulo { get; set; }
            public int? OrganizacionID { get; set; }
            public int? PublicacionID { get; set; }
            public int? ContribuyenteID { get; set; }
        }


    }
}
