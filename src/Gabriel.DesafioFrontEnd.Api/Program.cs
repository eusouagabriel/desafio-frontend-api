using Gabriel.DesafioFrontEnd.Api.Controllers.Security;
using Gabriel.DesafioFrontEnd.Api.Domain.Data;
using Gabriel.DesafioFrontEnd.Api.Domain.Service;
using Gabriel.DesafioFrontEnd.Api.Helpers;
using Gabriel.DesafioFrontEnd.Api.Helpers.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddSingleton<AuthorizedAppService>();
builder.Services.AddJwtAuthorization(builder.Configuration.GetSection("Jwt").Get<JwtSettings>());
builder.Services.AddAutoMapper(x =>
{
    x.AddProfile(new AppMapProfile(builder.Configuration));
});

// Add services to the container.
builder
    .Services
    .AddControllers()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options
    .SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API - Desafio front end",
        Description = "Api para suportar o desafio técnico proposto para os candidatos front end.",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
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

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), includeControllerXmlComments: true);


});


var app = builder.Build();
app.UseStaticFiles();


app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.RoutePrefix = "docs";
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    opt.InjectStylesheet("/assets/swagger-ui/custom.css");
});


app.UseSeedCustomers(app.Services);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


