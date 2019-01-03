using System.Collections.Generic;
using SpectabisLib.Enums;

namespace SpectabisLib.Models
{
    public class GameProfile
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string SerialNumber { get; set; }
        public string FilePath { get; set; }
        public string BoxArtPath { get; set; }
        public List<CommandLineArguments> LaunchOptions { get; set; }
    }
}