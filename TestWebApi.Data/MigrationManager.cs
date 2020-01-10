namespace TestWebApi.Data
{
    using System;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using TestWebApi.Data.Contexts;

    /// <summary>
    /// The migration manager.
    /// </summary>
    public static class MigrationManager
    {
        /// <summary>
        /// The migrate database.
        /// </summary>
        /// <param name="webHost">
        /// The web host.
        /// </param>
        /// <returns>
        /// The <see cref="IWebHost"/>.
        /// </returns>
        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                using (EmployeeDataContext context = scope.ServiceProvider.GetRequiredService<EmployeeDataContext>())
                {
                    try
                    {
                        // context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw;
                    }
                }
            }

            return webHost;
        }
    }
}
