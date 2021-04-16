using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisLib.Abstractions;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Repositories
{
    public class GameProfileRepository : IProfileRepository
    {
        private IList<GameProfile> _games;
        private readonly ProfileFileSystem _fileSystem;

        public GameProfileRepository(ProfileFileSystem pfs)
        {
            _fileSystem = pfs;
        }

        public async Task UpsertProfile(GameProfile profile)
        {
            if(profile.Id == Guid.Empty)
            {
                profile.Id = Guid.NewGuid();
            }

            if(!_games.Contains(profile))
            {
                _games.Add(profile);
            }

            await _fileSystem.WriteConfiguration(profile).ConfigureAwait(false);

            if(!_fileSystem.IsProfileContainerValid(profile))
            {
                await _fileSystem.WriteDefaultConfiguration(profile).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<GameProfile>> GetAll()
        {
            return _games ??= await _fileSystem.ReadAllProfiles().ConfigureAwait(false);
        }

        public void DeleteProfile(GameProfile profile)
        {
            _games.Remove(profile);
            _fileSystem.DeleteProfile(profile.Id);
        }
    }
}