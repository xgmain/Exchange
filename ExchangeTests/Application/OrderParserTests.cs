using Xunit;

namespace Exchange.Application.Tests
{
    public class OrderParserTests
    {
        private readonly OrderParser _parser = new();

        [Fact]
        public void Parse_ValidInput_ReturnsOrder()
        {
            var input = "TR1:ABC:100:10.50";
            var order = _parser.Parse(input);

            Xunit.Assert.NotNull(order);
            Xunit.Assert.Equal("TR1", order!.TraderId);
            Xunit.Assert.Equal("ABC", order.Instrument);
            Xunit.Assert.Equal(100, order.Quantity);
            Xunit.Assert.Equal(10.50m, order.LimitPrice);
        }

        [Theory]
        [InlineData("")]
        [InlineData("TR1:ABC:badqty:10.50")]
        [InlineData("TR1:ABC:100:badprice")]
        [InlineData("TR1:ABC:100")]
        public void Parse_InvalidInput_ReturnsNull(string input)
        {
            var result = _parser.Parse(input);
            Xunit.Assert.Null(result);
        }
    }
}
