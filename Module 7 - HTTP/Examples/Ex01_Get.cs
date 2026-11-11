using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Module7
{
    // GET with HttpClient
    class Ex01_Get : IExample
    {
        string baseUrl;

        public Ex01_Get(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public async Task RunAsync()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl); // the start of every URL below

            HttpResponseMessage response1 = await client.GetAsync("students");
            HttpResponseMessage response2 = await client.GetAsync("students/1");
            HttpResponseMessage response3 = await client.GetAsync("students/99");
            
            await Responses.Show(response1);
            await Responses.Show(response2);
            await Responses.Show(response3);
        }
    }
}
