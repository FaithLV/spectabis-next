using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpectabisLib.Configuration;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisNext.Services
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        public SpectabisConfig Spectabis { get; private set; }
        public UIConfig UserInterface { get; private set; }
        public DirectoryConfig Directories { get; private set; }
        public TextConfig TextConfig { get; private set; }

        public ConfigurationLoader()
        {
            var update = UpdateConfiguration();
            Task.WaitAll(update);
        }

        public async Task UpdateConfiguration()
        {
            UserInterface = await ReadConfiguration<UIConfig>().ConfigureAwait(false);
            Spectabis = await ReadConfiguration<SpectabisConfig>().ConfigureAwait(false);
            Directories = await ReadConfiguration<DirectoryConfig>().ConfigureAwait(false);
            TextConfig = await ReadConfiguration<TextConfig>().ConfigureAwait(false);
        }

        public bool ConfigurationExists<T>() where T : IJsonConfig, new()
        {
            var configTitle = new T().FileName.ToLowerInvariant();
            var configUri = new Uri($"{SystemDirectories.ConfigFolder}/{configTitle}.json", UriKind.Absolute);
            return File.Exists(configUri.LocalPath);
        }

        public async Task WriteConfiguration<T>(T obj) where T : IJsonConfig, new()
        {
            var configTitle = new T().FileName.ToLowerInvariant();
            var configUri = new Uri($"{SystemDirectories.ConfigFolder}/{configTitle}.json", UriKind.Absolute);
            var configText = JsonConvert.SerializeObject(obj, Formatting.Indented);

            if (ConfigurationExists<T>())
            {
                File.Delete(configUri.LocalPath);
            }

            Logging.WriteLine($"Writing to file '{configUri.LocalPath}'");
            await AsyncIOHelper.WriteTextToFile(configUri, configText).ConfigureAwait(false);
        }

        public async Task WriteDefaultsIfNotExist<T>() where T : IJsonConfig, new()
        {
            if(ConfigurationExists<T>())
            {
                return;
            }

            var obj = new T();
            await WriteConfiguration<T>(obj).ConfigureAwait(false);
        }

        public async Task<T> ReadConfiguration<T>() where T : IJsonConfig, new()
        {
            if (!ConfigurationExists<T>())
            {
                return ReturnDefault<T>();
            }

            var configTitle = new T().FileName.ToLowerInvariant();
            Logging.WriteLine($"Loading '{configTitle}.json'");

            var configUri = new Uri($"{SystemDirectories.ConfigFolder}/{configTitle}.json", UriKind.Absolute);
            var configText = await AsyncIOHelper.ReadTextFromFile(configUri).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(configText);
        }

        private T ReturnDefault<T>() where T : IJsonConfig, new()
        {
            Logging.WriteLine($"Getting default config for '{typeof(T)}'");
            return new T();
        }
    }
}