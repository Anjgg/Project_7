using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using P7CreateRestApi;
using P7CreateRestApi.Config;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;
using P7CreateRestApi.SwaggerConfig;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


/* --------------------------------
 * ---- Database Configuration ---- 
 * -------------------------------- */
builder.Services.AddDbContext<LocalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));


/* ------------------------------
 * ---- Dependency Injection ----
 * ------------------------------ */
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IBidService, BidService>();
builder.Services.AddScoped<ICurvePointService, CurvePointService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IRuleService, RuleService>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();


/* ------------------------------
 * ---- API Versioning Setup ---- 
 * ------------------------------ */
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new Asp.Versioning.ApiVersion(1);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
})
.AddMvc(options =>
{
    options.Conventions.Add(new VersionByNamespaceConvention());
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});


/* ----------------------------------
 * ---- AutoMapper Configuration ---- 
 * ---------------------------------- */
builder.Services.AddAutoMapper(config =>
{
    config.CreateMap<Bid, BidDto>().ReverseMap();
    config.CreateMap<CurvePoint, CurvePointDto>().ReverseMap();
    config.CreateMap<Rating, RatingDto>().ReverseMap();
    config.CreateMap<Rule, RuleDto>().ReverseMap();
    config.CreateMap<Trade, TradeDto>().ReverseMap();
}, typeof(Program));


/* --------------------------------
 * ---- Identity Configuration ---- 
 * -------------------------------- */
builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<LocalDbContext>()
    .AddDefaultTokenProviders();


/* ------------------------------------------
 * ---- JWT Authentication Configuration ----
 * ------------------------------------------ */
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
    options.DefaultScheme = "Bearer";
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer")!,
        ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience")!,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key")!))
    };
});
builder.Services.AddAuthorization();


/* -------------------------------
 * ---- Swagger Configuration ----
 * ------------------------------- */
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDocumentationOperationFilter>();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[]{}
        }
    });
});


var app = builder.Build();


/* ----------------------------------
 * ---- Middleware Configuration ----
 * ---------------------------------- */
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RolesSeeder.EnsureSeedRolesAsync(roleManager);
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UserLoggingMiddleware>();


app.MapControllers();

app.Run();
