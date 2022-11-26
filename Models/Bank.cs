using System;
using System.Collections.Generic;
using System.IO;
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
            return _accounts.Any(account => account.GetNumber() == number);
        }

        private Account FindAccount(ushort number)
        {
            if (AccountExists(number))
                return _accounts.First(account => account.GetNumber() == number);
            else
                return null;
        }

        public bool CreateAccount(string name, uint money, AccountType type=AccountType.Unprotected)
        {
            ushort number;
            do
            {
                number = (ushort) Random.Shared.Next(0, 65535);
            }
            while (AccountExists(number));

            Console.WriteLine("Your number is: " + number.ToString());
            if (type == AccountType.Protected)
                _accounts.Add(new ProtectedAccount(name, number, money));
            else if (type == AccountType.InterestEarning)
                _accounts.Add(new InterestEarningAccount(name, number, money));
            else if (type == AccountType.GiftCard)
                _accounts.Add(new GiftCardAccount(name, number, money,
                    QuestionsLists.GetUInt("How much money would You like" +
                                           " to put there every month?"),
                    QuestionsLists.GetByte("For how many more months (except the first one)" +
                                           " would You like to pay the gift card?")));
            else if (type == AccountType.LineOfCredit)
                _accounts.Add(new LineOfCreditAccount(name, number, money));
            else
                _accounts.Add(new Account(name, number, money));
            return true;
        }

        public bool Deposit(ushort number, uint money, string transactionName = "Deposit")
        {
            Account account = FindAccount(number);
            if (account != null)
                return Deposit(account, money, transactionName);
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
                return Statement(account);
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
                return Withdrawal(account, money, transactionName);
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
                return Transfer(accountFrom, accountTo, money, transactionName);
            else
                return false;
        }

        public bool NewMonth()
        {
            Console.WriteLine("Please, wait, accounts updating.");
            List<Account> accountsToDelete = new List<Account>();
            foreach (Account account in _accounts)
            {
                if (!account.NewMonth())
                    accountsToDelete.Add(account);
            }
            foreach (Account account in accountsToDelete)
            {
                _accounts.Remove(account);
            }
            return true;
        }

        public bool Save(DateTime time)
        {
            StreamWriter file = new StreamWriter(
                "/home/slava/RiderProjects/SemPrg2/SP-3-11-22/Data/Accounts.txt");
            file.WriteLine(time.ToString());
            foreach (Account account in _accounts)
            {
                file.WriteLine(account.GetNumber().ToString());
                account.Save();
            }
            file.WriteLine("!  end  !");
            file.Close();
            return true;
        }

        public DateTime Load()
        {
            StreamReader file = new StreamReader(
                "/home/slava/RiderProjects/SemPrg2/SP-3-11-22/Data/Accounts.txt");
            DateTime time = DateTime.Parse(file.ReadLine().Replace("_", " "));
            while (true)
            {
                string data = file.ReadLine();
                if (data == "!  end  !")
                    break;
                else
                    _accounts.Add(Account.Load(UInt16.Parse(data)));
            }
            return time;
        }
    }
}