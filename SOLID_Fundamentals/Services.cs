using System;

namespace SOLID_Fundamentals
{
    // Абстракции
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }

    public interface ISmsService
    {
        void SendSms(string phoneNumber, string message);
    }

    public interface INotificationService
    {
        void SendPromotion(string email, string promotion);
    }

    // Конкретные реализации
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            Console.WriteLine($"Sending email to {to}: {subject}");
        }
    }

    public class SmsService : ISmsService
    {
        public void SendSms(string phoneNumber, string message)
        {
            Console.WriteLine($"Sending SMS to {phoneNumber}: {message}");
        }
    }

    // Высокоуровневый класс
    public class OrderService
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public OrderService(IEmailService emailService, ISmsService smsService)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
        }

        public void PlaceOrder(Order order)
        {
            _emailService.SendEmail(order.CustomerEmail, "Order Confirmation", "Your order has been placed");
            _smsService.SendSms(order.CustomerPhone, "Your order has been placed");
        }
    }

    // Другой высокоуровневый класс
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;

        public NotificationService(IEmailService emailService)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public void SendPromotion(string email, string promotion)
        {
            _emailService.SendEmail(email, "Special Promotion", promotion);
        }
    }

    // Пример модели заказа
    public class CustomerOrder
    {
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}
