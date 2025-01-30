using Microsoft.OpenApi.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Resources;
using Inventory.Repository.DataContext;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using American.Shared.Core.SettingOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebToken.JWT;
using ExceptionMiddleware;
using Inventory.Infrastructure.Interface;
using Inventory.Infrastructure.Repository.UnitOfWork;
using Inventory.Services.Interface;
using Inventory.Services.Services;
using InventoryAPI.Mapper;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<InternalSettings>(builder.Configuration.GetSection("BackendSetting"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(
       options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#region Swagger
builder.Services.AddSwaggerGen(c =>
{

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

});

#endregion


IConfiguration Configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddEnvironmentVariables()
        .AddJsonFile("appsettings.json")
    .AddCommandLine(args)
    .Build();


var jwtOptionsSection = Configuration.GetRequiredSection("AuthSettings");
builder.Services.Configure<JwtOptions>(Configuration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOptions =>
{
    var configKey = jwtOptionsSection["Secret"];
    var key = Encoding.UTF8.GetBytes(configKey);

    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtOptionsSection["Issuer"],
        ValidAudience = jwtOptionsSection["Audiance"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

var services = builder.Services;

#region CORS
IConfigurationSection originsSection = Configuration.GetSection("AllowedOrigins");
string?[] origns = originsSection.AsEnumerable().Where(s => s.Value != null).Select(a => a.Value).ToArray();

services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins",
        builder =>
        {
            builder.WithOrigins(origns)
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });

    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()       
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

#endregion


var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());

});


services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IInventoryServices, InventoryServices>();
services.AddScoped<IItemServices, ItemServices>();
services.AddScoped<ICategoryServices, CategoryServices>();
services.AddScoped<ISupplierServices, SupplierServices>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionMiddleware();


app.MapControllers();

app.UseCors("AllowedOrigins");

app.Run();
