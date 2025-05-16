using System.Collections.Concurrent;

namespace Exchange.Core
{
    public class OrderMatcher
    {
        private readonly ConcurrentDictionary<string, List<Order>> _orderList = new();

        public IEnumerable<Trade> ProcessOrder(Order newOrder)
        {
            if (newOrder == null)
                throw new ArgumentNullException(nameof(newOrder), "Order cannot be null.");

            var trades = new List<Trade>();
            var instrument = newOrder.Instrument;

            if (string.IsNullOrWhiteSpace(instrument))
                throw new ArgumentException("Instrument must be specified.", nameof(newOrder));

            _orderList.TryAdd(instrument, new List<Order>());
            var book = _orderList[instrument];

            var candidates = book
                //This filters the order book to only include orders on the opposite side of the new order
                .Where(o => o.IsBuy != newOrder.IsBuy)
                //matches with sell orders where the new order's bid price is ≥ the existing ask price
                //matches with buy orders where the new order's ask price is ≤ the existing bid price
                .Where(o => newOrder.IsBuy ? newOrder.LimitPrice >= o.LimitPrice : newOrder.LimitPrice <= o.LimitPrice)
                .OrderBy(o => newOrder.IsBuy ? o.LimitPrice : -o.LimitPrice)
                .ToList();

            foreach (var candidate in candidates)
            {
                if (newOrder.Quantity == 0)
                    break;

                //Determines the executable quantity as the smaller of the two orders, eg: seller 100, buyer 50, then 50
                int matchQty = Math.Min(Math.Abs(candidate.Quantity), Math.Abs(newOrder.Quantity));

                //For buy orders: matches at the sell order's price (ask)
                //For sell orders: matches at the buy order's price (bid)
                decimal matchPrice = candidate.LimitPrice;

                //Identifies buyer and seller correctly based on order types
                var buyer = newOrder.IsBuy ? newOrder : candidate;
                var seller = newOrder.IsBuy ? candidate : newOrder;

                //Creates a new trade record
                trades.Add(new Trade(buyer.TraderId, seller.TraderId, instrument, matchQty, matchPrice));

                //Reduces quantities on both orders by the matched amount
                candidate.Quantity += candidate.Quantity > 0 ? -matchQty : matchQty;
                newOrder.Quantity += newOrder.Quantity > 0 ? -matchQty : matchQty;

                //Removes fully filled orders from the order book
                if (candidate.Quantity == 0)
                    book.Remove(candidate);
            }

            //If the new order isn't completely filled, adds it to the order book, ensures remaining quantity stays available for future matches
            if (newOrder.Quantity != 0)
                book.Add(newOrder);

            return trades;
        }
    }

}
