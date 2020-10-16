using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MS.AFORO255.Cross.Jaeger.Jaeger;
using MS.AFORO255.Cross.RabbitMQ.Src;
using MS.AFORO255.Cross.RabbitMQ.Src.Bus;
using MS.AFORO255.Notification.RabbitMQ.EventHandlers;
using MS.AFORO255.Notification.RabbitMQ.Events;
using MS.AFORO255.Notification.Repository;
using MS.AFORO255.Notification.Repository.Data;

namespace MS.AFORO255.Notification
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

            services.AddDbContext<ContextDatabase>( //AGREGAR LA BASE DE DATOS DE MYSQL, FORMA DE CONEXION
               opt =>
               {
                   opt.UseMySQL(Configuration["cnmariadb"]);
               });
            
            services.AddScoped<IMailRepository, MailRepository>();
            services.AddScoped<IContextDatabase, ContextDatabase>();

            /*Start Rabbit MQ*/
            services.AddMediatR(typeof(Startup));
            services.AddRabbitMQ();

            services.AddTransient<NotificationEventHandler>(); //ACA ESTAMOS REGISTRANDO EL MANEJADOR DE EVENTOS            
            services.AddTransient<IEventHandler<NotificationCreatedEvent>, NotificationEventHandler>(); //REGISTRAR LA RELACION ENTRE EL MANEJADOR DE EVENTOS Y LA COLA, CADA QUE ESCUCHES UN MENSAJE EN LA COLA DE DEPOSITCREATE EVENT SE CAPTURE EN EL CAPUTRADOR DE EVENTOS
            /*End Rabbit MQ*/

            /*Jaguer - Logs - Manejo de trazas*/
            services.AddJaeger();
            services.AddOpenTracing();
            //services.AddOpenTracing();
            /*Jaguer - Logs - Manejo de trazas*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }

        //CON ESTE METODO NOS ESTAMOS SUSCRIBIENDO A LA COLA DE RABBITMQ Y PODER TRABJAR CON LOS MENSAJES EN LA COLA
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<NotificationCreatedEvent, NotificationEventHandler>();
            
        }
    }
}
