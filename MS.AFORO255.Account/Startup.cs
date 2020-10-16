using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MS.AFORO255.Account.Repository;
using MS.AFORO255.Account.Repository.Data;
using MS.AFORO255.Account.Service;
using MS.AFORO255.Cross.Consul.Consul;
using MS.AFORO255.Cross.Consul.Mvc;
using MS.AFORO255.Cross.Jaeger.Jaeger;
using MS.AFORO255.Cross.Metrics.Registry;

namespace MS.AFORO255.Account
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
            services.AddControllers();

            services.AddDbContext<ContextDatabase>(
                options =>
                {
                    //options.UseSqlServer(Configuration["sql:cn"]);                
                    options.UseSqlServer(Configuration["cnsql"]); //ACA ESTAMOS USANDO LA CONEXION DESDE LA CONFIGURACION CENTRALIZDA           
                });


            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IContextDatabase, ContextDatabase>();


            /*Start - Consul*/
            services.AddSingleton<IServiceId, ServiceId>(); //lo unico que hace es crear un GUID para poder registrarme como instancia en el Registro y descumibrimiento
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //es para comunicarme con los ms y la consumir el endpoint para saber el estado de salud
            services.AddConsul(); //agregar toda la config de consult
            /*End - Consul*/

            /*Jaguer - Logs - Manejo de trazas*/
            services.AddJaeger();
            services.AddOpenTracing();
            /*Jaguer - Logs - Manejo de trazas*/


            //PROMETHEUS - Start
            /*Start - Metrics*/
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddTransient<IMetricsRegistry, MetricsRegistry>();
            /*End - Metrics*/




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env
            , IHostApplicationLifetime hostApplicationLifetime //ESTE NOS AYUDARA A TRABJAR CON EL TIEMPO DE VIDA DE LA APICACION, ACA PODEMOS SABE CUANDO LA APP SE APAGUE Y REMOVERME DEL WHITELIST DE CONSULT
            , IConsulClient consulClient // ESTE ES PARA TRABABJAR CON LA COMUNICACION CON CONSULT
            , ILoggerFactory loggerFactory //PARA EL TEMA DE LOG CENTRAZLIADOS CON  EL PRODUCTO - SEQ
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //COSUL ---
            var serviceId = app.UseConsul(); //OBTENEMOS EL DI DEL REGISTRO DE CONSUL
            hostApplicationLifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceId); //ACA ES SI NOS APAGMOS QUITAME DE LA LISTA
            });

            if (bool.Parse(Configuration["seq:enabled"]) == true)
            {
                loggerFactory.AddSeq(Configuration["seq:url"], apiKey: Configuration["seq:token"]);
            }
        }
    }
}
