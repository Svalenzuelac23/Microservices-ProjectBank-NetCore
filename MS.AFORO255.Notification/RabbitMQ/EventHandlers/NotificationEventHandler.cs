using MS.AFORO255.Cross.RabbitMQ.Src.Bus;
using MS.AFORO255.Notification.Model;
using MS.AFORO255.Notification.RabbitMQ.Events;
using MS.AFORO255.Notification.Repository;
using System;
using System.Threading.Tasks;

namespace MS.AFORO255.Notification.RabbitMQ.EventHandlers
{
    public class NotificationEventHandler : IEventHandler<NotificationCreatedEvent> //ESTA INTERFAZ ESTA EN EL PROYECTO DE RABBIT PERMITE TRABAJAR CON LOS EVENTOS DE ESA COLA
    {
        private readonly IMailRepository _mailRepository;

        public NotificationEventHandler(IMailRepository mailRepository)
        {
            _mailRepository = mailRepository;
        }

        //CADA VEZ QUE ENCONTREMOS UN EVENTO EN LA COLA DE TIPO DEPOSITCREATEDEVENT LO CAPTURARA ESTE MANEJADOR Y VA HACER UN REGISTRO EN LA BASE DE DATOS
        public Task Handle(NotificationCreatedEvent @event)
        {
            _mailRepository.Add(new SendMail()
            {
                SendDate = DateTime.Now.ToShortDateString(),
                AccountId = @event.AccountId
            });

            return Task.CompletedTask;
        }
    }
}
