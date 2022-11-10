using System;

namespace SP031122.Models
{
    public class Transaction
    {
        private readonly uint _money;
        private readonly TransactionType _type;
        private readonly string _name;
        private readonly DateTime _time;

        public Transaction(uint money, string name, TransactionType type)
        {
            _money = money;
            _name = name;
            _type = type;
            _time = DateTime.Now;
        }

        public bool Write()
        {
            Console.WriteLine(_name + " (" + _type.ToString() + "): " + _money.ToString()
                              + " (" + _time.ToString() + ")");
            return true;
        }
    }
}