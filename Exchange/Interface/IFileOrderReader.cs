using Exchange.Core;

namespace Exchange.Interface
{
    public interface IFileOrderReader
    {
        IEnumerable<Order> ReadAll();
    }
}
