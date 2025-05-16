namespace Exchange.Interface
{
    public interface IFileTradeWriter
    {
        void WriteToFile(IEnumerable<string> trades);
    }
}
