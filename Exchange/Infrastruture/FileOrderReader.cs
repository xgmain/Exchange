using Exchange.Application;
using Exchange.Core;
using Exchange.Interface;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace Exchange.Infrastruture
{

    public class FileOrderReader : IFileOrderReader
    {
        private readonly string _path;
        private readonly OrderParser _parser;

        public FileOrderReader(string path, OrderParser parser)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public IEnumerable<Order> ReadAll()
        {
            using var mmf = MemoryMappedFile.CreateFromFile(_path, FileMode.Open, null, 0, MemoryMappedFileAccess.Read);
            using var stream = mmf.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
            using var reader = new StreamReader(stream, Encoding.UTF8, true, 8192);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var order = _parser.Parse(line);
                if (order != null) yield return order;
            }
        }
    }
}
