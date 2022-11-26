using System;
using System.IO;

namespace SP031122.Models
{
    public class InterestEarningAccount : ProtectedAccount
    {
        public InterestEarningAccount(string name, ushort number, uint money)
        {
            _name = name;
            _number = number;
            Console.WriteLine("Create a new password.");
            _password = Console.ReadLine();
            InitialDeposit(money);
        }

        private InterestEarningAccount(ushort number)
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

        public override bool NewMonth()
        {
            AddMoney((uint) (_money / 50), "Monthly interest", TransactionType.MonthlyInterest);
            return TakeMoney(5, "Monthly payment", TransactionType.Payment);
        }
        
        public override bool Save()
        {
            StreamWriter file = new StreamWriter(
                "/home/slava/RiderProjects/SemPrg2/SP-3-11-22/Data/"
                + _number.ToString() + ".txt");
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