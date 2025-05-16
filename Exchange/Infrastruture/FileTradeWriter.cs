using Exchange.Infrastructure;
using Exchange.Interface;

namespace Exchange.Infrastruture
{
    public class FileTradeWriter : IFileTradeWriter
    {
        private readonly string _outputPath;

        public FileTradeWriter(string outputPath)
        {
            _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
        }

        public void WriteToFile(IEnumerable<string> trades)
        {
            try
            {
                var directory = Path.GetDirectoryName(_outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    Logger.Info($"Created output directory: {directory}");
                }

                File.WriteAllLines(_outputPath, trades);
                Logger.Info($"Successfully wrote {trades.Count()} trades to {_outputPath}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to write trades: {ex.Message}");
            }
        }
    }
}
