using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.AspNetCore.SignalR.Client;
using PolyPaint.Constants;
using PolyPaint.Models;
using PolyPaint.ViewModels;

namespace PolyPaint.Services
{
    public class SignalRService
    {
        private readonly HubConnection _connection;

        private readonly MainWindowViewModel _mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow?.DataContext;


        public SignalRService(string bearerToken)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(ConfigurationManager.AppSettings["Server:BaseUrl"] + "MessageHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(bearerToken);
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

            _connection.On(SignalRFunctions.Connected, (string res) =>
            {
                var a = res;
            });

            _connection.On(SignalRFunctions.ReceiveMessage, (string res) =>
            {
                Messenger.Default.Send(SignalRFunctions.ReceiveMessage + res);
                _mainWindowViewModel.LastMessageReceived = res;
            });

            _connection.On(SignalRFunctions.Join, (object res) =>
            {
                var a = res as Message;
            });
        }

        private bool CheckIfConnected()
        {
            return _connection.State == HubConnectionState.Connected;
        }

        public async Task<string> SendMessage(string username, string channelId, string message)
        {
            try
            {
                if (!CheckIfConnected())
                {
                    Initialize();
                }
                return await _connection.InvokeAsync<string>(SignalRFunctions.SendMessage, username, channelId, message);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<string> JoinChannel(string username, string channelId)
        {
            try
            {
                if (!CheckIfConnected())
                {
                    Initialize();
                }
                return await _connection.InvokeAsync<string>(SignalRFunctions.Join, username, channelId);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}