using dummyWebApi2.Services.Security;
using Generics.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserService.Data;
using UserService.Models.Data;
using UserService.Models.InAppDictionary;
using UserService.Models.SharedDictionary;
using UserService.Services.Data;
using UserService.Services.Security;

namespace UserService.Services.Application
{
    public static class ServiceConfigurator
    {
        /// <summary>
        /// Allows to configure application services more epic way
        /// </summary>
        public static IServiceCollection PlugServices(this IServiceCollection services, IConfiguration configuration)
        {
            // this can only be used in development / for test purposes only
            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition(PolicySettingNames.CurrentScheme, new OpenApiSecurityScheme()
                {
                    Name = PolicySettingNames.AuthorizedRequirement,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = PolicySettingNames.CurrentScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement() // I don't know how this part works
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = PolicySettingNames.CurrentScheme
                            }
                        }, Array.Empty<string>()
                    }
                });
            });
            //services.AddSwaggerGen();

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("Default"));
            });

            services.AddScoped<IRepository<User>, UserManager>();
            services.AddScoped<IRepository<Role>, RoleManager>();
            services.AddScoped<IUserRoleRepository<UserRole>, UserRolesManager>();


            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:issuer"],
                        ValidAudience = configuration["Jwt:audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]!))
                    };
                });

            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthorizationHandler, AuthorizationRequirementHandler>();

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(PolicyNames.AppUser, policy =>
                {
                    policy.Requirements.Add(new AuthorizationRequirement(PolicySettingNames.AuthorizedRequirement));
                });
                opt.AddPolicy(PolicyNames.AppAdmin, policy =>
                {
                    policy.Requirements.Add(new AuthorizationRequirement(PolicySettingNames.AuthorizedRequirement, "Admin"));
                });
            });

            // very imporant line, configures user authentication sub-system once per program instance (singleton, in source)
            JwtTokenManager.SetConfigurationReference(configuration);

            return services;
        }
    }
}
