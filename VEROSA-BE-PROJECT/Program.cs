using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VEROSA.BussinessLogicLayer.Services.Auth;
using VEROSA.Common.Settings.JWT;
using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Bases.UnitOfWork;
using VEROSA.DataAccessLayer.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VerosaBeautyContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 33)),
        mySqlOptions =>
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            )
    )
);

// Add these lines after builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper before building the app
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Add these lines before app.UseAuthentication();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3) Repositories & UnitOfWork
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 4) Services
builder.Services.AddScoped<IAuthService, AuthService>();

// 5) Authentication / JWT
var jwtConfig = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtConfig);
var jwtSettings = jwtConfig.Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opts =>
    {
        opts.RequireHttpsMetadata = false;
        opts.SaveToken = true;
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddControllers();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
