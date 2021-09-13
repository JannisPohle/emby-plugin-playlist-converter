using System.Threading.Tasks;
using Emby.Plugin.PlaylistConverter.Models;
using Emby.Plugin.PlaylistConverter.Service.ParameterModels;

namespace Emby.Plugin.PlaylistConverter.Abstractions
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
    Task<PlaylistImportResult> ImportPlaylist(ImportPlaylist request);

    #endregion
  }
}