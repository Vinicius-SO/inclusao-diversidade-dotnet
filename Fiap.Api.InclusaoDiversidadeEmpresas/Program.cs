using AutoMapper;
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using InclusaoDiversidadeEmpresas.Data;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.Services;
using InclusaoDiversidadeEmpresas.ViewModels;
// 🔑 USINGS NECESSÁRIOS PARA JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// CORREÇÃO: Adicionando AddJsonOptions para resolver o ciclo de objetos
builder.Services.AddControllers()
  .AddJsonOptions(options =>
  {
      // Esta linha resolve o erro "A possible object cycle was detected"
      options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
  });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Services
builder.Services.AddScoped<IColaboradorService, ColaboradorService>();
builder.Services.AddScoped<IParticipacaoEmTreinamentoService, ParticipacaoEmTreinamentoService>();
builder.Services.AddScoped<IRelatorioService, RelatorioService>();
builder.Services.AddScoped<ITreinamentoService, TreinamentoService>();

builder.Services.AddScoped<IAuthService, AuthService>();
#endregion

#region Configuracao do banco de dados
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddDbContext<DatabaseContext>(opt =>
  opt.UseOracle(connectionString)
   .EnableSensitiveDataLogging(true)
);
#endregion

#region AutoMapper

var mapperConfig = new MapperConfiguration(c =>
{
    c.AllowNullCollections = true;
    c.AllowNullDestinationValues = true;

    // Mapeamentos
    c.CreateMap<TreinamentoModel, TreinamentoViewModel>();
    c.CreateMap<TreinamentoViewModel, TreinamentoModel>();
    c.CreateMap<ParticipacaoEmTreinamentoModel, ParticipacaoViewModel>();
    c.CreateMap<ParticipacaoViewModel, ParticipacaoEmTreinamentoModel>();
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

#endregion

var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);

builder.Services.AddAuthentication(x =>
{
    // Define o JWT Bearer como o esquema padrão
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Mantenha false para localhost/http
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true, // Valida se o token expirou
        ClockSkew = TimeSpan.Zero // Sem tolerância de tempo para expiração
    };
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllers();

app.Run();