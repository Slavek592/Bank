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
                QuestionsLists.IIO.WriteString((money - given).ToString() + " remains");
                given += QuestionsLists.GetMoney();
            }

            if (given > money)
                OutputMoney(given - money);

            return true;
        }

        public static bool OutputMoney(uint money)
        {
            QuestionsLists.IIO.WriteString("Here You have the money:");
            string s = "";
            for (int i = 0; i < money; i++)
            {
                s+="( 1 )";
            }
            QuestionsLists.IIO.WriteString(s);
            
            return true;
        }
    }
}