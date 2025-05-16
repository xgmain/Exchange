using Exchange.Application;
using Exchange.Core;
using Exchange.Infrastructure;
using Exchange.Infrastruture;

namespace Exchange
{
    class Program
    {
        const int BatchSize = 1000;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: dotnet run --project OrderMatcher <inputFile> <outputFile>");
                    return;
                }

                var inputPath = args[0];
                var outputPath = args[1];

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file '{inputPath}' does not exist.");
                    Logger.Error($"Input file '{inputPath}' not found.");
                    return;
                }

                var parser = new OrderParser();
                var reader = new FileOrderReader(inputPath, parser);
                var batcher = new OrderBatcher();
                var matcher = new OrderMatcher();
                var processor = new BatchProcessor();
                var writer = new FileTradeWriter(outputPath);

                Logger.Info("Reading orders...");
                var orders = reader.ReadAll().ToList();

                Logger.Info("Batching orders...");
                var batches = batcher.Batch(orders, BatchSize);

                Logger.Info("Processing batches...");
                var trades = processor.ProcessBatches(batches);

                Logger.Info("Writing trades...");
                writer.WriteToFile(trades);

                Logger.Info("Finished successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Fatal error: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                Logger.Shutdown();
            }
        }
    }
}
