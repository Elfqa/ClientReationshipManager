using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Services;
using BusinessLogic.Models;
using DataAccess.DAL;
using DataAccess.Repositories;
using Microsoft.OpenApi.Models;
using WebApi.Auth;


var builder = WebApplication.CreateBuilder(args);



//TODO: SWAGGER---konfiguracja, cd na koncu

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();-->zmieniona konfiguracja pod JWS


//TODO dodatkowa konfiguracja SWAGGER dla JWS
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM API", Version = "v1" });

    // Konfiguracja Swaggera do u¿ywania tokenów Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Proszê wprowadziæ token JWT z prefiksem 'Bearer' w polu",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});


//TODO: konfiguracja SeriLog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));


//TODO konfiguracja JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//dodajemy dla komunikacji api z javascriptem
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.WithOrigins("http://localhost:5173")          //Specify the allowed origin(s)      //tutaj zmienic adres z tego z apki javascript!!!!!!!!!!!!
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});


builder.Services.AddControllers();



builder.Services.AddSingleton<IDapperContext, DapperContext>();

builder.Services.AddScoped<IContactsRepository, ContactsRepository>();
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<IContactsService, ContactsService>();
builder.Services.AddScoped<IClientsService, ClientsService>();

builder.Services.AddScoped<IUserAccountRepository,UserAccountRepository>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddSingleton<JwtTokenService>();


var app = builder.Build();




//todo: konfiguracja SWAGGER cd.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())    
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//TODO musza byc oba dla JWT
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();



