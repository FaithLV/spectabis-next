using SpectabisLib.Models;

namespace SpectabisLib.Interfaces.Controllers
{
    public interface IGameLibraryController
    {
        void LaunchGame(GameProfile game);
        void LaunchConfiguration(GameProfile game);
        void DeleteGame(GameProfile game);
        void OpenWikiPage(GameProfile game);
    }
}