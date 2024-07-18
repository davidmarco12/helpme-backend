using helpme.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace helpme.Controllers
{
    [Route("api/mercadopago")]
    [ApiController]
    public class MercadoPagoController : Controller
    {
        private readonly ILogger _logger;

        public MercadoPagoController(ILogger<MercadoPagoController> logger)
        {
              _logger = logger;
        }

        // GET: api/Organizacion
        [HttpGet]
        public async Task<IActionResult> GetMercadoPagoCode([FromQuery] MercadoPagoQueryObject body)
        {
            _logger.LogInformation(body.code);
            return NoContent();
        }
    }

    public class MercadoPagoQueryObject
    {
        public string? code { get; set; } = null;

    }
}
