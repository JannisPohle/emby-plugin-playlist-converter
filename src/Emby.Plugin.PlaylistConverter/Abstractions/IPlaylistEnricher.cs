using Emby.Plugin.PlaylistConverter.Models;
using Emby.Plugin.PlaylistConverter.Service.ParameterModels;

namespace Emby.Plugin.PlaylistConverter.Abstractions
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
    void EnrichPlaylistInformation(Playlist playlist, ImportPlaylist request);

    #endregion
  }
}