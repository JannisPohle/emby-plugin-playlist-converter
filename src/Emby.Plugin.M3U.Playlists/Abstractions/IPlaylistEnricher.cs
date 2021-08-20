using System.Threading.Tasks;
using Emby.Plugin.M3U.Playlists.Models;
using Emby.Plugin.M3U.Playlists.Service.ParameterModels;

namespace Emby.Plugin.M3U.Playlists.Abstractions
{
  /// <summary>
  ///   Enriches the internal playlist model with information from emby
  /// </summary>
  public interface IPlaylistEnricher
  {
    #region Methods

    /// <summary>
    ///   Enriches the playlist information.
    /// </summary>
    /// <param name="playlist">The playlist.</param>
    /// <param name="request">The playlist creation request.</param>
    /// <returns></returns>
    Task EnrichPlaylistInformation(Playlist playlist, ImportPlaylist request);

    #endregion
  }
}