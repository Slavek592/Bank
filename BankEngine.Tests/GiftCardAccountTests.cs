namespace BankEngine.Tests;

public class GiftCardAccountTests
{
    [Fact]
    public void LastMonth()
    {
        GiftCardAccount giftCardAccount = new GiftCardAccount(
            "test", 0, 0, 100, 0);
        
        giftCardAccount.NewMonth();
        giftCardAccount.NewMonth();
        // Does not throw exceptions when the time is over.
        
        Assert.False(giftCardAccount.Withdrawal(1, "test"));
        // No money, sad...
    }
}