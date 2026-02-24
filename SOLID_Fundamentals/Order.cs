namespace SOLID_Fundamentals
{
    //OCP
    public interface IPaymentMethod
    {
        void Pay(decimal amount);
        bool IsApplicable(string methodToken);
    }

    // 2. Конкретные стратегии
    public sealed class CreditCardPayment : IPaymentMethod
    {
        public void Pay(decimal amount) => Console.WriteLine("вызов банковского шлюза");
        public bool IsApplicable(string methodToken) => methodToken == "CreditCard";
    }

    public sealed class PayPalPayment : IPaymentMethod
    {
        public void Pay(decimal amount) => Console.WriteLine("вызов PayPal SDK");
        public bool IsApplicable(string methodToken) => methodToken == "PayPal";
    }

    // 3. Расширяем без правки Order
    public sealed class CryptoPayment : IPaymentMethod
    {
        public void Pay(decimal amount) => Console.WriteLine("вызов крипто-шлюза");
        public bool IsApplicable(string methodToken) => methodToken == "Crypto";
    }

    // 4. «Закрытый» Order
    public sealed class Order
    {
        public int Id { get; init; }
        public decimal TotalAmount { get; init; }
        public IReadOnlyList<string> Items { get; init; } = new List<string>();
        public string CustomerEmail { get; init; } = string.Empty;
        public string CustomerPhone { get; init; } = string.Empty;

        // Вместо строки теперь ссылка на стратегию
        public IPaymentMethod Payment { get; init; } = null!;

        public void Checkout() => Payment.Pay(TotalAmount);
    }

    // 5. Фабрика/реестр, чтобы клиентский код не знал о конкретных классах
    public static class PaymentFactory
    {
        private static readonly IEnumerable<IPaymentMethod> _methods =
        [
            new CreditCardPayment(),
            new PayPalPayment(),
            new CryptoPayment()
        ];

        public static IPaymentMethod Create(string methodToken) =>
            _methods.FirstOrDefault(m => m.IsApplicable(methodToken))
            ?? throw new NotSupportedException($"Payment {methodToken} is not supported.");
    }
}