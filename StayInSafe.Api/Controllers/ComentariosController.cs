using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayInSafe.Core.Interfaces;
using StayInSafe.Core.Models;
using StayInSafe.Core.Services;

namespace StayInSafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        string ConnectionStringAzure = string.Empty;

        public ComentariosController(IConfiguration configuration)
        {
            _configuration = configuration;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                ConnectionStringAzure = _configuration.GetConnectionString("CloudServer");
        }

        [HttpPost("api/[controller]/AddComment")]
        public ActionResult AddContact(Comentarios comentario)
        {
            if (comentario == null)
                return BadRequest("Ingrese informacion del sitio");

            long id = 0;
            using (IComentario Comentarios = FactorizeService.Comentarios(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                id = Comentarios.AddComment(comentario);
            }
            return id > 0 ? Ok() : BadRequest("Error al insertar");
        }

        [HttpGet("api/[controller]/GetComments/{id}")]
        public IEnumerable<Comentarios> GetComents(int id)
        {
            IEnumerable<Comentarios> model = new List<Comentarios>();
            using (IComentario comentario = FactorizeService.Comentarios(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                model = comentario.GetCommentSito(id);
            }

            return model;
        }


    }
}
