using System;
using System.Collections.Generic;
using System.Linq;

namespace SP031122.Models
{
    public static class QuestionsLists
    {
        public static string GetAnswer(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        public static ushort GetUShort(string question)
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

        public static uint GetUInt(string question)
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
        public static byte GetByte(string question)
        {
            while (true)
            {
                try
                {
                    byte result = Byte.Parse(GetAnswer(question));
                    return result;
                }
                catch
                {
                    Console.WriteLine("It was not a correct answer, try one more time.");
                }
            }
        }

        public static bool YesNo(string question)
        {
            Console.WriteLine(question);
            string answer = Console.ReadLine().ToLower();
            string[] strings = {"y", "yes", "yes.", "t", "true"};
            if (strings.ToList().Contains(answer))
                return true;
            else
                return false;
        }
        
        public static ushort GetMoney()
        {
            int[] moneyArray = {1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 2000, 5000};
            List<int> moneyList = moneyArray.ToList();
            while (true)
            {
                try
                {
                    ushort result = UInt16.Parse(GetAnswer(
                        "Give here the money: 1/2/5/10/20/50/100/200/500/1000/2000/5000"));
                    if (moneyList.Contains(result))
                        return result;
                    else
                        Console.WriteLine("It is fake money.");
                }
                catch
                {
                    Console.WriteLine("It is not money.");
                }
            }
        }

        public static List<string> CreateAnswers()
        {
            string[] answers = {"create", "create account", "create an account", "c"};
            return answers.ToList();
        }
        
        public static List<string> LogInAnswers()
        {
            string[] answers = {"log", "login", "log in", "l"};
            return answers.ToList();
        }
        
        public static List<string> ReadAnswers()
        {
            string[] answers = {"read", "read statement",
            "read the statement", "read the statement of an account", "r"};
            return answers.ToList();
        }

        public static List<string> ExitAnswers()
        {
            string[] answers = {"exit", "e"};
            return answers.ToList();
        }

        public static List<string> DepositAnswers()
        {
            string[] answers = {"deposit", "d"};
            return answers.ToList();
        }

        public static List<string> WithdrawalAnswers()
        {
            string[] answers = {"withdrawal", "w"};
            return answers.ToList();
        }

        public static List<string> TransferAnswers()
        {
            string[] answers = {"transfer", "t"};
            return answers.ToList();
        }
    }
}