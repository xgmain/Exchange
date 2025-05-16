namespace Exchange.Core
{
    public struct Trade
    {
        public string BuyerId { get; }
        public string SellerId { get; }
        public string Instrument { get; }
        public int Quantity { get; }
        public decimal Price { get; }

        public Trade(string buyerId, string sellerId, string instrument, int quantity, decimal price)
        {
            BuyerId = buyerId;
            SellerId = sellerId;
            Instrument = instrument;
            Quantity = quantity;
            Price = price;
        }

        public override string ToString()
        {
            return $"{BuyerId}:{SellerId}:{Instrument}:{Quantity}:{Price:F2}";
        }
    }

}
