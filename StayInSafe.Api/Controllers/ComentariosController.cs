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
    [Authorize]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LogsTool _logs;
        string ConnectionStringAzure = string.Empty;

        public ComentariosController(IConfiguration configuration)
        {
            _configuration = configuration;
            _logs = new LogsTool();
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                ConnectionStringAzure = _configuration.GetConnectionString("CloudServer");
        }

        
        [HttpPost("api/[controller]/AddComment")]
        public async Task<ActionResult> AddComment(Comentarios comentario)
        {
            if (comentario == null)
                return BadRequest("Ingrese informacion del sitio");

            long id = 0;
            using (IComentario Comentarios = FactorizeService.Comentarios(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                id = Comentarios.AddComment(comentario);
            }

            Logs log = new Logs();
            var rnd = new Random();
            log.idLog = rnd.Next(1, 100000);
            log.accion = "Add Comment";
            log.nombreMetodo = "AddComment";
            log.usuario = comentario.Id.ToString();
            await _logs.InsertLog(log);

            return id > 0 ? Ok() : BadRequest("Error al insertar");
        }

        [HttpGet("api/[controller]/GetComments/{id}")]
        public async Task<IEnumerable<Comentarios>> GetComents(int id)
        {
            IEnumerable<Comentarios> model = new List<Comentarios>();
            using (IComentario comentario = FactorizeService.Comentarios(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                model = comentario.GetCommentSito(id);
            }

            Logs log = new Logs();
            var rnd = new Random();
            log.idLog = rnd.Next(1, 100000);
            log.accion = "Get Comments By ID";
            log.nombreMetodo = "GetComments";
            log.usuario = "N/A";
            await _logs.InsertLog(log);

            return model;
        }


    }
}
