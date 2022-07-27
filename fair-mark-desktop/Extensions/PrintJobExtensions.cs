using System.Printing;
using fair_mark_desktop.CustomModels.Enums;

namespace fair_mark_desktop.Extensions
{
    public static class PrintJobExtensions
    {
        /// <summary>
        /// Получение флага "В работе" печатаемого документа
        /// </summary>
        /// <param name="systemJobInfo">Петачуемый документ</param>
        /// <returns></returns>
        public static bool InWork(this PrintSystemJobInfo systemJobInfo)
        {
            return systemJobInfo != null &&
                   (systemJobInfo.IsPrinting || systemJobInfo.IsDeleting || systemJobInfo.IsSpooling);
        }

        /// <summary>
        /// Получение статуса печатаемого документа
        /// </summary>
        /// <param name="systemJobInfo">Петачуемый документ</param>
        /// <returns></returns>
        public static (string, bool, NotificationType) GetStatusWithNotify(this PrintSystemJobInfo systemJobInfo)
        {
            systemJobInfo.Refresh();

            switch (systemJobInfo.JobStatus)
            {
                case PrintJobStatus.Printed:
                case PrintJobStatus.Completed:
                case PrintJobStatus.Deleted:
                case PrintJobStatus.Retained:
                    return ($"Файл {systemJobInfo.Name} напечатан", true, NotificationType.PrinterCompleted);
                case PrintJobStatus.Offline:
                    return ("Принтер недоступен", true, NotificationType.PrinterError);
                case PrintJobStatus.PaperOut:
                    return ("В принтере закончился требуемый размер бумаги", true, NotificationType.PrinterError);
                case PrintJobStatus.Blocked:
                    return ("Печать для файла заблокирована", true, NotificationType.PrinterError);
                case PrintJobStatus.Error:
                    return ("Ошибка при печати", true, NotificationType.PrinterError);
                case PrintJobStatus.UserIntervention:
                    return ("Принтер требует действий пользователя для устранения ошибки.", true, NotificationType.PrinterError);
                case PrintJobStatus.None:
                    return ("None", false, NotificationType.PrinterError);
            }

            return (string.Empty, false, NotificationType.PrinterError);
        }
    }
}