using Consul;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MS.AFORO255.Cross.Consul.Consul;
using MS.AFORO255.Cross.Consul.Mvc;
using MS.AFORO255.Cross.Jaeger.Jaeger;
using MS.AFORO255.Cross.RabbitMQ.Src;
using MS.AFORO255.Cross.RabbitMQ.Src.Bus;
using MS.AFORO255.Cross.Redis.Redis;
using MS.AFORO255.History.RabbitMQ.EventHandlers;
using MS.AFORO255.History.RabbitMQ.Events;
using MS.AFORO255.History.Repository;
using MS.AFORO255.History.Service;

namespace MS.AFORO255.History
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

            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<IHistoryService, HistoryService>();

            services.AddMediatR(typeof(Startup));
            services.AddRabbitMQ();

            services.AddTransient<DepositEventHandler>(); //ACA ESTAMOS REGISTRANDO EL MANEJADOR DE EVENTOS
            services.AddTransient<WithdrawalEventHandler>(); //ACA ESTAMOS REGISTRANDO EL MANEJADOR DE EVENTOS
            services.AddTransient<IEventHandler<DepositCreatedEvent>, DepositEventHandler>(); //REGISTRAR LA RELACION ENTRE EL MANEJADOR DE EVENTOS Y LA COLA, CADA QUE ESCUCHES UN MENSAJE EN LA COLA DE DEPOSITCREATE EVENT SE CAPTURE EN EL CAPUTRADOR DE EVENTOS
            services.AddTransient<IEventHandler<WithdrawalCreatedEvent>, WithdrawalEventHandler>(); //REGISTRAR LA RELACION ENTRE EL MANEJADOR DE EVENTOS Y LA COLA, CADA QUE ESCUCHES UN MENSAJE EN LA COLA DE DEPOSITCREATE EVENT SE CAPTURE EN EL CAPUTRADOR DE EVENTOS

            /*Start - Consul*/
            services.AddSingleton<IServiceId, ServiceId>(); //lo unico que hace es crear un GUID para poder registrarme como instancia en el Registro y descumibrimiento
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //es para comunicarme con los ms y la consumir el endpoint para saber el estado de salud
            services.AddConsul(); //agregar toda la config de consult
            /*End - Consul*/

            /*Redis*/
            services.AddRedis();
            /*Redis*/

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

            //ACA HABILITAMOS LA SUSCRIPCION A UNA COLA DE RABBIT MQ
            ConfigureEventBus(app);

            //COSUL ---
            var serviceId = app.UseConsul(); //OBTENEMOS EL DI DEL REGISTRO DE CONSUL
            hostApplicationLifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceId); //ACA ES SI NOS APAGMOS QUITAME DE LA LISTA
            });           
        }

        //CON ESTE METODO NOS ESTAMOS SUSCRIBIENDO A LA COLA DE RABBITMQ Y PODER TRABJAR CON LOS MENSAJES EN LA COLA
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<DepositCreatedEvent, DepositEventHandler>();
            eventBus.Subscribe<WithdrawalCreatedEvent, WithdrawalEventHandler>();
        }



    }
}
