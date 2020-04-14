using System;
using System.Diagnostics;
using System.IO;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameLauncherPCSX2 : IGameLauncher
    {
        private GameProcess _gameProcess;
        private readonly ProfileFileSystem _pfs;

        public GameLauncherPCSX2(ProfileFileSystem pfs)
        {
            _pfs = pfs;
        }

        public GameProcess StartGame(GameProfile game)
        {
            var process = CreateEmulatorProcess(game);
            var gameProcess = new GameProcess(game, process);

            _gameProcess = gameProcess;
            _gameProcess.Start();

            return gameProcess;
        }

        public GameProcess GetRunningGame()
        {
            return _gameProcess;
        }

        public void StopGame()
        {
            if (_gameProcess == null)
            {
                return;
            }

            _gameProcess.Stop();
            _gameProcess = null;
        }

        private Process CreateEmulatorProcess(GameProfile gameProfile)
        {
            if (_gameProcess != null)
            {
                Console.WriteLine($"[PCSX2-GameLauncher] Losing lease of existing process '{_gameProcess.Game.Title}' : '{_gameProcess.Process.Id}'");
            }

            var process = new Process();

            var launchArguments = EmulatorOptionsParser.ConvertToLaunchArguments(gameProfile.LaunchOptions);
            var romArgument = EmulatorOptionsParser.RomPathToArgument(gameProfile.FilePath);
            var cfgArgument = EmulatorOptionsParser.ConfigurationPathToArgument(_pfs.GetProfileConfigLocation(gameProfile));
            var fullArguments = $"{launchArguments} {romArgument} {cfgArgument}";

            process.StartInfo.FileName = GetEmulatorPath(gameProfile);
            process.StartInfo.Arguments = fullArguments;
            process.EnableRaisingEvents = true;

            return process;
        }

        private string GetEmulatorPath(GameProfile profile)
        {
            if (string.IsNullOrWhiteSpace(profile.EmulatorPath))
            {
                return SystemDirectories.PCSX2ExecutablePath;
            }

            return profile.EmulatorPath;
        }
    }
}