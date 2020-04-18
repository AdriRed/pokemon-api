using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Extensions
{
    public static class ExceptionHandler
    {
        public static IApplicationBuilder UsePokemonExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();

                    // Use exceptionHandlerPathFeature to process the exception (for example, 
                    // logging), but do NOT expose sensitive error information directly to 
                    // the client.

                    if (exceptionHandlerPathFeature?.Error is PokemonAPIException pexc)
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new 
                        { 
                            InnerCode = pexc.Code, 
                            Reason = pexc.Message
                        }));
                    }
                });
            });
            app.UseHsts();
            return app;
        }

    }
}
