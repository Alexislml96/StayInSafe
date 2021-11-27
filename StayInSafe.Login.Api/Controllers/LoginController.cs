using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayInSafe.Core.Interfaces;
using StayInSafe.Core.Models;
using StayInSafe.Core.Services;
using StayInSafe.Core.Tools;
using StayInSafe.Login.Api.Helpers;

namespace StayInSafe.Login.Api.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HashTool _hashTool;
        private readonly RefreshTokenGenerator _tokenGenerator;
        string ConnectionStringAzure = string.Empty;
        string _secretKey;
        string _audienceToken;
        string _issuerToken;
        string _expireTime;
        string _refreshKey;
        string _refreshExpire;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            _hashTool = new HashTool();
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                ConnectionStringAzure = _configuration.GetConnectionString("CloudServer");
                _secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
                _audienceToken = Environment.GetEnvironmentVariable("AUDIENCE_TOKEN");
                _issuerToken = Environment.GetEnvironmentVariable("ISSUER_TOKEN");
                _expireTime = Environment.GetEnvironmentVariable("EXPIRE_MINUTES");
                _refreshKey = Environment.GetEnvironmentVariable("REFRESH_SECRET");
                _refreshExpire = Environment.GetEnvironmentVariable("REFRESH_EXPIRE");
            }
            else
            {
                _secretKey = _configuration["JWT:SECRET_KEY"];
                _audienceToken = _configuration["JWT:AUDIENCE_TOKEN"];
                _issuerToken = _configuration["JWT:ISSUER_TOKEN"];
                _expireTime = _configuration["JWT:EXPIRE_MINUTES"];
                _refreshKey = _configuration["JWT:REFRESH_SECRET"];
                _refreshExpire = _configuration["JWT:REFRESH_EXPIRE"]; ;
            }
            _tokenGenerator = new RefreshTokenGenerator(_refreshKey, _audienceToken, _issuerToken, _refreshExpire);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult<RequestModel> Login([FromBody] LoginModel model)
        {
            long idRefresh = 0;
            if (string.IsNullOrEmpty(model.Email))
                throw new NullReferenceException("Email vacío, el campo es necesario");
            if (string.IsNullOrEmpty(model.Pass))
                throw new NullReferenceException("Password vacío, el campo es necesario");
            var validation = UsuarioValido(model);

            if (validation.Item1)
            {

                Users u = new Users();
                RequestModel request = new RequestModel();
                using (ILogin Login = FactorizeService.Login(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
                {
                    u = Login.Login(validation.Item2);
                }
                request.JWT = TokenGenerator.GenerateTokenJwt(u.Email, u.Id,_secretKey,_audienceToken, _issuerToken, _expireTime);
                var refresh = _tokenGenerator.GenerateRefreshToken();
                request.RefreshToken = refresh.Item1;
                request.RefreshTokenExpiryTime = refresh.Item2;
                RefreshToken refreshToken = new RefreshToken
                {
                    token = request.RefreshToken,
                    user_id = u.Id,
                };
                using (IRefresh service = FactorizeService.Refresh(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
                {
                    idRefresh = service.Create(refreshToken);
                }
                if (!(idRefresh > 0))
                    return BadRequest();
                
                return Ok(request);
            }

            return NotFound();
        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        public ActionResult<RequestModel> Refresh([FromBody] RefreshModel model)
        {
            long idRefresh = 0;
            RefreshToken tokenModel = new RefreshToken();
            Users userModel = new Users();
            RequestModel tokens = new RequestModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isValid = _tokenGenerator.Validate(model.refreshToken);

            if (!isValid)
                return BadRequest();

            using (IRefresh service = FactorizeService.Refresh(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                tokenModel = service.GetByToken(model.refreshToken);
            }
            if (tokenModel == null)
                return NotFound();

            using (IRefresh service = FactorizeService.Refresh(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                service.Delete(tokenModel.token_id);
            }

            using (IUser service = FactorizeService.Usuario(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                userModel = service.GetUser(tokenModel.user_id);
            }
            if (userModel == null)
                return NotFound();

            tokens.JWT = TokenGenerator.GenerateTokenJwt(userModel.Email, userModel.Id, _secretKey, _audienceToken, _issuerToken, _expireTime);
            var refresh =  _tokenGenerator.GenerateRefreshToken();
            tokens.RefreshToken = refresh.Item1;
            tokens.RefreshTokenExpiryTime = refresh.Item2;
            RefreshToken refreshToken = new RefreshToken
            {
                token = tokens.JWT,
                user_id = tokenModel.user_id,
            };
            using (IRefresh service = FactorizeService.Refresh(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                idRefresh = service.Create(refreshToken);
            }
            if (!(idRefresh > 0))
                return BadRequest();

            return Ok(tokens);

        }

        private Tuple<bool, LoginModel> UsuarioValido(LoginModel model)
        {
            LoginModel user = new LoginModel();
            using (IPassword service = FactorizeService.Password(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            {
                user = service.CheckUser(model);
            }

            var isValid = _hashTool.Check(user.Pass, model.Pass);
            var res = Tuple.Create(isValid, user);
            return res;
        }
    }
}
