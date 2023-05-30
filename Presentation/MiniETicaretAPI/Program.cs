using System.Security.Claims;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniETicaret.Application;
using MiniETicaret.Application.Validators.Products;
using MiniETicaret.Infrastructure;
using MiniETicaret.Infrastructure.Filters;
using MiniETicaret.Infrastructure.Services.Storage.Azure;
using MiniETicaret.Infrastructure.Services.Storage.Local;
using MiniETicaret.Persistence;
using MiniETicaret.Persistence.Concretes;
using MiniETicaretAPI.Configurations.ColumnWriters;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();


//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();


builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()));

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"),"Logs", 
        needAutoCreateTable: true,
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            { "Message", new RenderedMessageColumnWriter() },
            { "MessageTemplate", new MessageTemplateColumnWriter() },
            { "Level", new LevelColumnWriter() },
            { "TimeStamp", new TimestampColumnWriter() },
            { "Exception", new ExceptionColumnWriter() },
            { "logEvent", new LogEventSerializedColumnWriter() },
            { "userName", new UserNameColumnWriter() }
            
            
        })
    .WriteTo.Seq(builder.Configuration["Seq:ServerUrl"])
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience = builder.Configuration["Token:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator =(
                notBefore,
                expires,
                securityToken,
                validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
            
            NameClaimType = ClaimTypes.Name

        };
    });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseSerilogRequestLogging();
app.UseHttpLogging();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var userName = context.User?.Identity?.IsAuthenticated !=null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("userName", userName);
    await next();
    
});

app.MapControllers();

app.Run();