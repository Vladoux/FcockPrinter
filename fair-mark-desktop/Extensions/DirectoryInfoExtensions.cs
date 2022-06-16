using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair_mark_desktop.Extensions
{
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Рекурсивный поиск файлов в папке по формату
        /// </summary>
        /// <param name="directoryInfo">Исходная папка</param>
        /// <param name="searchFormatFile">Формат файлов</param>
        /// <returns>Все файлы всех папок</returns>
        public static IEnumerable<FileInfo> GetFilesDeepInfo(this DirectoryInfo directoryInfo, string searchFormatFile)
        {
            // инициализация результата
            var allFiles = new List<FileInfo>();
            
            // получение файлов в текущей папке
            var files = directoryInfo.GetFiles(searchFormatFile);
            // добавление в список
            allFiles.AddRange(files);
            
            // получение папок в текущей папке
            var directories = directoryInfo.GetDirectories();
            // проход по ним
            foreach (var directory in directories)
            {
                // добавление в список рекурсивного результата
                allFiles.AddRange(GetFilesDeepInfo(directory, searchFormatFile));
            }
            // возврат результата
            return allFiles;
        }

    }
}
