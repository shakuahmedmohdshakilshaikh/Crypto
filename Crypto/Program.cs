using Crypto.Middleware;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using DDDCryptoWebApi.Application.Mapping;
using DDDCryptoWebApi.Infrastructure.Data;
using DDDCryptoWebApi.Infrastructure.Jobs;
using DDDCryptoWebApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//logger
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn"))
);


builder.Services.Configure<CoinGeckoSettings>(
    builder.Configuration.GetSection("CoinGeckoSettings"));

builder.Services.AddHttpClient<ICoinGeckoService, CoinGeckoService>(
    (serviceProvider, client) =>
    {
        var config = serviceProvider
            .GetRequiredService<IConfiguration>();

        var settings = config
            .GetSection("CoinGeckoSettings")
            .Get<CoinGeckoSettings>();

        client.BaseAddress = new Uri(settings.BaseUrl);

        client.DefaultRequestHeaders.Add(
            "x-cg-demo-api-key",
            settings.ApiKey);
    });
builder.Services.AddHttpClient<ICoinGeckoService, CoinGeckoService>();

builder.Services.AddHostedService<CryptoSyncJob>();



builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserFavouriteService, UserFavouriteService>();
builder.Services.AddAutoMapper(typeof(DTOMapping)); // Automapper


builder.Services.AddAuthentication("JwtBearer")
    .AddJwtBearer("JwtBearer", options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
             
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });


builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); //to make authenication available  to application
app.UseAuthorization();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllers();

app.Run();
