using System;
using System.Collections.Generic;
using System.Linq;

namespace SP031122.Models
{
    public class Bank
    {
        private List<Account> _accounts;

        public Bank()
        {
            _accounts = new List<Account>();
        }

        public bool AccountExists(ushort number)
        {
            if (_accounts.Any(account => account.GetNumber() == number))
                return true;
            else
                return false;
        }

        public Account FindAccount(ushort number)
        {
            if (AccountExists(number))
                return _accounts.First(account => account.GetNumber() == number);
            else
                return null;
        }

        public bool CreateAccount(string name, uint money)
        {
            Random random = new Random();
            ushort number;
            while (true)
            {
                number = (ushort) random.Next(0, 65535);
                if (!AccountExists(number))
                {
                    break;
                }
            }
            Console.WriteLine("Your number is: " + number.ToString());
            _accounts.Add(new Account(name, number, money));
            return true;
        }

        public bool Deposit(ushort number, uint money, string transactionName = "Deposit")
        {
            Account account = FindAccount(number);
            if (account != null)
                return account.Deposit(money, transactionName);
            else
                return false;
        }
        
        public bool Deposit(Account account, uint money, string transactionName = "Deposit")
        {
            return account.Deposit(money, transactionName);
        }

        public bool Statement(Account account)
        {
            return account.Statement();
        }

        public bool Statement(ushort number)
        {
            Account account = FindAccount(number);
            if (account != null)
                return account.Statement();
            else
                return false;
        }

        public bool Withdrawal(Account account, uint money, string transactionName = "Withdrawal")
        {
            return account.Withdrawal(money, transactionName);
        }
        
        public bool Withdrawal(ushort number, uint money, string transactionName = "Withdrawal")
        {
            Account account = FindAccount(number);
            if (account != null)
                return account.Withdrawal(money, transactionName);
            else
                return false;
        }

        public bool Transfer(Account accountFrom, Account accountTo, uint money,
            string transactionName = "Transfer")
        {
            if (accountFrom.TransferFrom(money, transactionName))
            {
                if (accountTo.TransferTo(money, transactionName))
                    return true;
                else
                {
                    accountFrom.Deposit(money, transactionName + " - cancel");
                    return false;
                }
            }
            else
                return false;
        }

        public bool Transfer(ushort numberFrom, ushort numberTo, uint money,
            string transactionName = "Transfer")
        {
            Account accountFrom = FindAccount(numberFrom);
            Account accountTo = FindAccount(numberTo);
            if ((accountFrom != null) && (accountTo != null))
            {
                if (accountFrom.TransferFrom(money, transactionName))
                {
                    if (accountTo.TransferTo(money, transactionName))
                        return true;
                    else
                    {
                        accountFrom.Deposit(money, transactionName + " - cancel");
                        return false;
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }

        /*private uint RandomUint()
        {
            Random random = new Random();
            return (uint) (random.Next(Int32.MaxValue) << 16) | (uint) random.Next(Int32.MaxValue);
        }*/
    }
}