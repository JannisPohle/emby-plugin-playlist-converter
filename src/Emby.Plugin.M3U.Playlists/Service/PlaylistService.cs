using System;
using System.Threading.Tasks;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Models;
using Emby.Plugin.M3U.Playlists.Service.ParameterModels;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Services;

namespace Emby.Plugin.M3U.Playlists.Service
{
  /// <summary>
  ///   The service that exposes a Restful api for managing M3U playlists
  /// </summary>
  /// <seealso cref="IService" />
  public class PlaylistService: IService
  {
    #region Members

    private readonly ILogger _logger;

    #endregion

    #region Properties

    private IPlaylistBusinessLogic BusinessLogic => Plugin.PlaylistBusinessLogic;

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="PlaylistService" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public PlaylistService(ILogger logger)
    {
      _logger = logger;
    }

    #endregion

    #region Methods

    /// <summary>
    ///   Accepts a request to import a playlist
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns></returns>
    public async Task<object> Post(ImportPlaylist request)
    {
      try
      {
        return await BusinessLogic.ImportPlaylist(request);
      }
      catch (Exception ex)
      {
        _logger.ErrorException($"Failed to import playlist with parameters: {request}", ex);

        return new PlaylistImportResult()
        {
          Success = false,
          Message = $"Failed to import the playlist: {ex.Message}"
        };
      }
    }

    /// <summary>
    ///   Accepts a request to export a playlist
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns></returns>
    public async Task<object> Get(ExportPlaylist request)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}