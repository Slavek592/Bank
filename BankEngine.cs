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
      DateTime lastActualization = bank.Load();

      List<string> createAnswers = QuestionsLists.CreateAnswers();
      List<string> logInAnswers = QuestionsLists.LogInAnswers();
      List<string> readAnswers = QuestionsLists.ReadAnswers();
      List<string> exitAnswers = QuestionsLists.ExitAnswers();
      List<string> depositAnswers = QuestionsLists.DepositAnswers();
      List<string> withdrawalAnswers = QuestionsLists.WithdrawalAnswers();
      List<string> transferAnswers = QuestionsLists.TransferAnswers();
      
      while (true)
      {
        if (lastActualization.Month != DateTime.Now.Month)
        {
          bank.NewMonth();
          lastActualization = DateTime.Now;
        }
        Console.WriteLine("You can create an account, log in and exit.");
        string answer = QuestionsLists.GetAnswer("What do You wish?").ToLower();
        if (createAnswers.Contains(answer))
        {
          if (QuestionsLists.YesNo("Do You wish an interest earning account?"))
          {
            bank.CreateAccount(QuestionsLists.GetAnswer("What is the name of the new account?"),
              QuestionsLists.GetUInt("How much money do You want to put on the account?"),
              AccountType.InterestEarning);
          }
          else if (QuestionsLists.YesNo("Do You wish a gift card account?"))
          {
            bank.CreateAccount(QuestionsLists.GetAnswer("What is the name of the new account?"),
              QuestionsLists.GetUInt("How much money do You want to put on the account?"),
              AccountType.GiftCard);
          }
          else if (QuestionsLists.YesNo("Do You wish a line of credit account?"))
          {
            bank.CreateAccount(QuestionsLists.GetAnswer("What is the name of the new account?"),
              QuestionsLists.GetUInt("How much money do You want to put on the account?"),
              AccountType.LineOfCredit);
          }
          else if (QuestionsLists.YesNo("Do You wish an unprotected account?"))
          {
            bank.CreateAccount(QuestionsLists.GetAnswer("What is the name of the new account?"),
              QuestionsLists.GetUInt("How much money do You want to put on the account?"),
              AccountType.Unprotected);
          }
          else
          {
            bank.CreateAccount(QuestionsLists.GetAnswer("What is the name of the new account?"),
              QuestionsLists.GetUInt("How much money do You want to put on the account?"),
              AccountType.Protected);
          }
          Console.WriteLine("Account created.");
        }
        else if (logInAnswers.Contains(answer))
        {
          ushort number = QuestionsLists.GetUShort("What is Your number?");
          if (bank.AccountExists(number))
          {
            while (true)
            {
              //There is no password at this moment of time.
              Console.WriteLine("You can make a deposit, withdrawal and transfer," +
                                " read the statement and exit.");
              string answerInner = QuestionsLists.GetAnswer("What do You wish?").ToLower();
              if (depositAnswers.Contains(answerInner))
              {
                if (QuestionsLists.YesNo("Do You wish a name for the transaction?"))
                  bank.Deposit(number, QuestionsLists.GetUInt(
                      "How much money do You want to put on the account?"),
                    QuestionsLists.GetAnswer("What is the name of the transaction?"));
                else
                  bank.Deposit(number, QuestionsLists.GetUInt(
                    "How much money do You want to put on the account?"));
              }
              else if (withdrawalAnswers.Contains(answerInner))
              {
                if (QuestionsLists.YesNo("Do You wish a name for the transaction?"))
                  bank.Withdrawal(number, QuestionsLists.GetUInt(
                      "How much money do You want to take from the account?"),
                    QuestionsLists.GetAnswer("What is the name of the transaction?"));
                else
                  bank.Withdrawal(number, QuestionsLists.GetUInt(
                    "How much money do You want to take from the account?"));
              }
              else if (transferAnswers.Contains(answerInner))
              {
                if (QuestionsLists.YesNo("Do You wish a name for the transaction?"))
                  bank.Transfer(number, QuestionsLists.GetUShort(
                      "Who do You want to send the money to?"), QuestionsLists.GetUInt(
                      "How much money do You want to send?"),
                    QuestionsLists.GetAnswer("What is the name of the transaction?"));
                else
                  bank.Transfer(number, QuestionsLists.GetUShort(
                    "Who do You want to send the money to?"), QuestionsLists.GetUInt(
                    "How much money do You want to send?"));
              }
              else if (readAnswers.Contains(answerInner))
              {
                bank.Statement(number);
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
          else
            Console.WriteLine("There is no account with this number.");
        }
        else if (exitAnswers.Contains(answer))
        {
          bank.Save(lastActualization);
          break;
        }
        else
        {
          Console.WriteLine("It was not understandable, try one more time.");
        }
      }
    }
  }
}