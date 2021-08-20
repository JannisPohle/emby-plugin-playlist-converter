using System;
using System.Threading.Tasks;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Service.ParameterModels;
using MediaBrowser.Controller.Playlists;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Services;

namespace Emby.Plugin.M3U.Playlists.Service
{
  /// <summary>
  ///   The service that exposes a Restful api for managing M3U playlists
  /// </summary>
  /// <seealso cref="IService" />
  //TODO use ResultFactory
  public class PlaylistService: IService
  {
    #region Members

    private ITargetFormatConverterSelector _formatConverterSelector;
    private ILogger _logger;
    private IPlaylistEnricher _playlistEnricher;
    private IPlaylistManager _playlistManager;

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
        _logger.Debug("Starting to import a new playlist...");
        var validationResult = request.Validate();

        if (!validationResult.Success)
        {
          _logger.Warn($"Validation of parameters for importing a new playlist failed: {validationResult}");

          return false;
        }

        _logger.Info($"Importing playlist with parameters: {request}");
        var converter = _formatConverterSelector.GetConverterForPlaylistFormat(request.PlaylistFormat);
        var playlist = converter.DeserializeFromFile(request.PlaylistData);
        await _playlistEnricher.EnrichPlaylistInformation(playlist, request);
        var creationRequest = playlist.ToCreationRequest();
        var creationResult = await _playlistManager.CreatePlaylist(creationRequest);
        _logger.Info($"Imported new playlist {creationResult.Name} (Id: {creationResult.Id})");

        return true;
      }
      catch (Exception ex)
      {
        _logger.ErrorException($"Failed to import playlist with parameters: {request}", ex);

        return false;
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