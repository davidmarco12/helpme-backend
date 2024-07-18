using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using helpme.Models;

namespace helpme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            return await _context.Usuario.ToListAsync();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Usuario>> PostUsuario([FromBody] UsuarioDTO usuarioDTO)
        //{

        //    var usuario = new Usuario
        //    {
        //        User = usuarioDTO.User,
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.Password),
        //        TipoDeUsuario = usuarioDTO.TipoDeUsuario,
        //        Email = usuarioDTO.Email,
        //    };

        //    _context.Usuario.Add(usuario);
        //    await _context.SaveChangesAsync();

        //    if (usuario.TipoDeUsuario == "Contribuyente")
        //    {
        //        var contribuyente = new Contribuyente
        //        { 
        //             Nombre = usuarioDTO.Nombre,
        //             Apellido = usuarioDTO.Apellido,
        //             FechaNacimiento = usuarioDTO.FechaNacimiento,
        //             IdUsuario = usuario.Id,
        //        };

        //        _context.Contribuyente.Add(contribuyente);
        //        await _context.SaveChangesAsync();

        //    }else
        //    {
        //        var organizacion = new Organizacion
        //        {
        //            NombreOrganizacion = usuarioDTO.NombreOrganizacion,
        //            Descripcion = usuarioDTO.Descripcion,
        //            CUIT = usuarioDTO.CUIT,
        //            Localidad = usuarioDTO.Localidad,
        //            CodigoPostal = usuarioDTO.CodigoPostal,
        //            Provincia = usuarioDTO.Provincia,
        //            FechaDeCreacion = usuarioDTO.FechaDeCreacion,
        //            IdUsuario = usuario.Id,
        //        };

        //        _context.Organizacion.Add(organizacion);
        //        await _context.SaveChangesAsync();
        //    }

        //    return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        //}

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }
    }


    

}
