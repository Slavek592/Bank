using System;

namespace BankEngine.Models
{
    public static class MoneySimulator
    {
        public static bool InputMoney(uint money)
        {
            uint given = 0;
            while (given < money)
            {
                Console.WriteLine((money - given).ToString() + " remains");
                given += QuestionsLists.GetMoney();
            }

            if (given > money)
                OutputMoney(given - money);

            return true;
        }

        public static bool OutputMoney(uint money)
        {
            Console.WriteLine("Here You have the money:");
            for (int i = 0; i < money; i++)
            {
                Console.Write("( 1 )");
            }
            Console.WriteLine();
            
            return true;
        }
    }
}