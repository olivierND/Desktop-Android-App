using System;
using System.Configuration;
using System.Threading.Tasks;
using ClientLourd.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClientLourd.Services
{
    public class SignalRService
    {
        private readonly HubConnection _connection;
        private string _bearerToken;

        public SignalRService(string bearerToken)
        {
            _bearerToken = bearerToken;

            _connection = new HubConnectionBuilder()
                .WithUrl(ConfigurationManager.AppSettings["Server:BaseUrl"] + "MessageHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_bearerToken);
                })
                .Build();

            Initialize();
        }

        private async void Initialize()
        {
            try
            {
                await _connection.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            _connection.On("Connected", (string res) =>
            {
                var a = res;
            });

            _connection.On("ReceiveMessage", (string res) => { MessageViewModel.GetInstance(_bearerToken).SetLastMessageReceived(res); });
        }

        private bool CheckIfConnected()
        {
            return _connection.State == HubConnectionState.Connected;
        }

        public async Task<string> SendMessage(string username, string message)
        {
            try
            {
                if (!CheckIfConnected())
                {
                    Initialize();
                }
                return await _connection.InvokeAsync<string>("SendMessage", username, message);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}