using System;
using System.IO;

namespace BankEngine.Models
{
    public class LineOfCreditAccount : ProtectedAccount
    {
        protected uint _limit;
        protected uint _debt;
        
        public LineOfCreditAccount(string name, ushort number, uint money, uint limit=5000)
            : base(name, number, money)
        {
            _limit = limit;
            //_limit = 5000;
            _debt = 0;
        }
        
        private LineOfCreditAccount(ushort number)
        {
            _number = number;
            StreamReader file = new StreamReader(
                DataPath.Path() + number.ToString() + ".txt");
            file.ReadLine();
            _name = file.ReadLine();
            _money = UInt32.Parse(file.ReadLine());
            _debt = UInt32.Parse(file.ReadLine());
            _limit = 5000;
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
        
        protected LineOfCreditAccount() {}

        public override bool Withdrawal(uint money, string transactionName)
        {
            Console.WriteLine("Write Your password.");
            if (money <= _money)
                return Withdrawal(money, transactionName, Console.ReadLine());
            money -= _money;
            if (Withdrawal(_money, transactionName, Console.ReadLine()))
            {
                MoneySimulator.OutputMoney(money);
                return IncreaseDebt(money, transactionName, TransactionType.Withdrawal);
            }
            return false;
        }

        public override bool TransferFrom(uint money, string transactionName)
        {
            Console.WriteLine("Write Your password.");
            if (money <= _money)
                return TransferFrom(money, transactionName, Console.ReadLine());
            money -= _money;
            if (TransferFrom(_money, transactionName, Console.ReadLine()))
            {
                return IncreaseDebt(money, transactionName, TransactionType.Transfer);
            }
            return false;
        }

        public override bool Deposit(uint money, string transactionName)
        {
            MoneySimulator.InputMoney(money);
            if (_debt == 0)
                return AddMoney(money, transactionName, TransactionType.Deposit);
            if (money <= _debt)
            {
                return ShrinkDebt(money, transactionName, TransactionType.Deposit);
            }
            money -= _debt;
            ShrinkDebt(_debt, transactionName, TransactionType.Deposit);
            return AddMoney(money, transactionName, TransactionType.Deposit);
        }

        public override uint TransferTo(uint money, string transactionName)
        {
            if (_debt == 0)
            {
                AddMoney(money, transactionName, TransactionType.Transfer);
                return 0;
            }
            if (money <= _debt)
            {
                ShrinkDebt(money, transactionName, TransactionType.Transfer);
                return 0;
            }
            money -= _debt;
            ShrinkDebt(_debt, transactionName, TransactionType.Transfer);
            AddMoney(money, transactionName, TransactionType.Transfer);
            return 0;
        }

        protected bool IncreaseDebt(uint money, string transactionName, TransactionType transactionType)
        {
            _memory.Add(new Transaction(money, transactionName + " (going into debt)",
                transactionType, false));
            _debt += money;
            if (_debt > _limit)
            {
                _debt += 20;
                _memory.Add(new Transaction(20, "Limit exceeded",
                    TransactionType.Payment, false));
            }
            return true;
        }

        protected bool ShrinkDebt(uint money, string transactionName, TransactionType transactionType)
        {
            _debt -= money;
            _memory.Add(new Transaction(money, transactionName + " (paying debt)",
                transactionType, true));
            return true;
        }

        public override bool Statement(string password)
        {
            if (_password == password)
            {
                Console.WriteLine();
                Console.WriteLine(_name);
                if (_debt == 0)
                    Console.WriteLine("Money: " + _money.ToString());
                else
                    Console.WriteLine("Money: -" + _debt.ToString());
                Console.WriteLine("Transactions:");
                foreach (Transaction transaction in _memory)
                {
                    transaction.Write();
                }
                Console.WriteLine();
                return true;
            }
            else
            {
                Console.WriteLine("Wrong password.");
                return false;
            }
        }

        public override bool NewMonth()
        {
            if (_debt > 0)
            {
                IncreaseDebt((uint) (_debt / 40), "Debt monthly interest",
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
            file.WriteLine("LineOfCredit");
            file.WriteLine(_name);
            file.WriteLine(_money.ToString());
            file.WriteLine(_debt.ToString());
            file.WriteLine(_password);
            foreach (Transaction transaction in _memory)
            {
                file.WriteLine(transaction.Data());
            }
            file.WriteLine("!  end  !");
            file.Close();
            return true;
        }

        public static LineOfCreditAccount LoadLineOfCredit(ushort number)
        {
            return new LineOfCreditAccount(number);
        }
    }
}