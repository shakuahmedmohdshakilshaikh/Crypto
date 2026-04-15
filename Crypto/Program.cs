using Asp.Versioning;
using Asp.Versioning.Conventions;
using Crypto.Middleware;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using DDDCryptoWebApi.Application.Mapping;
using DDDCryptoWebApi.Infrastructure.Data;
using DDDCryptoWebApi.Infrastructure.Jobs;
using DDDCryptoWebApi.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log.txt",
        rollingInterval: RollingInterval.Infinite,
        shared: true
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddResponseCaching(); // response caching

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn"))
);


//coingeko data fetching services
builder.Services.Configure<CoinGeckoSettings>(
    builder.Configuration.GetSection("CoinGeckoSettings"));

builder.Services.AddHttpClient<ICoinGeckoService, CoinGeckoService>(
    (serviceProvider, client) =>
    {
        var config = serviceProvider.GetRequiredService<IConfiguration>();

        var settings = config
            .GetSection("CoinGeckoSettings")
            .Get<CoinGeckoSettings>();

        client.BaseAddress = new Uri(settings.BaseUrl);

        if (!string.IsNullOrEmpty(settings.ApiKey))
        {
            client.DefaultRequestHeaders.Add("x-cg-demo-api-key", settings.ApiKey);
        }
    });

builder.Services.AddHostedService<CryptoSyncJob>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserFavouriteService, UserFavouriteService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddAutoMapper(typeof(DTOMapping));

// Jwt
builder.Services.AddAuthentication("JwtBearer")
    .AddJwtBearer("JwtBearer", options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

//api versioning 
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddMvc(options => {
    options.Conventions.Add(new VersionByNamespaceConvention());
}).AddApiExplorer(option =>
{
    option.GroupNameFormat = "'v'V";
    option.SubstituteApiVersionInUrl = true; // for in swaggerTesting
});

// rate limiter
builder.Services.AddRateLimiter(options => {
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 5; // max limit
        opt.Window = TimeSpan.FromSeconds(10);

        //  This enables throttling (queue instead of reject)
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular"); // cors
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseResponseCaching(); // resonse caching
app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter(); // global ratelimit
app.MapControllers().RequireRateLimiting("fixed");  //global ratelimit

app.Run();