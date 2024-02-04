using System;

namespace BankEngine.Models
{
    public class Transaction
    {
        private readonly uint _money;
        private readonly TransactionType _type;
        private readonly string _name;
        private readonly DateTime _time;
        private readonly bool _positive;

        public Transaction(uint money, string name, TransactionType type, bool positive)
        {
            _money = money;
            _name = name;
            _type = type;
            _time = DateTime.Now;
            _positive = positive;
        }
        
        private Transaction(uint money, string name, TransactionType type,
            bool positive, DateTime time)
        {
            _money = money;
            _name = name;
            _type = type;
            _time = time;
            _positive = positive;
        }

        public bool Write()
        {
            string s = _name + " (" + _type.ToString() + "): ";
            if (_positive)
                s += "+";
            else
                s += "-";
            s += _money.ToString() + " (" + _time.ToString() + ")";
            Console.WriteLine(s);
            return true;
        }

        public string Data()
        {
            return _name.Replace(" ", "_") + " " + _type.ToString() + " "
                   + _positive.ToString() + " " + _money.ToString() + " "
                   + _time.ToString().Replace(" ", "_");
        }

        public static Transaction Load(string data)
        {
            string[] dataSplit = data.Split();
            return new Transaction(UInt32.Parse(dataSplit[3]), dataSplit[0].Replace("_", " "),
                Enum.Parse<TransactionType>(dataSplit[1]), Boolean.Parse(dataSplit[2]),
                DateTime.Parse(dataSplit[4].Replace("_", " ")));
        }
    }
}