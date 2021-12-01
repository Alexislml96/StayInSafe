using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayInSafe.Core.Interfaces;
using StayInSafe.Core.Models;
using StayInSafe.Core.Services;

namespace StayInSafe.Api.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ContactosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        string ConnectionStringAzure = string.Empty;

        public ContactosController(IConfiguration configuration)
        {
            _configuration = configuration;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                ConnectionStringAzure = _configuration.GetConnectionString("CloudServer");
        }

        //[Authorize]
        [HttpPost("api/[controller]/AddContact")]
        public ActionResult AddContact(Contactos contacto)
        {
            if (contacto == null)
                return BadRequest("Ingrese informacion del sitio");

            long id = 0;
            using (IContactos Contactos = FactorizeService.Contactos(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                id = Contactos.AddContact(contacto);
            }
            return id > 0 ? Ok() : BadRequest("Error al insertar");
        }

        //[Authorize]
        [HttpDelete("api/[controller]/DeleteContact/{id}")]
        public ActionResult DeleteContact(int id)
        {
            if (id == 0)
                return BadRequest("Ingrese un ID válido");

            using (IContactos Contacto = FactorizeService.Contactos(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                Contacto.DeleteContact(id);
            }

            return Ok();
        }

        //[Authorize]
        [HttpGet("api/[controller]/GetContacts/{id}")]
        public IEnumerable<Contactos> GetContacts(int id) 
        {
            IEnumerable<Contactos> model = new List<Contactos>();
            using (IContactos Contacto = FactorizeService.Contactos(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                model = Contacto.GetContactsById(id);
            }

            return model;
        }
    }
}
