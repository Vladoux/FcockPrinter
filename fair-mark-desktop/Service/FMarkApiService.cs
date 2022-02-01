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
    public class FMarkApiService
    {
#if DEBUG
        private const string _baseUrl = "http://10.81.80.6:6162";
#else
        private const string _baseUrl = "http://94.198.50.203:81/api";
#endif   
        public async Task<ResponseResult> CheckNewVersion()
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
    }

    public class ResponseResult
    {
        public string Value { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
