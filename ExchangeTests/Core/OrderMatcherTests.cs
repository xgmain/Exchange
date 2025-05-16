using Xunit;

namespace Exchange.Core.Tests
{
    public class OrderMatcherTests
    {
        [Fact]
        public void ProcessOrder_MatchesBuyAndSell()
        {
            var engine = new OrderMatcher();

            var sell = new Order("SELLER", "XYZ", -100, 10.0m);
            var buy = new Order("BUYER", "XYZ", 100, 10.0m);

            engine.ProcessOrder(sell); // Add sell order first
            var trades = engine.ProcessOrder(buy).ToList(); // Should match

            Xunit.Assert.Single(trades);
            var trade = trades[0];

            Xunit.Assert.Equal("BUYER", trade.BuyerId);
            Xunit.Assert.Equal("SELLER", trade.SellerId);
            Xunit.Assert.Equal(100, trade.Quantity);
            Xunit.Assert.Equal(10.0m, trade.Price);
        }

        [Fact]
        public void ProcessOrder_NoMatch_OrderAddedToBook()
        {
            var engine = new OrderMatcher();
            var buy = new Order("BUYER", "ABC", 100, 9.0m);
            var trades = engine.ProcessOrder(buy).ToList();

            Xunit.Assert.Empty(trades);
        }
    }
}
