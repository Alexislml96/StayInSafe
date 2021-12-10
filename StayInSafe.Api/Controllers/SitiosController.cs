using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayInSafe.Core.Interfaces;
using StayInSafe.Core.Models;
using StayInSafe.Core.Services;
using StayInSafe.Core.Tools;

namespace StayInSafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitiosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LogsTool _logs;
        string ConnectionStringAzure = string.Empty;

        public SitiosController(IConfiguration configuration)
        {
            _configuration = configuration;
            _logs = new LogsTool();
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                ConnectionStringAzure = _configuration.GetConnectionString("CloudServer");
        }

        [Authorize]
        [HttpPost]
        public async Task <ActionResult> AddSitio(Sitios sitio)
        {
            if (sitio == null)
                return BadRequest("Ingrese informacion del sitio");

            long id = 0;
            using (ISitios Sitio = FactorizeService.Sitios(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                id = Sitio.AddSitio(sitio);
            }

            Logs log = new Logs();
            var rnd = new Random();
            log.idLog = rnd.Next(1, 100000);
            log.accion = "Add Sitio";
            log.nombreMetodo = "AddSitio";
            log.usuario = "N/A";
            await _logs.InsertLog(log);
            
            return id > 0 ? Ok() : BadRequest("Error al insertar");
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Sitios>> GetSitios()
        {
            List<Sitios> model = new List<Sitios>();
            using (ISitios Sitios = FactorizeService.Sitios(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                model = Sitios.GetSitios();
            }

            Logs log = new Logs();
            var rnd = new Random();
            log.idLog = rnd.Next(1, 100000);
            log.accion = "Get Sitios";
            log.nombreMetodo = "GetSitios";
            log.usuario = "N/A";
            await _logs.InsertLog(log);

            return model;
        }
    }
}
