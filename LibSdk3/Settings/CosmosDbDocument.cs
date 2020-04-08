using System.IO;

namespace LibSdk3.Settings
{
    public class CosmosDbDocument
    {
        private string _document;

        public string Document
        {
            get
            {
                if (IsJsonFile())
                {
                    _document = File.ReadAllText(_document);
                }

                return _document;
            }
            set => _document = value;
        }

        private bool IsJsonFile()
        {
            if (string.IsNullOrWhiteSpace(_document))
            {
                return false;
            }

            return !_document.StartsWith("{");
        }
    }
}