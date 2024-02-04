using System;
using System.IO;

namespace BankEngine.Models
{
    public class GiftCardAccount : Account
    {
        private readonly uint _monthlyMoney;
        private byte _monthsRemaining;
        public GiftCardAccount(string name, ushort number, uint money, uint monthlyMoney, byte months)
            : base(name, number, money)
        {
            _monthlyMoney = monthlyMoney;
            _monthsRemaining = months;
            MoneySimulator.InputMoney((monthlyMoney + 5) * months + money);
            AddMoney(money, "Initial deposit", TransactionType.Deposit);
        }
        
        private GiftCardAccount(ushort number)
        {
            _number = number;
            StreamReader file = new StreamReader(
                DataPath.Path() + number.ToString() + ".txt");
            file.ReadLine();
            _name = file.ReadLine();
            _money = UInt32.Parse(file.ReadLine());
            _monthsRemaining = Byte.Parse(file.ReadLine());
            _monthlyMoney = UInt32.Parse(file.ReadLine());
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
            Console.WriteLine("Impossible. It is a gift card account.");
            return false;
        }

        public override uint TransferTo(uint money, string transactionName)
        {
            Console.WriteLine("Impossible. It is a gift card account.");
            return money;
        }

        public override bool NewMonth()
        {
            if (_monthsRemaining > 0)
            {
                _monthsRemaining -= 1;
                AddMoney(_monthlyMoney, "Prepaid deposit", TransactionType.Deposit);
            }
            /*else
                return false;*/
            return true;
        }
        
        public override bool Save()
        {
            StreamWriter file = new StreamWriter(
                DataPath.Path() + _number.ToString() + ".txt");
            file.WriteLine("GiftCard");
            file.WriteLine(_name);
            file.WriteLine(_money.ToString());
            file.WriteLine(_monthsRemaining.ToString());
            file.WriteLine(_monthlyMoney.ToString());
            foreach (Transaction transaction in _memory)
            {
                file.WriteLine(transaction.Data());
            }
            file.WriteLine("!  end  !");
            file.Close();
            return true;
        }
        
        public static GiftCardAccount LoadGiftCard(ushort number)
        {
            return new GiftCardAccount(number);
        }
    }
}