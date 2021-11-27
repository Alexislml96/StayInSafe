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
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        string ConnectionStringAzure = string.Empty;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                ConnectionStringAzure = _configuration.GetConnectionString("CloudServer");
        }

        [AllowAnonymous]
        [HttpPost("api/[controller]/Register")]
        public ActionResult Register(Users user)
        {
            if (user == null)
                return BadRequest("Ingrese informacion del usuario");
            
            long id = 0;
            using (IUser User = FactorizeService.Usuario(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                id = User.Register(user);
            }
            return id > 0 ? Ok() : BadRequest("Error al insertar");
        }

        [Authorize]
        [HttpPost("api/[controller]/Update")]
        public ActionResult UpdateUser(Users user)
        {
            if (user.Id == 0)
                return BadRequest("Ingrese un ID válido");

            bool model = false;
            using (IUser User = FactorizeService.Usuario(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                model = User.UpdateUser(user);
            }

            return model == true ? Ok() : BadRequest("Error al actualizar");
        }


    }
}
