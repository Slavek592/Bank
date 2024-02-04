using System;
using System.Collections.Generic;
using System.IO;

namespace BankEngine.Models
{
    public class Account
    {
        protected string _name;
        protected ushort _number;
        protected uint _money;
        protected List<Transaction> _memory;

        public Account(string name, ushort number, uint money)
        {
            _name = name;
            _number = number;
            _money = 0;
            _memory = new List<Transaction>();
            InitialDeposit(money);
        }

        private Account(ushort number)
        {
            _number = number;
            StreamReader file = new StreamReader(
                DataPath.Path() + number.ToString() + ".txt");
            file.ReadLine();
            _name = file.ReadLine();
            _money = UInt32.Parse(file.ReadLine());
            _memory = new List<Transaction>();
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

        protected Account()
        {
            _money = 0;
            _memory = new List<Transaction>();
        }

        public string GetName()
        {
            return _name;
        }

        public ushort GetNumber()
        {
            return _number;
        }

        protected bool InitialDeposit(uint money)
        {
            return Deposit(money, "Initial deposit");
        }
        public virtual bool Deposit(uint money, string transactionName)
        {
            if (AddMoney(money, transactionName, TransactionType.Deposit))
            {
                MoneySimulator.InputMoney(money);
                return true;
            }
            else
                return false;
        }
        protected bool AddMoney(uint money, string transactionName, TransactionType transactionType)
        {
            _money += money;
            _memory.Add(new Transaction(money, transactionName, transactionType, true));
            return true;
        }

        public virtual bool Withdrawal(uint money, string transactionName)
        {
            if (TakeMoney(money, transactionName, TransactionType.Withdrawal))
            {
                MoneySimulator.OutputMoney(money);
                return true;
            }
            else
                return false;
        }
        
        protected bool TakeMoney(uint money, string transactionName, TransactionType transactionType)
        {
            if (_money >= money)
            {
                _money -= money;
                _memory.Add(new Transaction(money, transactionName, transactionType, false));
                return true;
            }
            else
            {
                Console.WriteLine("Not enough resources.");
                return false;
            }
        }

        public virtual bool Statement()
        {
            Console.WriteLine();
            Console.WriteLine(_name);
            Console.WriteLine("Money: " + _money.ToString());
            Console.WriteLine("Transactions:");
            foreach (Transaction transaction in _memory)
            {
                transaction.Write();
            }
            Console.WriteLine();
            return true;
        }

        public virtual uint TransferTo(uint money, string transactionName)
        {
            AddMoney(money, transactionName, TransactionType.Transfer);
            return 0;
        }
        
        public virtual bool TransferFrom(uint money, string transactionName)
        {
            Console.WriteLine("Sending money...");
            return TakeMoney(money, transactionName, TransactionType.Transfer);
        }

        public virtual bool NewMonth()
        {
            return true;
        }

        public virtual bool Save()
        {
            StreamWriter file = new StreamWriter(
                DataPath.Path() + _number.ToString() + ".txt");
            file.WriteLine("Unprotected");
            file.WriteLine(_name);
            file.WriteLine(_money.ToString());
            foreach (Transaction transaction in _memory)
            {
                file.WriteLine(transaction.Data());
            }
            file.WriteLine("!  end  !");
            file.Close();
            return true;
        }

        public static Account Load(ushort number)
        {
            StreamReader file = new StreamReader(
                DataPath.Path() + number.ToString() + ".txt");
            AccountType type = Enum.Parse<AccountType>(file.ReadLine());
            file.Close();
            if (type == AccountType.Protected)
                return ProtectedAccount.LoadProtected(number);
            else if (type == AccountType.InterestEarning)
                return InterestEarningAccount.LoadInterestEarning(number);
            else if (type == AccountType.GiftCard)
                return GiftCardAccount.LoadGiftCard(number);
            else if (type == AccountType.LineOfCredit)
                return LineOfCreditAccount.LoadLineOfCredit(number);
            else if (type == AccountType.Loan)
                return LoanAccount.LoadLoan(number);
            else
                return Account.LoadUnprotected(number);
        }

        public static Account LoadUnprotected(ushort number)
        {
            return new Account(number);
        }
    }
}