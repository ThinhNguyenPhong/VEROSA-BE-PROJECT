using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VEROSA_BE_PROJECT.Mappers;
using VEROSA_BE_PROJECT.Middlewares;
using VEROSA.BussinessLogicLayer.PasswordHash;
using VEROSA.BussinessLogicLayer.Services.Account;
using VEROSA.BussinessLogicLayer.Services.Address;
using VEROSA.BussinessLogicLayer.Services.BeautyService;
using VEROSA.BussinessLogicLayer.Services.BlogPost;
using VEROSA.BussinessLogicLayer.Services.Contact;
using VEROSA.BussinessLogicLayer.Services.Email;
using VEROSA.BussinessLogicLayer.Services.Favorite;
using VEROSA.BussinessLogicLayer.Services.Product;
using VEROSA.BussinessLogicLayer.Services.Review;
using VEROSA.BussinessLogicLayer.Services.SupportTicket;
using VEROSA.Common.Models.Settings;
using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Bases.UnitOfWork;
using VEROSA.DataAccessLayer.Context;

var builder = WebApplication.CreateBuilder(args);

// 1) DbContext
builder.Services.AddDbContext<VerosaBeautyContext>(opt =>
    opt.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 33)),
        mysql => mysql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
    )
);

// 2) Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.Configure<FrontendSettings>(builder.Configuration.GetSection("Frontend"));

// 3) Authentication + Authorization
var jwt = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwt.Key);

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            // match với claim key bên dưới ("role")
            RoleClaimType = ClaimTypes.Role,
        };
    });

// override 403 handler
builder.Services.AddSingleton<
    IAuthorizationMiddlewareResultHandler,
    CustomAuthorizationResultHandler
>();

builder.Services.AddAuthorization();

// 4) AutoMapper
builder.Services.AddAutoMapper(
    typeof(Program),
    typeof(AuthService),
    typeof(BeautyServiceMapper),
    typeof(AccountMapper),
    typeof(AddressMapper),
    typeof(BlogPostMapper),
    typeof(ProductCategoryMapper),
    typeof(ProductMapper),
    typeof(ContactMapper),
    typeof(FavoriteMapper),
    typeof(ReviewMapper),
    typeof(SupportTicketMapper)
);

// 5) DI: Repos,UnitOfWork , Services.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, MailKitEmailService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IBeautyServiceService, BeautyServiceService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<ISupportTicketService, SupportTicketService>();

// 6) Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VEROSA API", Version = "v1" });

    // Thay đổi ở đây:
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http, // <-- đổi từ ApiKey thành Http
            Scheme = "bearer", // <-- scheme phải là "bearer"
            BearerFormat = "JWT",
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});

// 7) Controllers
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
