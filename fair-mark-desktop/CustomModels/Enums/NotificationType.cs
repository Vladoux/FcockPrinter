using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair_mark_desktop.CustomModels.Enums
{
    /// <summary>
    /// Перечисление типов оповещений
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// О завершении печати принтера
        /// </summary>
        PrinterCompleted,
        /// <summary>
        /// 1 Ошибка печати принтера
        /// </summary>
        PrinterError,
        /// <summary>
        /// 2 Файл получен успешно
        /// </summary>
        FileReceivedSuccess,
        /// <summary>
        /// 3 Файл не получен
        /// </summary>
        FileReceivedError
    }
}
