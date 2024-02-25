using System;
using System.IO;

namespace BankEngine.Models
{
    public class ProtectedAccount : Account
    {
        protected string _password;
        
        public ProtectedAccount(string name, ushort number, uint money)
            : base(name, number, money)
        {
            QuestionsLists.IIO.WriteString("Create a new password.");
            _password = QuestionsLists.IIO.GetString();
        }

        public ProtectedAccount(string name, ushort number, uint money, string password)
            : base(name, number, money)
        {
            _password = password;
        }

        private ProtectedAccount(ushort number)
        {
            _number = number;
            StreamReader file = new StreamReader(
                DataPath.Path() + number.ToString() + ".txt");
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
            QuestionsLists.IIO.WriteString("Write Your password.");
            return Withdrawal(money, transactionName, QuestionsLists.IIO.GetString());
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
                QuestionsLists.IIO.WriteString("Wrong password.");
                return false;
            }
        }

        public override bool TransferFrom(uint money, string transactionName)
        {
            QuestionsLists.IIO.WriteString("Write Your password.");
            return TransferFrom(money, transactionName, QuestionsLists.IIO.GetString());
        }

        public bool TransferFrom(uint money, string transactionName, string password)
        {
            if (_password == password)
            {
                QuestionsLists.IIO.WriteString("Sending money...");
                return TakeMoney(money, transactionName, TransactionType.Transfer);
            }
            else
            {
                QuestionsLists.IIO.WriteString("Wrong password.");
                return false;
            }
        }

        public override bool Statement()
        {
            QuestionsLists.IIO.WriteString("Write Your password.");
            return Statement(QuestionsLists.IIO.GetString());
        }

        public virtual bool Statement(string password)
        {
            if (_password == password)
            {
                QuestionsLists.IIO.WriteString("");
                QuestionsLists.IIO.WriteString(_name);
                QuestionsLists.IIO.WriteString("Money: " + _money.ToString());
                QuestionsLists.IIO.WriteString("Transactions:");
                foreach (Transaction transaction in _memory)
                {
                    transaction.Write();
                }
                QuestionsLists.IIO.WriteString("");
                return true;
            }
            else
            {
                QuestionsLists.IIO.WriteString("Wrong password.");
                return false;
            }
        }

        public bool ChangePassword()
        {
            QuestionsLists.IIO.WriteString("Write Your password.");
            if (_password == QuestionsLists.IIO.GetString())
            {
                QuestionsLists.IIO.WriteString("Write a new password.");
                _password = QuestionsLists.IIO.GetString();
                QuestionsLists.IIO.WriteString("Password changed.");
                return true;
            }
            else
            {
                QuestionsLists.IIO.WriteString("Wrong password.");
                return false;
            }
        }

        public override bool NewMonth()
        {
            if (_money >= 5)
                return TakeMoney(5, "Monthly payment", TransactionType.Payment);
            else
                return TakeMoney(_money, "Not enough resources", TransactionType.Payment);
        }

        public override bool Save()
        {
            StreamWriter file = new StreamWriter(
                DataPath.Path() + _number.ToString() + ".txt");
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