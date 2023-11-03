using Ad.API.ActionFilters;
using Ad.Core.Constants;
using Microsoft.OpenApi.Models;

namespace Ad.API.ExtensionMethods;

public static class ServiceExtensions
{
    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Advancly Micro Service",
                Description = "Advancly Management APIs.",
                Contact = new OpenApiContact
                {
                    //Name = "Peerless Team",
                    //Email = "dev@peerless.co",
                    Url = new Uri(AppConstants.SwaggerContactUrl)
                }
            });
            c.OperationFilter<TenantHeaderOperationFilter>();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                //Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API"
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
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }
}