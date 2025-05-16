using Exchange.Core;
using Exchange.Infrastructure;
using System.Collections.Concurrent;

namespace Exchange.Application
{
    public class BatchProcessor
    {
        public ConcurrentBag<string> ProcessBatches(IEnumerable<List<Order>> batches)
        {
            var allTrades = new ConcurrentBag<string>(); //Thread-safe collection for storing trade results
            var errors = new ConcurrentBag<string>(); //Thread-safe collection for error messages

            Parallel.ForEach(batches, batch => //Automatically manages thread pooling for optimal performance
            {
                try
                {
                    var matcher = new OrderMatcher(); // Thread-safe scope
                    foreach (var order in batch)
                    {
                        try
                        {
                            var trades = matcher.ProcessOrder(order);
                            foreach (var trade in trades)
                            {
                                allTrades.Add(trade.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Order failed: {order.TraderId}:{order.Instrument} - {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($"Batch failed: {ex.Message}");
                }
            });

            if (!errors.IsEmpty)
            {
                foreach (var err in errors)
                    Logger.Error(err);
            }

            return allTrades;
        }
    }

}
