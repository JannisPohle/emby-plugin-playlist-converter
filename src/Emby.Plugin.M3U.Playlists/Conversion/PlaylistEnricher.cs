using System;
using System.Threading.Tasks;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Models;
using Emby.Plugin.M3U.Playlists.Service.ParameterModels;

namespace Emby.Plugin.M3U.Playlists.Conversion
{
  /// <summary>
  ///   Enriches the internal playlist with information from emby
  /// </summary>
  /// <seealso cref="IPlaylistEnricher" />
  public class PlaylistEnricher: IPlaylistEnricher
  {
    #region Interfaces

    /// <inheritdoc />
    public async Task EnrichPlaylistInformation(Playlist playlist, ImportPlaylist request)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}