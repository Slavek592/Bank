using System;
using System.Collections.Generic;

namespace SP031122.Models
{
    public class Account
    {
        private readonly string _name;
        private readonly ushort _number;
        private uint _money;
        private List<Transaction> _memory;

        public Account(string name, ushort number, uint money)
        {
            _name = name;
            _number = number;
            _money = 0;
            _memory = new List<Transaction>();
            Deposit(money, "Initial deposit");
        }

        public string GetName()
        {
            return _name;
        }

        public ushort GetNumber()
        {
            return _number;
        }
        
        public bool Deposit(uint money, string transactionName)
        {
            return AddMoney(money, transactionName, TransactionType.Deposit);
        }
        public bool AddMoney(uint money, string transactionName, TransactionType transactionType)
        {
            _money += money;
            _memory.Add(new Transaction(money, transactionName, transactionType));
            return true;
        }

        public bool Withdrawal(uint money, string transactionName)
        {
            return TakeMoney(money, transactionName, TransactionType.Withdrawal);
        }
        
        public bool TakeMoney(uint money, string transactionName, TransactionType transactionType)
        {
            if (_money >= money)
            {
                _money -= money;
                _memory.Add(new Transaction(money, transactionName, transactionType));
                return true;
            }
            else
                return false;
        }

        public bool Statement()
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

        public bool TransferTo(uint money, string transactionName)
        {
            return AddMoney(money, transactionName, TransactionType.TransferTo);
        }
        
        public bool TransferFrom(uint money, string transactionName)
        {
            return TakeMoney(money, transactionName, TransactionType.TransferFrom);
        }
    }
}