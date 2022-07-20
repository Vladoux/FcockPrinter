using fair_mark_desktop.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair_mark_desktop.CustomModels
{
    public class StorageFilePaths : List<string>
    {
        private readonly string _path = Path.Combine(Path.GetTempPath(), "FCode", "StoragePaths.txt");
        private readonly string _userIdPath = Path.Combine(Path.GetTempPath(), "FCode", "userId.txt");
        public new void AddRange(IEnumerable<string> items)
        {
            base.AddRange(items);
            SaveToFile();
        }

        public new void Remove(string value)
        {
            base.Remove(value);
            SaveToFile();
        }

        private void SaveToFile()
        {
            var text = string.Join("\n", this);
            _path.WriteToFile(text);
        }

        public List<string> GetPaths()
        {
            return _path.ReadFromFile()?.Split('\n')
                .Where(x => !string.IsNullOrEmpty(x) && File.Exists(x)).ToList() ?? new List<string>();
        }

        public string LastConnectionUserId => File.Exists(_userIdPath) ? File.ReadAllText(_userIdPath) : null;
    }
}
