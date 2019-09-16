﻿using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LanguageHandler
{
    public class LanguageHandler
    {
        public static async Task<string> Translate(string text, string fromLanguage, string toLanguage)
        {
            dynamic result = string.Empty;
            try
            {
                string uri = ClientSecrets.uri;
                var uriBuilder = new UriBuilder(uri);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["from"] = fromLanguage;
                query["to"] = toLanguage;
                uriBuilder.Query = query.ToString();
                uri = uriBuilder.ToString();
                System.Object[] body = new System.Object[] { new { Text = text } };
                var requestBody = JsonConvert.SerializeObject(body);

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(uri);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", ClientSecrets.key);

                    var response = await client.SendAsync(request);
                    var responseBody = await response.Content.ReadAsStringAsync();
                     result = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);
                }
            }
            catch(Exception ex)
            {
                result = new ExpandoObject();
                result.ErrorCode = 500;
                result.ErrorMessage = ex.StackTrace.ToString();
                result = JsonConvert.SerializeObject(result);

            }
            return result;
        }
    }
}
