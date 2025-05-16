using Exchange.Core;
using Exchange.Infrastructure;
using Exchange.Interface;

namespace Exchange.Application
{
    public class OrderParser : IOrderParser
    {
        public Order? Parse(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;

            try
            {
                var parts = line.Trim().Split(':');
                if (parts.Length != 4)
                    throw new FormatException("Input must contain exactly four parts separated by colons.");

                var traderId = parts[0].Trim();
                var instrument = parts[1].Trim();
                var quantityStr = parts[2].Trim();
                var priceStr = parts[3].Trim();

                if (string.IsNullOrWhiteSpace(traderId) ||
                    string.IsNullOrWhiteSpace(instrument) ||
                    string.IsNullOrWhiteSpace(quantityStr) ||
                    string.IsNullOrWhiteSpace(priceStr))
                {
                    throw new ArgumentException("One or more fields are empty or whitespace.");
                }

                if (!int.TryParse(quantityStr, out var quantity))
                    throw new FormatException($"Invalid quantity format: '{quantityStr}'");
                if (!decimal.TryParse(priceStr, out var price))
                    throw new FormatException($"Invalid price format: '{priceStr}'");

                return new Order(traderId, instrument, quantity, price);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to parse line: '{line}'. Reason: {ex.GetType().Name} - {ex.Message}");
                return null;
            }
        }
    }
}
