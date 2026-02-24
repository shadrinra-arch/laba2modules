// файл, уже лежащий в пространстве SOLID_Fundamentals
namespace SOLID_Fundamentals
{
    //SRP
    #region Новые SRP-совместимые классы

    /* 1. Ответственность № 1 – хранить/выдавать правила скидок */
    public interface IDiscountStrategy
    {
        decimal GetDiscount(decimal orderAmount);
    }

    // конкретные стратегии
    public sealed class RegularDiscount : IDiscountStrategy
    {
        public decimal GetDiscount(decimal amount) => amount * 0.05m;
    }

    public sealed class PremiumDiscount : IDiscountStrategy
    {
        public decimal GetDiscount(decimal amount) => amount * 0.10m;
    }

    public sealed class VipDiscount : IDiscountStrategy
    {
        public decimal GetDiscount(decimal amount) => amount * 0.15m;
    }

    public sealed class StudentDiscount : IDiscountStrategy
    {
        public decimal GetDiscount(decimal amount) => amount * 0.08m;
    }

    public sealed class SeniorDiscount : IDiscountStrategy
    {
        public decimal GetDiscount(decimal amount) => amount * 0.07m;
    }

    public sealed class NoDiscount : IDiscountStrategy
    {
        public decimal GetDiscount(decimal amount) => 0m;
    }

    // Фабрика – чтобы не плодить if/else во внешнем коде
    public static class DiscountStrategyFactory
    {
        public static IDiscountStrategy Create(string customerType) =>
            customerType switch
            {
                "Regular" => new RegularDiscount(),
                "Premium" => new PremiumDiscount(),
                "VIP" => new VipDiscount(),
                "Student" => new StudentDiscount(),
                "Senior" => new SeniorDiscount(),
                _ => new NoDiscount()
            };
    }

    /* 2. Ответственность № 2 – считать стоимость доставки */
    public interface IShippingCalculator
    {
        decimal Calculate(string shippingMethod, decimal weight, string destination);
    }

    public sealed class StandardShippingCalculator : IShippingCalculator
    {
        public decimal Calculate(string method, decimal weight, string dest) =>
            method switch
            {
                "Standard" => 5.00m + weight * 0.5m,
                "Express" => 15.00m + weight * 1.0m,
                "Overnight" => 25.00m + weight * 2.0m,
                "International" => dest switch
                {
                    "USA" => 30.00m,
                    "Europe" => 35.00m,
                    "Asia" => 40.00m,
                    _ => 50.00m
                },
                _ => 0m
            };
    }

    #endregion
}
