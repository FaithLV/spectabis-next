using System.Threading.Tasks;
using SpectabisLib.Configuration;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Services
{
    public class FirstTimeWizardService : IFirstTimeWizardService
    {
        private readonly IConfigurationLoader _configLoader;

        public FirstTimeWizardService(IConfigurationLoader configLoader)
        {
            _configLoader = configLoader;
        }

        public async Task WriteInitialConfigs()
        {
            await _configLoader.WriteDefaultsIfNotExist<DirectoryConfig>().ConfigureAwait(false);
            await _configLoader.WriteDefaultsIfNotExist<SpectabisConfig>().ConfigureAwait(false);
        }

        public async Task WriteFirstTimeWizardCompleted()
        {
            _configLoader.Spectabis.RunFirstTimeWizard = false;
            await _configLoader.WriteConfiguration(_configLoader.Spectabis).ConfigureAwait(false);
        }

        public bool IsRequired()
        {
            return _configLoader.Spectabis.RunFirstTimeWizard;
        }
    }
}