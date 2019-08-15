namespace TestWebAPI.Library
{
    using System;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using TestWebAPI.Data;

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
            using (var scope = webHost.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<EmployeeDataContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        throw;
                    }
                }
            }

            return webHost;
        }
    }
}
