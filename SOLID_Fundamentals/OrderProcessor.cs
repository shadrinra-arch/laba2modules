using System;
using System.Collections.Generic;
using System.Text;

namespace SOLID_Fundamentals
{
    public class OrderProcessor
    {
        private readonly List<Order> _orders = new(); // тут теперь private

        public void AddOrder(Order order)
        {
            _orders.Add(order); // тут теперь private
            Console.WriteLine($"Order {order.Id} added");
        }

        public interface IInventoryService { void Update(IReadOnlyList<string> items); }
        public interface INotificationService { void SendEmail(string to, string subject, string body); }
        public interface ILoggingService { void LogInfo(string message); }
        public interface IReceiptService { void Generate(Order order); }

        // ----------- сам процессор – теперь только координация -----------
        public sealed class OrderProcessorCore
        {
            private readonly List<Order> _orders = new();
            private readonly IInventoryService _inventory;
            private readonly INotificationService _notifications;
            private readonly ILoggingService _logging;
            private readonly IReceiptService _receipts;

            public OrderProcessorCore(
                IInventoryService inventory,
                INotificationService notifications,
                ILoggingService logging,
                IReceiptService receipts)
            {
                _inventory = inventory;
                _notifications = notifications;
                _logging = logging;
                _receipts = receipts;
            }

            public void AddOrder(Order order)
            {
                _orders.Add(order);
                Console.WriteLine($"Order {order.Id} added");
            }

            public void ProcessOrder(int orderId)
            {
                var order = _orders.FirstOrDefault(o => o.Id == orderId);
                if (order is null) return;

                if (order.TotalAmount <= 0)
                    throw new InvalidOperationException("Invalid order amount");

                order.Payment.Pay(order.TotalAmount);

                _inventory.Update(order.Items);
                _notifications.SendEmail(order.CustomerEmail,
                                         $"Order {orderId} processed",
                                         $"Your order #{orderId} on sum {order.TotalAmount:C} is complete.");
                _logging.LogInfo($"Order {orderId} processed at {DateTime.Now:u}");
                _receipts.Generate(order);
            }

            // Формирование отчёта остаётся внутри процессора – это его собственная ответственность.
            public void GenerateMonthlyReport()
            {
                var totalRevenue = _orders.Sum(o => o.TotalAmount);
                var totalOrders = _orders.Count;
                Console.WriteLine($"Monthly Report: {totalOrders} orders, Revenue: {totalRevenue:C}");
            }
        }

        // ----------- заглушки-реализации для быстрого старта -----------
        public sealed class DummyInventoryService : IInventoryService
        {
            public void Update(IReadOnlyList<string> items) => Console.WriteLine("Inventory updated");
        }

        public sealed class DummyNotificationService : INotificationService
        {
            public void SendEmail(string to, string subject, string body) => Console.WriteLine($"Email sent to {to}");
        }

        public sealed class ConsoleLoggingService : ILoggingService
        {
            public void LogInfo(string message) => Console.WriteLine($"LOG: {message}");
        }

        public sealed class DummyReceiptService : IReceiptService
        {
            public void Generate(Order order) => Console.WriteLine($"Receipt generated for order {order.Id}");
        }
    }
}