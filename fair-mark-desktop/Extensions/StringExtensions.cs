using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace fair_mark_desktop.Extensions
{
    public static class StringExtensions
    {
        public static void WriteToFile(this string source, string text)
        {
            try
            {
                source.CreateNotExistDirectories();
                File.WriteAllText(source, text);
            }
            catch (Exception)
            {
                // ignored
            }
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

        public static int GetCountPagesPdf(this string source)
        {
            var pdfReader = new PdfReader(source);
            var result = pdfReader.NumberOfPages;
            pdfReader.Close();
            return result;
        }

        public static string ReadFromFile(this string source)
        {
            return File.Exists(source) ? File.ReadAllText(source) : null;
        }

        public static void Clean(this string source)
        {
            if (Directory.Exists(source))
            {
                var list = Directory.GetDirectories(source);
                foreach (var item in list)
                {
                    var days = (DateTime.Today - Directory.GetCreationTime(item)).TotalDays;
                    if (days > 30) Directory.Delete(item, true);
                }

                ;
            }
        }

        public static string ToStrikeText(this string source, bool isfile = false)
        {
            var sym = "̶";
            var newText = " ̶";
            var newnew = isfile ? Path.GetFileNameWithoutExtension(source) : source;

            foreach (var s in newnew)
            {
                newText += s + sym;
            }

            return isfile ? newText : newText + $".pdf";
        }
    }
}