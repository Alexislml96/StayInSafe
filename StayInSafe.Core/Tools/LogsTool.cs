using Newtonsoft.Json;
using StayInSafe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace StayInSafe.Core.Tools
{
    public class LogsTool
    {
        public async Task<bool> InsertLog(Logs log)
        {
            Uri requestUri = new Uri("https://infinite-beach-55400.herokuapp.com/v1/logs/newLog");
            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(log);
            var jsString = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri,jsString);

            return response.StatusCode == HttpStatusCode.OK;

        }
    }
}
