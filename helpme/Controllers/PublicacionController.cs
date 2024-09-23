using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using helpme.Models;
using Microsoft.AspNetCore.Authorization;
using Azure.Storage.Blobs;
using helpme.Helpers;
using System.ComponentModel.DataAnnotations;
using MercadoPago.Client.Common;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;

namespace helpme.Controllers
{
    [Route("api/publicacion")]
    [ApiController]
    //[Authorize]
    public class PublicacionController : ControllerBase
    {
       
        private readonly ApplicationDbContext _context;
        private readonly FileService _fileService;

        public PublicacionController(ApplicationDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // GET: api/Publicacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publicacion>>> GetPublicacion()
        {
            return await _context.Publicacion.OrderByDescending(p => p.Id).ToListAsync();
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
        public async Task<ActionResult<Publicacion>> PostPublicacion(PublicacionRequestDTO publicacion)
        {
            if (publicacion.OrganizacionId.HasValue)
            {
                var organizacion = await _context.Organizacion
                    .FirstOrDefaultAsync(o => o.Id == publicacion.OrganizacionId.Value);
                if (organizacion == null)
                {
                    return BadRequest("La organización especificada no existe.");
                }

                //MercadoPagoConfig.AccessToken = organizacion.MercadoPagoCode;
                //PreferenceRequest request = new PreferenceRequest
                //{
                //    BackUrls = new PreferenceBackUrlsRequest
                //    {
                //        Success = "http://test.com/success",
                //        Failure = "http://test.com/failure",
                //        Pending = "http://test.com/pending"
                //    },
                //    DifferentialPricing = new PreferenceDifferentialPricingRequest
                //        {
                //            Id = 1
                //        },
                //    Expires = false,
                //    Items = new List<PreferenceItemRequest>
                //    {
                //    new PreferenceItemRequest
                //        {
                //            Id = "1234",
                //            Title = publicacion.Titulo,
                //            Description = "Donacion para ONG",
                //            PictureUrl = "http://www.myapp.com/myimage.jpg",
                //            CategoryId = "car_electronics",
                //            Quantity = 1,
                //            CurrencyId = "ARS",
                //            UnitPrice = 10000
                //        }
                //    },
                //};
                //PreferenceClient client = new PreferenceClient();
                //Preference preference = client.Create(request);

                var publicacionPost = new Publicacion
                {
                    Titulo = publicacion.Titulo,
                    Contenido = publicacion.Contenido,
                    OrganizacionId = publicacion.OrganizacionId,
                    DescripcionDonacion = publicacion.DetallePago,
                };

                _context.Publicacion.Add(publicacionPost);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPublicacion", new { id = publicacionPost.Id }, publicacionPost);

            }
            else
            {
                return BadRequest("El Id de la organización no está especificado.");
            }

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

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await _fileService.UploadAsync(file);
            return Ok(result);
        }
    }


    public class PublicacionRequestDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public string DetallePago { get; set; } = string.Empty;
        public int? OrganizacionId { get; set; }
    }

    



}
