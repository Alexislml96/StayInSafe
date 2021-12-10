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
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LogsTool _logs;
        private readonly IWebHostEnvironment _hostEnvironment;
        string ConnectionStringAzure = string.Empty;

        public UserController(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _logs = new LogsTool();
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                ConnectionStringAzure = _configuration.GetConnectionString("CloudServer");
        }

        [AllowAnonymous]
        [HttpPost("api/[controller]/Register")]
        public async Task<ActionResult> Register([FromBody]Users user)
        {
            if (user == null)
                return BadRequest("Ingrese informacion del usuario");
            
            long id = 0;
            //user.Imagen = await SaveImage(user.ImageFile);
            using (IUser User = FactorizeService.Usuario(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                id = User.Register(user);
            }

            Logs log = new Logs();
            var rnd = new Random();
            log.idLog = rnd.Next(1, 100000);
            log.accion = "Registro";
            log.nombreMetodo = "Register";
            log.usuario = user.Email;
            await _logs.InsertLog(log);

            return id > 0 ? Ok() : BadRequest("Error al insertar");
        }

        [Authorize]
        [HttpPost("api/[controller]/Update")]
        public async Task <ActionResult> UpdateUserAsync([FromBody]Users user)
        {
            if (user.Id == 0)
                return BadRequest("Ingrese un ID válido");

            bool model = false;
            //user.Imagen = await SaveImage(user.ImageFile);
            using (IUser User = FactorizeService.Usuario(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                model = User.UpdateUser(user);
            }

            Logs log = new Logs();
            var rnd = new Random();
            log.idLog = rnd.Next(1, 100000);
            log.accion = "Update";
            log.nombreMetodo = "Update";
            log.usuario = user.Email;
            await _logs.InsertLog(log);

            return model == true ? Ok() : BadRequest("Error al actualizar");
        }

        [Authorize]
        [HttpGet("api/[controller]/GetUser/{id}")]
        public async Task<ActionResult<Users>> GetUser(long id)
        {
            if(id ==0)
                return BadRequest("Ingrese ID de usuario válido");
            Users userModel = new Users();
            using (IUser user = FactorizeService.Usuario(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                userModel = user.GetUser(id);
            }

            if (userModel == null)
                return BadRequest("Usuario no encontrado");
            return userModel;
        }


        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            var imageName = new string (Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = $"{imageName}{DateTime.Now.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }


    }
}
