namespace TT.WebClient.Application;

/* [TODO]
 * After pass TT.Core into shared libs this class has to impement IApplication from it!
 */
public class TaskTrainWebClientApp
{
    #region Startup initializer
    private class Initialize 
    {
        public IConfiguration Configuration { get; }

        private readonly string _pgConnection;

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
            builder.UseStaticFiles();

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
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => 
            {
                webBuilder.UseStartup<Initialize>()
                    .UseKestrel(options => { })
                    .UseUrls("http://*:6996");
            });

        _app = builder.Build();
    }

    public void Run() 
    {
        _app.Run();
    }
}
