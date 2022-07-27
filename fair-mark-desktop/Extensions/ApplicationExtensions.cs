using System.Diagnostics;
using System.Reflection;

namespace fair_mark_desktop.Extensions
{
    public static class ApplicationSettings
    {
        public static string GetNormalizeProductVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return $"{fvi.FileMajorPart}.{fvi.FileMinorPart}.{fvi.FileBuildPart}";
        }
    }
}
