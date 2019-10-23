using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClientLourd.Services
{
    public class ServerService
    {
        private static string baseUrl = ConfigurationManager.AppSettings["Server:BaseUrl"] + "api";
        private static HttpClient client = new HttpClient();


        public ServerService(string bearerToken)
        {
           client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        public async Task<HttpContent> GetMessages()
        {
            HttpResponseMessage res = await client.GetAsync(baseUrl + "/message");

            return res.IsSuccessStatusCode ? res.Content : null;
        }
    }
}