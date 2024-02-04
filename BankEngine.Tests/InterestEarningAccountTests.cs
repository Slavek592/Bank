namespace BankEngine.Tests;

public class InterestEarningAccountTests
{
    // Works for any interest bigger than 0,001 % and payments smaller then 100 per month.
    [Fact]
    public void EarnsInterest()
    {
        InterestEarningAccount richAccount =
            new InterestEarningAccount("test", 1, 0, "");
        richAccount.TransferTo(1000000000, "money came");

        richAccount.NewMonth();

        Assert.True(richAccount.TransferFrom(1000000001, "test", ""));
    }
    
    [Fact]
    public void CostsSomething()
    {
        InterestEarningAccount account =
            new InterestEarningAccount("test", 1, 0, "");
        account.TransferTo(1, "Here you have.");

        account.NewMonth();

        Assert.False(account.TransferFrom(1, "test", ""));
    }
}