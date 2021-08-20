using System;
using System.Threading.Tasks;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Models;
using Emby.Plugin.M3U.Playlists.Service.ParameterModels;
using MediaBrowser.Model.Logging;

namespace Emby.Plugin.M3U.Playlists.Conversion
{
  /// <summary>
  ///   Enriches the internal playlist with information from emby
  /// </summary>
  /// <seealso cref="IPlaylistEnricher" />
  public class PlaylistEnricher: IPlaylistEnricher
  {
    #region Members

    private readonly ILogger _logger;

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="PlaylistEnricher" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public PlaylistEnricher(ILogger logger)
    {
      _logger = logger;
    }

    #endregion

    #region Interfaces

    /// <inheritdoc />
    public async Task EnrichPlaylistInformation(Playlist playlist, ImportPlaylist request)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}