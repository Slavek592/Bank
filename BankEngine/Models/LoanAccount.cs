using System;
using System.IO;

namespace BankEngine.Models
{
    public class LoanAccount : LineOfCreditAccount
    {
        public LoanAccount(string name, ushort number, uint money , uint limit)
            : base(name, number, money, limit)
        {}
        
        private LoanAccount(ushort number)
        {
            _number = number;
            StreamReader file = new StreamReader(
                DataPath.Path() + number.ToString() + ".txt");
            file.ReadLine();
            _name = file.ReadLine();
            _money = UInt32.Parse(file.ReadLine());
            _debt = UInt32.Parse(file.ReadLine());
            _limit = UInt32.Parse(file.ReadLine());
            _password = file.ReadLine();
            while (true)
            {
                string data = file.ReadLine();
                if (data == "!  end  !")
                    break;
                else
                    _memory.Add(Transaction.Load(data));
            }
            file.Close();
        }

        public override bool Deposit(uint money, string transactionName)
        {
            if (money <= _debt)
                return base.Deposit(money, transactionName);
            else
                return base.Deposit(_debt, transactionName);
        }

        public override bool Withdrawal(uint money, string transactionName)
        {
            if (_debt + money <= _limit)
                return base.Withdrawal(money, transactionName);
            else
                return base.Withdrawal(_limit - _debt, transactionName);
        }

        public override uint TransferTo(uint money, string transactionName)
        {
            if (money <= _debt)
                return base.TransferTo(money, transactionName);
            else
            {
                money -= _debt;
                base.TransferTo(_debt, transactionName);
                return money;
            }
        }

        public override bool TransferFrom(uint money, string transactionName)
        {
            if (_debt + money <= _limit)
                return base.TransferFrom(money, transactionName);
            else
                return base.TransferFrom(_limit - _debt, transactionName);
        }

        public override bool NewMonth()
        {
            if (_debt > 0)
            {
                IncreaseDebt((uint) (_debt / 100), "Debt monthly interest",
                    TransactionType.MonthlyInterest);
                IncreaseDebt(5, "Monthly payment", TransactionType.Payment);
            }
            else if (_money >= 5)
                TakeMoney(5, "Monthly payment", TransactionType.Payment);
            else
            {
                IncreaseDebt(5 - _money, "Monthly payment", TransactionType.Payment);
                TakeMoney(_money, "Monthly payment", TransactionType.Payment);
            }
            return true;
        }

        public override bool Save()
        {
            StreamWriter file = new StreamWriter(
                DataPath.Path() + _number.ToString() + ".txt");
            file.WriteLine("Loan");
            file.WriteLine(_name);
            file.WriteLine(_money.ToString());
            file.WriteLine(_debt.ToString());
            file.WriteLine(_limit.ToString());
            file.WriteLine(_password);
            foreach (Transaction transaction in _memory)
            {
                file.WriteLine(transaction.Data());
            }
            file.WriteLine("!  end  !");
            file.Close();
            return true;
        }
        
        public static LoanAccount LoadLoan(ushort number)
        {
            return new LoanAccount(number);
        }
    }
}