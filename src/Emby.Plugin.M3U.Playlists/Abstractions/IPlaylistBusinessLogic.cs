using System.Threading.Tasks;
using Emby.Plugin.M3U.Playlists.Service.ParameterModels;

namespace Emby.Plugin.M3U.Playlists.Abstractions
{
  /// <summary>
  ///   Interface for the playlist business logic
  /// </summary>
  public interface IPlaylistBusinessLogic
  {
    #region Methods

    /// <summary>
    ///   Imports the playlist for the given request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns></returns>
    Task<bool> ImportPlaylist(ImportPlaylist request);

    #endregion
  }
}