using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayInSafe.Core.Interfaces;
using StayInSafe.Core.Models;
using StayInSafe.Core.Services;

namespace StayInSafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitiosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        string ConnectionStringAzure = string.Empty;

        public SitiosController(IConfiguration configuration)
        {
            _configuration = configuration;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                ConnectionStringAzure = _configuration.GetConnectionString("CloudServer");
        }

        //[Authorize]
        [HttpPost]
        public ActionResult AddSitio(Sitios sitio)
        {
            if (sitio == null)
                return BadRequest("Ingrese informacion del sitio");

            long id = 0;
            using (ISitios Sitio = FactorizeService.Sitios(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                id = Sitio.AddSitio(sitio);
            }
            return id > 0 ? Ok() : BadRequest("Error al insertar");
        }
    }
}
