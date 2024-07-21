using ETicaretAPI.Application;
using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Persistence;
using ETicaretAPI.SignalR;
using ETicaretAPI.WebAPI.Extensions;
using ETicaretAPI.WebAPI.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor(); // Client'tan gelen isteklerin bilgilerine eriþmek için kullanýlýr.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();

builder.Services.AddStorage<AzureStorage>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});

var columnOptions = new ColumnOptions()
{
    AdditionalColumns =
    [
        new() { ColumnName = "Username", DataType = SqlDbType.NVarChar, DataLength = 256, AllowNull = true }
    ],
    Store =
        [
            StandardColumn.Message,
            StandardColumn.MessageTemplate,
            StandardColumn.Level,
            StandardColumn.TimeStamp,
            StandardColumn.Exception,
            StandardColumn.LogEvent,
        ]
};

Logger log = new LoggerConfiguration()
    .WriteTo.MSSqlServer(
        builder.Configuration.GetConnectionString("MsSql"), "Logs",
        autoCreateSqlTable: true,
        columnOptions: columnOptions)
    .WriteTo.Seq(builder.Configuration["Seq:Url"])
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

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<RolePermissionFilter>();
})
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // Token deðerini kimin kullanacaðýný ifade edeceðimiz deðerdir. (www.bilmemne.com)
            ValidateIssuer = true, // Token deðerini kimin oluþturduðunu ifade edeceðimiz deðerdir. (www.myapi.com)
            ValidateLifetime = true, // Token'ýn geçerlilik süresini kontrol eder.
            ValidateIssuerSigningKey = true, // Token'ýn imzalanýp imzalanmadýðýný kontrol eder.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,

            NameClaimType = ClaimTypes.Name
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());

app.UseSerilogRequestLogging();

app.UseHttpLogging();
app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.Use(async (context, next) =>
{
    var username = (context.User?.Identity?.IsAuthenticated != null || true) ? context.User.Identity.Name : null;
    LogContext.PushProperty("Username", username);
    await next();
});

app.MapControllers();
app.MapHubs();

app.Run();
