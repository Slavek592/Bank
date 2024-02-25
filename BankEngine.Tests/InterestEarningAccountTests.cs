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
}