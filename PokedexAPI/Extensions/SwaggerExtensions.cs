using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PokedexApi.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddConfigureSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pokedex API",
                    Description = "APP Covid",
                    Version = "v1"
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[]{ } }
                };

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });
            });
        }

        public static IApplicationBuilder UseCustomizedSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (!env.IsProduction())
            //{
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();
                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.DisplayRequestDuration();
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemon API v1.0");
                });
            //}
            return app;
        }
    }
}
