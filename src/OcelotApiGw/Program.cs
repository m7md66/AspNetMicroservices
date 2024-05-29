using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OcelotApiGw
{

    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        new WebHostBuilder()
    //        .UseKestrel()
    //        .UseContentRoot(Directory.GetCurrentDirectory())
    //        .ConfigureAppConfiguration((hostingContext, config) =>
    //        {
    //            config
    //                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
    //                .AddJsonFile("appsettings.json", true, true)
    //                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
    //                .AddJsonFile("ocelot.json")
    //                .AddEnvironmentVariables();
    //        })
    //        .ConfigureServices(s => {
    //            s.AddOcelot();
    //        })
    //        .ConfigureLogging((hostingContext, logging) =>
    //        {
    //            //add your logging
    //        })
    //        .UseIISIntegration()
    //        .Configure(app =>
    //        {
    //            app.UseOcelot().Wait();
    //        })
    //        .Build()
    //        .Run();
    //    }
    //}
















    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);
            builder.Configuration.AddJsonFile("ocelot.json", true, true);

            //builder.Host.ConfigureLogging((hostingContext, loggingBuilder) =>
            //{
            //    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            //    loggingBuilder.AddConsole();
            //    loggingBuilder.AddDebug();
            //});

            //builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging")).AddConsole().AddDebug();

            ////builder.Services.AddOcelot(builder.Configuration);

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");
            app.UseOcelot().Wait();
            app.Run();
        }
    }

}
