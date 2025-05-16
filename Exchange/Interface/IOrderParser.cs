using Exchange.Core;

namespace Exchange.Interface
{
    public interface IOrderParser
    {
        Order? Parse(string line);
    }
}