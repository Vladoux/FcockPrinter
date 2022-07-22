using fair_mark_desktop.CustomModels;
using fair_mark_desktop.CustomModels.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fair_mark_desktop.Service
{
    public static class FMarkApiService
    {
#if DEBUG
        private const string _baseUrl = "http://10.81.80.6:6161/api";
#else
        private const string _baseUrl = "https://fc.caspel.com/api";
#endif   

        public static readonly string DownloadAppUrl = $"{_baseUrl}/application-info/win-app-setup";
        
        public static async Task<ResponseResult> CheckNewVersion()
        {
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(_baseUrl) })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    var response = await client.GetAsync($"{_baseUrl}/application-info/win-app-version");
                    if (response.IsSuccessStatusCode)
                    {
                        var readTask = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var rawResponse = readTask.GetAwaiter().GetResult();
                        var obj = JsonConvert.DeserializeObject<ResponseResult>(rawResponse);
                        return obj;
                    }
                }
                return new ResponseResult()
                {
                    IsSuccess = false
                };
            }
            catch (Exception e)
            {
                return new ResponseResult
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }

        }

        /// <summary>
        /// Оповещение пользователя
        /// </summary>
        /// <param name="userId">Id пользователя системы FairCode</param>
        /// <param name="message">Сообщение для отправки</param>
        /// <param name="type">Тип оповещения</param>
        /// <returns></returns>
        public static async Task<ResponseResult> NotifyUserFMark(string userId, string message, NotificationType type)
        {
            try
            {
                // формирование тела для отправки в формате json
                var json = JsonConvert.SerializeObject(new PostNotificationModel
                {
                    Message = message,
                    UserId = userId,
                    Type = type
                });
                // создание тела для самого запроса
                var sendObject = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient { BaseAddress = new Uri(_baseUrl) })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    var response = await client.PostAsync($"{_baseUrl}/notifications/notify-user", sendObject);
                    if (response.IsSuccessStatusCode)
                    {
                        var readTask = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var rawResponse = readTask.GetAwaiter().GetResult();
                        var obj = JsonConvert.DeserializeObject<ResponseResult>(rawResponse);
                        return obj;
                    }
                }
                return ResponseResult.Error();
            }
            catch(Exception e)
            {
                return ResponseResult.Error(e.Message);
            }
        }
    }

    public class ResponseResult
    {
        public string Value { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public static ResponseResult Error(string message = null)
        {
            return new ResponseResult
            {
                IsSuccess = false,
                Message = message
            };
        }
    }

    public class ResponseResult<T>
    {
        public T Value { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
