using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Module7
{
    static class Responses
    {
        public static async Task Show(HttpResponseMessage response)
        {
            HttpRequestMessage request = response.RequestMessage;
            string body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{request.Method} {request.RequestUri.AbsolutePath} -> " +
                              $"{(int)response.StatusCode} {response.StatusCode} {body}");
        }
    }
}
