using Asp.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;
using P7CreateRestApi.SwaggerConfig;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDbContext<LocalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IBidService, BidService>();
builder.Services.AddScoped<ICurvePointService, CurvePointService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IRuleService, RuleService>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new Asp.Versioning.ApiVersion(1);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
}).AddMvc(options =>
{
    options.Conventions.Add(new VersionByNamespaceConvention());
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;   
});

builder.Services.AddAutoMapper(config =>
{
    config.CreateMap<Bid, BidDto>().ReverseMap();
    config.CreateMap<CurvePoint, CurvePointDto>().ReverseMap();
    config.CreateMap<Rating,RatingDto>().ReverseMap();
    config.CreateMap<Rule, RuleDto>().ReverseMap();
    config.CreateMap<Trade, TradeDto>().ReverseMap();
    config.CreateMap<User, UserDto>().ReverseMap();
}, typeof(Program));

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
