using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Todo.Data;

namespace Todo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Info("Initializing app..");
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetService<ToDoContext>();

                        context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "An error while migrating the database");
                    }
                    host.Run();
                }

            } catch (Exception ex)
            {
                logger.Error(ex, "Application stopped because of exception.");
                throw;
            } finally
            {
                NLog.LogManager.Shutdown();
            }


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .ConfigureLogging(logging =>
             {
                 logging.ClearProviders();
                 logging.AddConsole();
             })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
