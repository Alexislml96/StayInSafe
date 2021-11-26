using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StayInSafe.Core.Configuration;
using StayInSafe.Core.Interfaces;
using StayInSafe.Core.Models;
using StayInSafe.Core.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = Environment.GetEnvironmentVariable("ISSUER_TOKEN"),
                         ValidAudience = Environment.GetEnvironmentVariable("AUDIENCE_TOKEN"),
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY"))),
                         ClockSkew = TimeSpan.Zero,
                     };
                 });
}

else
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer(options =>
                  {
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuer = true,
                          ValidateAudience = true,
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,
                          ValidIssuer = builder.Configuration["JWT:ISSUER_TOKEN"],
                          ValidAudience = builder.Configuration["JWT:AUDIENCE_TOKEN"],
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SECRET_KEY"])),
                          ClockSkew = TimeSpan.Zero,
                      };
                  });
}

builder.Services.AddTransient((ServiceProvider) => BridgeDbConnection<Users>.Create(builder.Configuration.GetConnectionString("LocalServer"), Alexis.CORE.Connection.Models.DbEnum.Sql));
builder.Services.AddTransient((ServiceProvider) => BridgeDbConnection<LoginModel>.Create(builder.Configuration.GetConnectionString("LocalServer"), Alexis.CORE.Connection.Models.DbEnum.Sql));
builder.Services.AddTransient((ServiceProvider) => BridgeDbConnection<RefreshToken>.Create(builder.Configuration.GetConnectionString("LocalServer"), Alexis.CORE.Connection.Models.DbEnum.Sql));


builder.Services.AddScoped<ILogin, LoginService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
