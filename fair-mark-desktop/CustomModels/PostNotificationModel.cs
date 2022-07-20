using fair_mark_desktop.CustomModels.Enums;
using System;

namespace fair_mark_desktop.CustomModels
{
    /// <summary>
    /// Модель для отправки уведомлений пользователю
    /// </summary>
    [Serializable]
    public class PostNotificationModel
    {
        /// <summary>
        /// Сообщение для отправки пользователю
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Id пользователя (адресат)
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Тип уведомления
        /// </summary>
        public NotificationType Type { get; set; }
    }
}
