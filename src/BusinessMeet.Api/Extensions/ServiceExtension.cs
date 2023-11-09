using BusinessMeet.Data.IRepositories;
using BusinessMeet.Data.Repositories;
using BusinessMeet.Service.Interfaces.Companys;
using BusinessMeet.Service.Interfaces.IEmailServices;
using BusinessMeet.Service.Interfaces.IFileUploadServices;
using BusinessMeet.Service.Interfaces.Meet;
using BusinessMeet.Service.Interfaces.UserMeets;
using BusinessMeet.Service.Interfaces.Users;
using BusinessMeet.Service.Mappers;
using BusinessMeet.Service.Services.Companys;
using BusinessMeet.Service.Services.EmailService;
using BusinessMeet.Service.Services.FileUploadService;
using BusinessMeet.Service.Services.Meets;
using BusinessMeet.Service.Services.UserMeets;
using BusinessMeet.Service.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace BusinessMeet.Api.Extensions;

public static class ServiceExtension
{
    public static void AddCustomService(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddAutoMapper(typeof(MapperProfile));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMeetService, MeetService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IUserMeetService, UserMeetService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<IEmailService, EmailService>();
    }

    public static void AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    public static void AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "BusinessMeet.Api", Version = "v1" });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                In = ParameterLocation.Header,
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
                new string[]{ }
            }
        });
        });
    }
}
