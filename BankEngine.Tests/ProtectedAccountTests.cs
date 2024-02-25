using Moq;

namespace BankEngine.Tests;

public class ProtectedAccountTests
{
    [Fact]
    public void CostsSomething()
    {
        InterestEarningAccount account =
            new InterestEarningAccount("test", 1, 0, "");
        account.TransferTo(1, "Here you have.");

        account.NewMonth();

        Assert.False(account.TransferFrom(1, "test", ""));
    }

    [Fact]
    public void NeedsCorrectPasswordToSendMoney()
    {
        var mock = new Mock<IIOProvider> ();
        mock.Setup(w => w.GetString()).Returns("AwesomePassword");
        QuestionsLists.IIO = mock.Object;
        ProtectedAccount account = new ProtectedAccount("Account", 0, 0);
        account.TransferTo(200, "Here you have the money");

        // Works with the correct password
        bool worksWell = account.Withdrawal(5, "Give me the money")
            && account.TransferFrom(10, "Send me the money");
        
        mock.Setup(w => w.GetString()).Returns("WrongPassword");
        QuestionsLists.IIO = mock.Object;
        
        // Does not work with a wrong password
        worksWell = worksWell && !account.Withdrawal(5, "Do not give the money") 
            && !account.TransferFrom(10, "Do not send the money");
        
        Assert.True(worksWell);
    }
}