namespace SOLID_Fundamentals
{
    // LSP
    #region Common abstraction
    public abstract class Account
    {
        public decimal Balance { get; protected set; }

        public virtual void Deposit(decimal amount) => Balance += amount;

        /// <returns>true – снятие разрешено и выполнено; false – иначе.</returns>
        public abstract bool TryWithdraw(decimal amount);

        public virtual decimal CalculateInterest() => Balance * 0.01m;
    }
    #endregion

    #region Конкретные счета
    public class SavingsAccount : Account
    {
        public decimal MinimumBalance { get; } = 100m;

        public override bool TryWithdraw(decimal amount)
        {
            if (Balance - amount < MinimumBalance) return false;
            Balance -= amount;
            return true;
        }
    }

    public class CheckingAccount : Account
    {
        public decimal OverdraftLimit { get; } = 500m;

        public override bool TryWithdraw(decimal amount)
        {
            if (Balance - amount < -OverdraftLimit) return false;
            Balance -= amount;
            return true;
        }
    }

    public class FixedDepositAccount : Account
    {
        public DateTime MaturityDate { get; }

        public FixedDepositAccount(DateTime maturityDate) => MaturityDate = maturityDate;

        public override bool TryWithdraw(decimal amount)
        {
            if (DateTime.Now < MaturityDate) return false;
            if (amount > Balance) return false;

            Balance -= amount;
            return true;
        }

        public override decimal CalculateInterest() => Balance * 0.05m;
    }
    #endregion

    #region Сервисный класс
    public class Bank
    {
        public bool ProcessWithdrawal(Account account, decimal amount)
        {
            bool ok = account.TryWithdraw(amount);
            Console.WriteLine(ok
                ? $"Successfully withdrew {amount}"
                : $"Withdrawal failed (amount={amount})");
            return ok;
        }

        public bool Transfer(Account from, Account to, decimal amount)
        {
            if (!from.TryWithdraw(amount)) return false; // откат не нужен – деньги не сняты
            to.Deposit(amount);
            return true;
        }
    }
    #endregion
}
