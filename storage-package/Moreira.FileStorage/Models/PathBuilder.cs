using System.Text;

namespace Moreira.FileStorage.Models
{
    public class PathBuilder
    {
        private readonly string _delimiter;
        private StringBuilder _builder;

        public PathBuilder(string delimiter)
        {
            _delimiter = delimiter;
            _builder = new StringBuilder();
        }

        public void Append(object path)
        {
            if (_builder.Length > 0)
                _builder.Append(_delimiter);

            _builder.Append(path);
        }

        public void Reset()
        {
            _builder.Clear();
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
