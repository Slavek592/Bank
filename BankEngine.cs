using System;
using System.Collections.Generic;
using System.Linq;
using SP031122.Models;

namespace SP031122
{
    public class BankEngine
    {
        public static void Main()
        {
            Bank bank = new Bank();
            
            List<string> createAnswers = CreateAnswers();
            List<string> logInAnswers = LogInAnswers();
            List<string> readAnswers = ReadAnswers();
            List<string> exitAnswers = ExitAnswers();
            List<string> depositAnswers = DepositAnswers();
            List<string> withdrawalAnswers = WithdrawalAnswers();
            List<string> transferAnswers = TransferAnswers();
            
            while (true)
            {
                Console.WriteLine("You can create an account, log in, read the statement of an account and exit.");
                string answer = GetAnswer("What do You wish?").ToLower();
                if (createAnswers.Contains(answer))
                {
                    bank.CreateAccount(GetAnswer("What is the name of the new account?"),
                        GetUInt("How much money do You want to put on the account?"));
                    Console.WriteLine("Account created.");
                }
                else if (logInAnswers.Contains(answer))
                {
                    ushort number = GetUShort("What is Your number?");
                    while (true)
                    {
                        //There is no password at this moment of time.
                        Console.WriteLine("You can make a deposit, withdrawal, transfer and exit.");
                        string answerInner = GetAnswer("What do You wish?").ToLower();
                        if (depositAnswers.Contains(answerInner))
                        {
                            if (YesNo("Do You wish a name for the transaction?"))
                                bank.Deposit(number, GetUInt(
                                    "How much money do You want to put on the account?"), 
                                    GetAnswer("What is the name of the transaction?"));
                            else
                                bank.Deposit(number, GetUInt(
                                    "How much money do You want to put on the account?"));
                        }
                        else if (withdrawalAnswers.Contains(answerInner))
                        {
                            if (YesNo("Do You wish a name for the transaction?"))
                                bank.Withdrawal(number, GetUInt(
                                    "How much money do You want to take from the account?"),
                                    GetAnswer("What is the name of the transaction?"));
                            else
                                bank.Withdrawal(number, GetUInt(
                                    "How much money do You want to take from the account?"));
                        }
                        else if (transferAnswers.Contains(answerInner))
                        {
                            if (YesNo("Do You wish a name for the transaction?"))
                                bank.Transfer(number, GetUShort(
                                    "Who do You want to send the money to?"), GetUInt(
                                    "How much money do You want to send?"),
                                    GetAnswer("What is the name of the transaction?"));
                            else
                                bank.Transfer(number, GetUShort(
                                  "Who do You want to send the money to?"), GetUInt(
                                   "How much money do You want to send?"));
                        }
                        else if (exitAnswers.Contains(answerInner))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("It was not understandable, try one more time.");
                        }
                    }
                }
                else if (readAnswers.Contains(answer))
                {
                    bank.Statement(GetUShort("What is the number of the account?"));
                }
                else if (exitAnswers.Contains(answer))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("It was not understandable, try one more time.");
                }
            }
        }

        private static string GetAnswer(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        private static ushort GetUShort(string question)
        {
            while (true)
            {
                try
                {
                    ushort result = UInt16.Parse(GetAnswer(question));
                    return result;
                }
                catch
                {
                    Console.WriteLine("It was not a correct answer, try one more time.");
                }
            }
        }
        
        private static uint GetUInt(string question)
        {
            while (true)
            {
                try
                {
                    uint result = UInt32.Parse(GetAnswer(question));
                    return result;
                }
                catch
                {
                    Console.WriteLine("It was not a correct answer, try one more time.");
                }
            }
        }

        private static bool YesNo(string question)
        {
            Console.WriteLine(question);
            string answer = Console.ReadLine().ToLower();
            string[] strings = {"y", "yes", "yes.", "t", "true"};
            if (strings.ToList().Contains(answer))
                return true;
            else
                return false;
        }

        private static List<string> CreateAnswers()
        {
            string[] answers = {"create", "create account", "create an account", "c"};
            return answers.ToList();
        }
        private static List<string> LogInAnswers()
        {
            string[] answers = {"log", "login", "log in", "l"};
            return answers.ToList();
        }
        private static List<string> ReadAnswers()
        {
            string[] answers = {"read", "read statement",
                "read the statement", "read the statement of an account", "r"};
            return answers.ToList();
        }
        
        private static List<string> ExitAnswers()
        {
            string[] answers = {"exit", "e"};
            return answers.ToList();
        }
        
        private static List<string> DepositAnswers()
        {
            string[] answers = {"deposit", "d"};
            return answers.ToList();
        }
        
        private static List<string> WithdrawalAnswers()
        {
            string[] answers = {"withdrawal", "w"};
            return answers.ToList();
        }
        
        private static List<string> TransferAnswers()
        {
            string[] answers = {"transfer", "t"};
            return answers.ToList();
        }
    }
}