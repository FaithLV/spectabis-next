using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Configuration
{
    public class TextConfig : IJsonConfig
    {
        public string FileName => nameof(TextConfig).ConfigClassToFileName();

        public string TelemetryOptedOut = "Opted-out of telemetry";
        public string TelemetryEnabled = "Telemetry is enabled";
    }
}