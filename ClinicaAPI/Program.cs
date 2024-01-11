using ClinicaAPI.DataContext;
using ClinicaAPI.Service.ClienteService;
using ClinicaAPI.Service.DonoSalaService;
using ClinicaAPI.Service.EmailService;
using ClinicaAPI.Service.FormacaoService;
using ClinicaAPI.Service.PerfilService;
using ClinicaAPI.Service.ProntuarioService;
using ClinicaAPI.Service.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ClinicaAPI.Service.ColaboradorService;
using ClinicaAPI.Service.InfoService;
using ClinicaAPI.Service.ValorService;
using ClinicaAPI.Service.FinanceiroService;

var builder = WebApplication.CreateBuilder(args);



var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Casagrande - Sistema de controle",
        Version = "v1",
        Description = "API de comunicacao entre sistemas da Clinica Casagrande",
    });
    /*c.IncludeXmlComments(Path.Combine(
        AppContext.BaseDirectory, 
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml")); */
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Token de Autorizacao Dev+ API \r\n\r\n Entre com 'Bearer' [space] e seu token de autorizacao.\r\n\r\n Exemplo: \"Bearer token\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        }, new string[] { }
        }
    });
});

builder.Services.AddScoped<IClienteInterface, ClienteService>();
builder.Services.AddScoped<IPerfilInterface, PerfilService>();
builder.Services.AddScoped<IFormacaoInterface, FormacaoService>();
builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<IProntuarioInterface, ProntuarioService>();
builder.Services.AddScoped<IAgendaInterface, AgendaService>();
builder.Services.AddScoped<IDonoSalaInterface, DonoSalaService>();
builder.Services.AddScoped<IEmailInterface, EmailService>();
builder.Services.AddScoped<IColaboradorInterface, ColaboradorService>();
builder.Services.AddScoped<InfoInterface, InfoService>();
builder.Services.AddScoped<IValorInterface, ValorService>();
builder.Services.AddScoped<IFinanceiroInterface, FinanceiroService>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        builder => builder

                        //.WithOrigins("http://34.123.211.220")
                        //.WithOrigins("http://34.170.174.186")  
                        //.WithOrigins("http://34.66.160.170")

                        //.WithOrigins("http://localhost:4200")
                        //.AllowAnyOrigin()                             
                        .AllowAnyMethod()
                        .AllowAnyHeader()); 
                
});





        //builder => builder
        //builder => builder.WithOrigins("http://34.170.174.186")

//======================================

/*
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "966741631742-sjsrqt2f8251f7qc0b48i2qcnrsi7tk2.apps.googleusercontent.com";
    options.ClientSecret = "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";
});
*/


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

//====================================

builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowMyOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

