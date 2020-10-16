using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MS.AFORO255.Cross.Jaeger.Jaeger;
using MS.AFORO255.Cross.Jwt.Src;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace MS.AFORO255.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyPolicy = "_myPolicy"; //PARA NUESTRA POLITICA DE CORS

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*Start - Cors*/
            services.AddCors(o => o.AddPolicy(MyPolicy, builder =>
            {
                builder.AllowAnyOrigin() //permitir todos los origines
                       .AllowAnyMethod() // todos los metodos GET PUT DELETE POST
                       .AllowAnyHeader(); // todos los encabezados
            }));
            services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true); //PERMITIR LAS METADATAS
            /*End - Cors*/

            services.AddJwtCustomized();
            services.AddOcelot();

            /*Jaguer - Logs - Manejo de trazas*/
            services.AddJaeger();
            services.AddOpenTracing();
            /*Jaguer - Logs - Manejo de trazas*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*Start - Cors*/
            app.UseCors(MyPolicy);
            app.Use((context, next) =>
            {
                context.Items["__CorsMiddlewareInvoked"] = true;
                return next();
            });
            /*End - Cors*/

            app.UseOcelot().Wait();            
        }
    }
}
