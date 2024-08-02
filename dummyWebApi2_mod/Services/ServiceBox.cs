using dummyWebApi2.Models.Data;
using dummyWebApi2.Models.SharedDictionary;
using dummyWebApi2.Services.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace dummyWebApi2.Services
{
    public static class ServiceBox
    {
        /// <summary>
        /// Allows to configure services the more epic way
        /// </summary>
        public static IServiceCollection PlugServices(this IServiceCollection services, IConfiguration configuration)
        {
            // this can only be used in development / for test purposes only
            /*
            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition(PolicySettingNames.BearerScheme, new OpenApiSecurityScheme()
                {
                    Name = PolicySettingNames.AuthorizedRequirement,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = PolicySettingNames.BearerScheme,
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
                                Id = PolicySettingNames.BearerScheme
                            }
                        }, Array.Empty<string>()
                    }
                });
            });*/

            services.AddSwaggerGen();

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

            services.AddScoped<IUser, UserManager>();

            //services.AddScoped<AuthorizationValidationService>();

            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthorizationHandler, AuthorizationRequirementHandler>();

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Policies.AppUser, policy =>
                {
                    policy.Requirements.Add(new AuthorizationRequirement(PolicySettingNames.AuthorizedRequirement));
                });
                opt.AddPolicy(Policies.AppAdmin, policy =>
                {
                    policy.Requirements.Add(new AuthorizationRequirement(PolicySettingNames.AuthorizedRequirement, "Admin"));
                });
            });

            // Configure token generator "service"
            TokenManager.SetConfiguration(configuration);

            return services;
        }
    }
}
