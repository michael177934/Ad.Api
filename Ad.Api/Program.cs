using Ad.Core;
using Ad.Core.Configurations;
using Ad.Core.Models;
using Ad.Core.Services;
using Ad.Data;
using Ad.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ad.API.ActionFilters;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
Directory.CreateDirectory(logPath);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(Path.Combine(logPath, "log.txt"), restrictedToMinimumLevel: LogEventLevel.Information)
    .CreateLogger();
builder.Services.AddControllers();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opt => { opt.User.RequireUniqueEmail = false; })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
builder.Configuration.AddJsonFile("appsettings.json", optional: true);
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(true);
});


builder.Services.AddSingleton(Log.Logger);
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("JwtSetting"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IUnitOfWork, UnitofWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
            builder.Configuration.GetSection("JwtSetting:Secret").Value))
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Advancly Take Home Assessment", Version = "v1" });
    c.OperationFilter<TenantHeaderOperationFilter>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization bearer token",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
});

builder.Services.AddControllers().AddMvcOptions(opt =>
{
    var authorizePolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser().Build();

    opt.Filters.Add(new AuthorizeFilter(authorizePolicy));
    opt.Filters.Add(typeof(DynamicAuthorizationFilter));
});

builder.Services.Configure<ApiBehaviorOptions>(config =>
{
    config.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Where(e => e.Value.Errors.Count > 0)
            .Select(e => new
            {
                FieldName = e.Key,
                Message = e.Value.Errors.Select(e => e.ErrorMessage).ToList()
            });

        return new OkObjectResult(new
        { ResponseCode = "99", Message = "Input validation error", Data = errors });
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
