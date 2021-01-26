using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using API.Options;
using DataAccessLayer.DBContext;
using DataAccessLayer;
using API.Hubs;
using API.Games;
using API.Service;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DatabaseContext")));
            services.AddScoped<UnitOfWork>();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options=> 
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.AddControllers();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Arena API", Version = "v1" });
            });
            services.AddSingleton<GameFactory>();
            services.AddSingleton<GameHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "Arena API");
                option.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GameHub>("/gamehub");
            });
        }
    }
}
