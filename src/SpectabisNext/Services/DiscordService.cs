using System;
using DiscordRPC;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisLib.Helpers;
using SpectabisLib;

namespace SpectabisNext.Services
{
    public class DiscordService : IDiscordService
    {
        private static readonly ulong StartTime = (ulong) DateTimeOffset.Now.ToUnixTimeMilliseconds();
        private static DiscordRpcClient client;
        private readonly IConfigurationLoader _configLoader;

        public DiscordService(IConfigurationLoader configLoader)
        {
            _configLoader = configLoader;
        }

        public void InitializeDiscord()
        {
            if(!_configLoader.Spectabis.EnableDiscordRichPresence)
            {
                return;
            }

            Logging.WriteLine("Starting DiscordService client");

            client = new DiscordRpcClient(Constants.DiscordClientId);
            client.Initialize();

            SetMenuPresence();
        }

        public void SetMenuPresence()
        {
            SetStatus("Library");
        }

        public void SetGamePresence(GameProfile game)
        {
            SetStatus(game.Title);
        }

        private void SetStatus(string status)
        {
            if(!_configLoader.Spectabis.EnableDiscordRichPresence)
            {
                return;
            }

            if(client == null)
            {
                InitializeDiscord();
            }

            Logging.WriteLine($"Updating status to: '{status}'");

            var presence = new RichPresence()
            {
                Details = status,
                Timestamps = new Timestamps()
                {
                    StartUnixMilliseconds = StartTime
                },
                Assets = new Assets()
                {
                    LargeImageKey = "menus",
                }
            };

            client.SetPresence(presence);
        }

        ~DiscordService()
        {
            client.Dispose();
        }
    }
}