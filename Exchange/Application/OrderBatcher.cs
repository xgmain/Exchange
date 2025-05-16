using Exchange.Core;

namespace Exchange.Application
{
    public class OrderBatcher
    {
        public IEnumerable<List<Order>> Batch(IEnumerable<Order> orders, int batchSize)
        {
            if (orders == null) throw new ArgumentNullException(nameof(orders));
            if (batchSize <= 0) throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch size must be greater than zero.");

            var batch = new List<Order>(batchSize);

            foreach (var order in orders)
            {
                batch.Add(order);
                if (batch.Count >= batchSize)
                {
                    yield return batch; //process orders in memory efficient way
                    batch = new List<Order>(batchSize);
                }
            }

            if (batch.Count > 0) //process any remaining orders
                yield return batch;
        }
    }
}
