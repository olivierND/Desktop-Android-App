using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PolyPaint.Models;

namespace PolyPaint.Services
{
    public class ServerService
    {
        private static readonly string BaseUrl = ConfigurationManager.AppSettings["Server:BaseUrl"] + "api";
        private static readonly HttpClient Client = new HttpClient();


        public ServerService(string bearerToken)
        {
           Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        public async Task<HttpContent> GetMessages(string channelId)
        {
            HttpResponseMessage res = await Client.GetAsync(BaseUrl + $"/Channel/{channelId}/messages");

            return res.IsSuccessStatusCode ? res.Content : null;
        }

        public async Task<HttpContent> GetLobbies()
        {
            HttpResponseMessage res = await Client.GetAsync(BaseUrl + "/Channel/lobby");

            return res.IsSuccessStatusCode ? res.Content : null;
        }


        public async Task<HttpContent> CreateNewChannel(string channelName, bool isLobby)
        {
            Channel c = new Channel
            {
                name = channelName,
                message = new List<Message>(),
                userChannel = new List<UserChannel>(),
                isLobby = isLobby
            };

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(c), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage res = await Client.PostAsync(BaseUrl + "/Channel", httpContent);

            return res.IsSuccessStatusCode ? res.Content : null;
        }
    }
}