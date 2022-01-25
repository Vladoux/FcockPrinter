using System.IO;
using System.Linq;

namespace fair_mark_desktop.Extensions
{
    public static class StringExtensions
    {
        public static void WriteToFile(this string source, string text)
        {
            source.CreateNotExistDirectories();
            File.WriteAllText(source, text);
        }

        /// <summary>
        /// Initialize file with text
        /// </summary>
        /// <param name="source"></param>
        /// <param name="text"></param>
        public static void FirstCreateFile(this string source, string text = null)
        {
            source.CreateNotExistDirectories();
            if (!File.Exists(source) && text != null)
                File.WriteAllText(source, text);
        }

        /// <summary>
        /// Return true -> created, false -> don't created
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static bool CreateNotExistDirectories(this string source)
        {
            var result = false;
            var splitText = source.Split('\\');
            var checkPath = string.Empty;

            foreach (var line in splitText.Take(splitText.Length - 1))
            {
                checkPath += $"{line}\\";
                if (!Directory.Exists(checkPath))
                {
                    Directory.CreateDirectory(checkPath);
                    result = true;
                }
            }
            return result;
        }

        public static string GetFileNameFromPath(this string source)
        {
            return source.Split('\\').Last();
        }
    }
}
