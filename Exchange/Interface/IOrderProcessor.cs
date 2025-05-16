namespace Exchange.Interface
{
    public interface IOrderProcessor
    {
        IEnumerable<string> Process(string input);
    }
}
