namespace SOLID_Fundamentals
{
    //ISP
    /* 1. Узкие, «ролевые» интерфейсы ---------------------- */

    // Для клиентского портала
    public interface ICustomerOrderService
    {
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int orderId);
    }

    // Для бэк-офиса, который управляет ордерами
    public interface IOrderManagementService
    {
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int orderId);
        void ProcessPayment(Order order);
        void ShipOrder(Order order);
        void GenerateInvoice(Order order);
        void SendNotification(Order order);
    }

    // Для администратора/аудита
    public interface IReportingService
    {
        void GenerateReport(DateTime from, DateTime to);
        void ExportToExcel(string filePath);
    }

    // Для DBA
    public interface IDatabaseAdminService
    {
        void BackupDatabase();
        void RestoreDatabase();
    }

    /* 2. Реализации, которые реализуют ТОЛЬКО нужные им роли */

    // Всеядный back-office
    public class OrderManager : IOrderManagementService,
                                IReportingService,
                                IDatabaseAdminService
    {
        public void CreateOrder(Order order) => Console.WriteLine("Order created");
        public void UpdateOrder(Order order) => Console.WriteLine("Order updated");
        public void DeleteOrder(int orderId) => Console.WriteLine("Order deleted");
        public void ProcessPayment(Order order) => Console.WriteLine("Payment processed");
        public void ShipOrder(Order order) => Console.WriteLine("Order shipped");
        public void GenerateInvoice(Order order) => Console.WriteLine("Invoice generated");
        public void SendNotification(Order order) => Console.WriteLine("Notification sent");

        public void GenerateReport(DateTime from, DateTime to) => Console.WriteLine("Report generated");
        public void ExportToExcel(string filePath) => Console.WriteLine("Exported to Excel");

        public void BackupDatabase() => Console.WriteLine("Database backed up");
        public void RestoreDatabase() => Console.WriteLine("Database restored");
    }

    // Клиентский портал зависит только от ICustomerOrderService
    public class CustomerPortal : ICustomerOrderService
    {
        public void CreateOrder(Order order) => Console.WriteLine("Order created by customer");
        public void UpdateOrder(Order order) => Console.WriteLine("Order updated by customer");
        public void DeleteOrder(int orderId) => Console.WriteLine("Order deleted by customer");
    }
}