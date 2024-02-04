using System;
using System.IO;

namespace BankEngine.Models
{
    public class InterestEarningAccount : ProtectedAccount
    {
        public InterestEarningAccount(string name, ushort number, uint money)
            : base(name, number, money)
        {}
        
        public InterestEarningAccount(string name, ushort number, uint money, string password)
            : base(name, number, money, password)
        {}

        private InterestEarningAccount(ushort number)
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

        public override bool NewMonth()
        {
            AddMoney((uint) (_money / 500), "Monthly interest", TransactionType.MonthlyInterest);
            return base.NewMonth();
        }
        
        public override bool Save()
        {
            StreamWriter file = new StreamWriter(
                DataPath.Path() + _number.ToString() + ".txt");
            file.WriteLine("InterestEarning");
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

        public static InterestEarningAccount LoadInterestEarning(ushort number)
        {
            return new InterestEarningAccount(number);
        }
    }
}