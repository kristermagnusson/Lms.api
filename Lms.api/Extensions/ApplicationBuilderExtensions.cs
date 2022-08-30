using Lms.Data.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lms.api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedDataAsynk(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<LmsapiContext>();
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                try
                {
                    await SeedData.initAsync(db, serviceProvider);
                }
                catch (Exception e)


                {

                    throw;
                }
            }
            return app;
        }
    }
}
