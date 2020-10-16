using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MS.AFORO255.Cross.Consul.Consul;
using MS.AFORO255.Cross.Consul.Mvc;
using MS.AFORO255.Cross.Jaeger.Jaeger;
using MS.AFORO255.Cross.Jwt.Src;
using MS.AFORO255.Security.Repository;
using MS.AFORO255.Security.Repository.Data;
using MS.AFORO255.Security.Service;

namespace MS.AFORO255.Security
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
            services.Configure<JwtOptions>(Configuration.GetSection("jwt")); //ACA ESTAMOS MAPEANDO LA INFORMACION DEL JSON DE CONFIG A UNA CLASE PARA TIPARLA

            services.AddControllers();

            services.AddDbContext<ContextDatabase>( //AGREGAR LA BASE DE DATOS DE MYSQL, FORMA DE CONEXION
               opt =>
               {
                   //opt.UseMySQL(Configuration["mysql:cn"]);
                   opt.UseMySQL(Configuration["cnmysql"]);
               });

            services.AddScoped<IAccessService, AccessService>();
            services.AddScoped<IAccessRepository, AccessRepository>();
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env
            , IHostApplicationLifetime hostApplicationLifetime //ESTE NOS AYUDARA A TRABJAR CON EL TIEMPO DE VIDA DE LA APICACION, ACA PODEMOS SABE CUANDO LA APP SE APAGUE Y REMOVERME DEL WHITELIST DE CONSULT
            , IConsulClient consulClient // ESTE ES PARA TRABABJAR CON LA COMUNICACION CON CONSULT
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
        }
    }
}
