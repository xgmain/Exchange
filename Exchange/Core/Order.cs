namespace Exchange.Core
{
    public class Order
    {
        public string TraderId { get; }
        public string Instrument { get; }
        public int Quantity { get; set; }
        public decimal LimitPrice { get; }

        public bool IsBuy => Quantity > 0;

        public Order(string traderId, string instrument, int quantity, decimal limitPrice)
        {
            TraderId = traderId;
            Instrument = instrument;
            Quantity = quantity;
            LimitPrice = limitPrice;
        }
    }
}


