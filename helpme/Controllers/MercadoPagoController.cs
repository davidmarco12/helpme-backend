using Azure.Core;
using helpme.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace helpme.Controllers
{
    [Route("api/mercadopago")]
    [ApiController]
    public class MercadoPagoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _dbContext;

        public MercadoPagoController(HttpClient httpClient, ApplicationDbContext dbContext)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetMercadoPagoCode([FromQuery] MercadoPagoQueryObject query)
        {
            // Aquí el código y el estado que recibes en la URL
            var code = query.Code;
            var state = query.State; // Este es el id de la organizacion para crear el token



            if (!int.TryParse(state, out int organizacionId))
            {
                return BadRequest("El ID de la organización no es válido.");
            }

            // Configuración del cuerpo de la solicitud para el POST
            var requestData = new
            {
                client_secret = "ioFEcfVV7VkQVJa2QeKikdbCSkwSJFnv",
                client_id = "90814232312033",
                grant_type = "client_credentials",
                code, // Utilizas el código que recibes en la query string
                code_verifier = "47DEQpj8HBSa-_TImW-5JCeuQeRkm5NMpJWZG3hSuFU",
                redirect_uri = "https://dexus-web.com:7146/",
                refresh_token = code,
                test_token = false,
            };

            // Serializar los datos en JSON
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            // Hacer la solicitud POST al endpoint de Mercado Pago
            var response = await _httpClient.PostAsync("https://api.mercadopago.com/oauth/token", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

            if (responseObject != null && responseObject.TryGetValue("access_token", out var accessToken))
            {
                // Buscar la organización por el state (que es el Id de la organización)
                var organizacion = await _dbContext.Organizacion.FindAsync(organizacionId); // Usando FindAsync para buscar por ID

                if (organizacion == null)
                {
                    return NotFound("Organización no encontrada.");
                }
                // Actualizar el campo MercadoPagoCode
                organizacion.MercadoPagoCode = accessToken.ToString();

                // Guardar los cambios en la base de datos
                await _dbContext.SaveChangesAsync();

                return Ok(new { AccessToken = accessToken.ToString(), Message = "Token actualizado para la organización." });
            }

            // En caso de que no se encuentre el token
            return BadRequest("Access token not found.");

        }
    }

    public class MercadoPagoQueryObject
    {
        public string Code { get; set; }
        public string State { get; set; }
    }
}
