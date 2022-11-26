using System;
using System.IO;

namespace SP031122.Models
{
    public class ProtectedAccount : Account
    {
        protected string _password;
        
        public ProtectedAccount(string name, ushort number, uint money)
        {
            _name = name;
            _number = number;
            InitialDeposit(money);
            Console.WriteLine("Create a new password.");
            _password = Console.ReadLine();
        }

        private ProtectedAccount(ushort number)
        {
            _number = number;
            StreamReader file = new StreamReader(
                "/home/slava/RiderProjects/SemPrg2/SP-3-11-22/Data/"
                + number.ToString() + ".txt");
            file.ReadLine();
            _name = file.ReadLine();
            _money = UInt32.Parse(file.ReadLine());
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

        protected ProtectedAccount()
        { }

        public override bool Withdrawal(uint money, string transactionName)
        {
            Console.WriteLine("Write Your password.");
            return Withdrawal(money, transactionName, Console.ReadLine());
        }

        public bool Withdrawal(uint money, string transactionName, string password)
        {
            if (_password == password)
            {
                if (TakeMoney(money, transactionName, TransactionType.Withdrawal))
                {
                    MoneySimulator.OutputMoney(money);
                    return true;
                }
                else
                    return false;
            }
            else
            {
                Console.WriteLine("Wrong password.");
                return false;
            }
        }

        public override bool TransferFrom(uint money, string transactionName)
        {
            Console.WriteLine("Write Your password.");
            return TransferFrom(money, transactionName, Console.ReadLine());
        }

        public bool TransferFrom(uint money, string transactionName, string password)
        {
            if (_password == password)
            {
                Console.WriteLine("Sending money...");
                return TakeMoney(money, transactionName, TransactionType.Transfer);
            }
            else
            {
                Console.WriteLine("Wrong password.");
                return false;
            }
        }

        public override bool Statement()
        {
            Console.WriteLine("Write Your password.");
            return Statement(Console.ReadLine());
        }

        public virtual bool Statement(string password)
        {
            if (_password == password)
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
            else
            {
                Console.WriteLine("Wrong password.");
                return false;
            }
        }

        public bool ChangePassword()
        {
            Console.WriteLine("Write Your password.");
            if (_password == Console.ReadLine())
            {
                Console.WriteLine("Write a new password.");
                _password = Console.ReadLine();
                Console.WriteLine("Password changed.");
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
            return TakeMoney(5, "Monthly payment", TransactionType.Payment);
        }

        public override bool Save()
        {
            StreamWriter file = new StreamWriter(
                "/home/slava/RiderProjects/SemPrg2/SP-3-11-22/Data/"
                + _number.ToString() + ".txt");
            file.WriteLine("Protected");
            file.WriteLine(_name);
            file.WriteLine(_money.ToString());
            file.WriteLine(_password);
            foreach (Transaction transaction in _memory)
            {
                file.WriteLine(transaction.Data());
            }
            file.WriteLine("!  end  !");
            file.Close();
            return true;
        }

        public static ProtectedAccount LoadProtected(ushort number)
        {
            return new ProtectedAccount(number);
        }
    }
}