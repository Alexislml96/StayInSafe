using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayInSafe.Core.Interfaces;
using StayInSafe.Core.Models;
using StayInSafe.Core.Services;
using StayInSafe.Core.Tools;

namespace StayInSafe.Api.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ContactosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LogsTool _logs;
        string ConnectionStringAzure = string.Empty;

        public ContactosController(IConfiguration configuration)
        {
            _configuration = configuration;
            _logs = new LogsTool();
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                ConnectionStringAzure = _configuration.GetConnectionString("CloudServer");
        }

        [Authorize]
        [HttpPost("api/[controller]/AddContact")]
        public async Task <ActionResult> AddContact(Contactos contacto)
        {
            if (contacto == null)
                return BadRequest("Ingrese informacion del sitio");

            long id = 0;
            using (IContactos Contactos = FactorizeService.Contactos(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                id = Contactos.AddContact(contacto);
            }
            Logs log = new Logs();
            var rnd = new Random();
            log.idLog = rnd.Next(1, 100000);
            log.accion = "Add Contact";
            log.nombreMetodo = "AddContact";
            log.usuario = contacto.Id.ToString();
            await _logs.InsertLog(log);

            return id > 0 ? Ok() : BadRequest("Error al insertar");
        }

        [Authorize]
        [HttpDelete("api/[controller]/DeleteContact/{id}")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            if (id == 0)
                return BadRequest("Ingrese un ID válido");

            using (IContactos Contacto = FactorizeService.Contactos(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                Contacto.DeleteContact(id);
            }

            Logs log = new Logs();
            var rnd = new Random();
            log.idLog = rnd.Next(1, 100000);
            log.accion = "Delete Contact";
            log.nombreMetodo = "DeleteContact";
            log.usuario = "N/A";
            await _logs.InsertLog(log);

            return Ok();
        }

        [Authorize]
        [HttpGet("api/[controller]/GetContacts/{id}")]
        public async Task<IEnumerable<Contactos>> GetContacts(int id) 
        {
            IEnumerable<Contactos> model = new List<Contactos>();
            using (IContactos Contacto = FactorizeService.Contactos(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                model = Contacto.GetContactsById(id);
            }

            Logs log = new Logs();
            var rnd = new Random();
            log.idLog = rnd.Next(1, 100000);
            log.accion = "Get Contacts By ID";
            log.nombreMetodo = "GetContacts";
            log.usuario = id.ToString();
            await _logs.InsertLog(log);

            return model;
        }
    }
}
