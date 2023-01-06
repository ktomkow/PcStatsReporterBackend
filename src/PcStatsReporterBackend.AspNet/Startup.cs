using System.Reflection;
using Microsoft.OpenApi.Models;
using PcStatsReporterBackend.AspNet.SignalR;

namespace PcStatsReporterBackend.AspNet;

public class Startup
{
    public IWebHostEnvironment Env { get; }
    public IConfigurationRoot Configuration { get; }

    public Startup(IWebHostEnvironment env)
    {
        Env = env;
        
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        Configuration = builder.Build();
    }


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pc Stats Reporter API", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        
        services.AddMySignalR();
    }

    public void Configure(IApplicationBuilder app)
    {
        // Configure the HTTP request pipeline.
        if (Env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pc Stats Reporter API V1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseRouting();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapMySignalR();
        });
    }
}