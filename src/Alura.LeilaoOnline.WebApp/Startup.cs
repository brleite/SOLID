using Alura.LeilaoOnline.WebApp.Dados;
using Alura.LeilaoOnline.WebApp.Dados.DAO;
using Alura.LeilaoOnline.WebApp.Dados.DAO.EfCore;
using Alura.LeilaoOnline.WebApp.Seeding;
using Alura.LeilaoOnline.WebApp.Services;
using Alura.LeilaoOnline.WebApp.Services.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Alura.LeilaoOnline.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            /*
             services
                .AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
                */

            services.AddMvc(cfg =>
            {
                cfg.ReturnHttpNotAcceptable = true;
                cfg.InputFormatters.Add(new XmlSerializerInputFormatter(cfg));
                cfg.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                /*
                var jsonOutputFormatter = cfg.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();
                if (jsonOutputFormatter != null)
                {
                    jsonOutputFormatter.SupportedMediaTypes.Add(CustomMediaType.Hateoas);
                }
                */
            }).
            SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            /*
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                */

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("SOLIDPostgresConnection")));

            // Por requisição
            services.AddTransient<ILeilaoDao, LeilaoDaoComEfCore>();
            services.AddTransient<ICategoriaDao, CategoriaDaoComEfCore>();

            services.AddScoped<IAdminService, ArquivamentoAdminService>();
            services.AddScoped<IProdutoService, DefaultProdutoService>();
            services.AddScoped<IBuscaService, DefaultBuscaService>();

            var sp = services.BuildServiceProvider();
            var ctx = sp.GetService<AppDbContext>();
            DatabaseGenerator.Seed(ctx);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePagesWithRedirects("/Home/StatusCode/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // app.UseRouting();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
