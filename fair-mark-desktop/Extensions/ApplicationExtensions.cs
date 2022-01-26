using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace fair_mark_desktop.Extensions
{
    public static class ApplicationSettings
    {
        public static string GetNormalizeProductVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return $"{fvi.FileMajorPart}.{fvi.FileMinorPart}.{fvi.FileBuildPart}";
        }
    }
}
