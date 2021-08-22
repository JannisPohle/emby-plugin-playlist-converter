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
  //TODO extract methods into separate file
  public class PlaylistService: IService
  {
    #region Members

    private readonly ILogger _logger;
    private readonly IPlaylistManager _playlistManager;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the format converter selector.
    /// </summary>
    /// <value>
    /// The format converter selector.
    /// </value>
    private ITargetFormatConverterSelector FormatConverterSelector => Plugin.TargetFormatConverterSelector;

    /// <summary>
    /// Gets the playlist enricher.
    /// </summary>
    /// <value>
    /// The playlist enricher.
    /// </value>
    private IPlaylistEnricher PlaylistEnricher => Plugin.PlaylistEnricher;

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="PlaylistService" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="playlistManager">The playlist manager.</param>
    public PlaylistService(ILogger logger, IPlaylistManager playlistManager)
    {
      _logger = logger;
      _playlistManager = playlistManager;
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
        _logger.Debug("Starting to import a new playlist...");
        var validationResult = request.Validate();

        if (!validationResult.Success)
        {
          _logger.Warn($"Validation of parameters for importing a new playlist failed: {validationResult}");

          return false;
        }

        _logger.Info($"Importing playlist with parameters: {request}");
        var converter = FormatConverterSelector.GetConverterForPlaylistFormat(request.PlaylistFormat);
        var playlist = converter.DeserializeFromFile(request.PlaylistData);
        PlaylistEnricher.EnrichPlaylistInformation(playlist, request);
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