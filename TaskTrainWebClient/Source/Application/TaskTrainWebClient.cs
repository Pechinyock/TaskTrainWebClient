using Microsoft.Extensions.FileProviders;

namespace TT.WebClient.Application;

/* [TODO]
 * - After pass TT.Core into shared libs this class has to implement IApplication from it!
 * - Hide everything in extension methods this file should looks like:
 *   services.Use[Something](<params>)
 * - UseUrls gets urls form config file. Might be deference depends on $Configuration
 */
public class TaskTrainWebClientApp
{
    #region Startup initializer
    private class Initialize
    {
        public IConfiguration Configuration { get; }

        public Initialize(IHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("Config/appsettings.json", false, true)
                .AddJsonFile($"Config/appsettings.{env.EnvironmentName}.json")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder builder, IWebHostEnvironment env)
        {

            if (!env.IsDevelopment())
            {
                builder.UseExceptionHandler("/Error");
                //builder.UseHsts();
            }

            builder.UseHttpsRedirection();
            var pathToRoot = Path.Combine(env.ContentRootPath, "wwwroot");
            builder.UseStaticFiles();
            builder.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(pathToRoot)
            });

            builder.UseRouting();

            builder.UseAuthorization();

            builder.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
    #endregion
    private IHost _app;

    public void Build(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Initialize>()
                .UseKestrel(options => { })
                .UseUrls("http://*:5002");

            webBuilder.UseWebRoot(AppContext.BaseDirectory);
        });

        _app = builder.Build();
    }

    public void Run() => _app.Run();
}
