using helpme.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace helpme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                User = usuarioDTO.User,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.Password), //Hasheo la password
                TipoDeUsuario = usuarioDTO.TipoDeUsuario,
                Email = usuarioDTO.Email,
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            if (usuario.TipoDeUsuario == "contribuyente")
            {
                var contribuyente = new Contribuyente
                {
                    Nombre = usuarioDTO.Nombre,
                    Apellido = usuarioDTO.Apellido,
                    FechaNacimiento = usuarioDTO.FechaNacimiento,
                    IdUsuario = usuario.Id,
                };

                _context.Contribuyente.Add(contribuyente);
                await _context.SaveChangesAsync();
            }
            else
            {
                var organizacion = new Organizacion
                {
                    NombreOrganizacion = usuarioDTO.NombreOrganizacion,
                    Descripcion = usuarioDTO.Descripcion,
                    CUIT = usuarioDTO.CUIT,
                    Localidad = usuarioDTO.Localidad,
                    CodigoPostal = usuarioDTO.CodigoPostal,
                    Provincia = usuarioDTO.Provincia,
                    FechaDeCreacion = usuarioDTO.FechaDeCreacion,
                    IdUsuario = usuario.Id,
                };

                _context.Organizacion.Add(organizacion);
                await _context.SaveChangesAsync();
            }

            return Ok(new { Message = "Usuario registrado correctamente" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.User == loginDto.user);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.password, usuario.PasswordHash)) //Comparo las password con la hasheada
            {
                return Unauthorized(new { Message = "Credenciales incorrectas" });
            }

            var token = GenerateJwtToken(usuario);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.User),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


    //Esto deberia ir en archivos separados, pero no me interesa las buenas practicas por ahora.
    public class LoginDTO
    {
        public string user { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }

    public class RegisterUsuarioDTO
    {
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoDeUsuario { get; set; } = string.Empty;
        public string NombreOrganizacion { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string CUIT { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string MercadoPagoCode { get; set; } = string.Empty;
        public DateTime FechaDeCreacion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
    }
}
