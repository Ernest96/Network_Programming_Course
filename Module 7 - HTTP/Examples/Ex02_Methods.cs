using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Module7
{
    // POST, GET, PUT and DELETE. The same client works with mockapi.io and with our own server.
    class Ex02_Methods : IExample
    {
        string baseUrl;

        public Ex02_Methods(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public async Task RunAsync()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            // POST
            Student maria = new Student("Maria", "TI-231", 2);
            string mariaJson = JsonConvert.SerializeObject(maria);
            
            StringContent stringContent = new StringContent(mariaJson, Encoding.UTF8, "application/json");
            
            HttpResponseMessage created = await client.PostAsync("students", stringContent);
            await Responses.Show(created);

            Student student = JsonConvert.DeserializeObject<Student>(await created.Content.ReadAsStringAsync());
            string path = "students/" + student.Id;

            await Responses.Show(await client.GetAsync(path));
            
            // PUT
            Student mariaChanged = new Student("Maria", "TI-231", 3);
            string mariaChangedJson = JsonConvert.SerializeObject(mariaChanged);
            
            stringContent = new StringContent(mariaChangedJson, Encoding.UTF8, "application/json");
            await Responses.Show(await client.PutAsync(path, stringContent));

            // DELETE
            await Responses.Show(await client.DeleteAsync(path));
            
            // GET
            await Responses.Show(await client.GetAsync(path));
        }
    }
}
