using Xunit;
using Exchange.Core;

namespace Exchange.Application.Tests
{
    public class BatchProcessorTests
    {
        [Fact]
        public void ProcessBatches_ReturnsExpectedTrades()
        {
            var processor = new BatchProcessor();

            var matchingBatch = new List<Order>
            {
                new Order("T1", "AAPL", -100, 10.0m),
                new Order("T2", "AAPL", 100, 10.0m)
            };

            var batches = new List<List<Order>> { matchingBatch };
            var trades = processor.ProcessBatches(batches).ToList();

            Xunit.Assert.Single(trades);
            Xunit.Assert.Contains("T2:T1:AAPL:100:10.00", trades[0]);
        }

        [Fact]
        public void ProcessBatches_ReturnUnexpectedTrades()
        {
            var processor = new BatchProcessor();

            var nonMatchingBatch = new List<Order>
            {
                new Order("T3", "AAPL", -100, 12.0m), // Sell at 12.0
                new Order("T4", "AAPL", 100, 10.0m)   // Buy at 10.0 → won't match
            };

            var batches = new List<List<Order>> { nonMatchingBatch };
            var trades = processor.ProcessBatches(batches).ToList();

            Xunit.Assert.Empty(trades);
        }
    }
}
