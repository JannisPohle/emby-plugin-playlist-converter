using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Emby.Plugin.PlaylistConverter.Abstractions;
using Emby.Plugin.PlaylistConverter.Models;
using Emby.Plugin.PlaylistConverter.Service.ParameterModels;
using MediaBrowser.Controller.Playlists;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Playlists;
using Playlist = Emby.Plugin.PlaylistConverter.Models.Playlist;

[assembly: InternalsVisibleTo("Emby.Plugin.PlaylistConverter.UnitTests")]

namespace Emby.Plugin.PlaylistConverter.Conversion
{
  /// <summary>
  ///   Converts playlists
  /// </summary>
  /// <seealso cref="IPlaylistBusinessLogic" />
  public class PlaylistBusinessLogic: IPlaylistBusinessLogic
  {
    #region Members

    private readonly ITargetFormatConverterSelector _formatConverterSelector;
    private readonly ILogger _logger;
    private readonly IPlaylistEnricher _playlistEnricher;
    private readonly IPlaylistManager _playlistManager;

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="PlaylistBusinessLogic" /> class.
    /// </summary>
    /// <param name="formatConverterSelector">The format converter selector.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="playlistEnricher">The playlist enricher.</param>
    /// <param name="playlistManager">The playlist manager.</param>
    public PlaylistBusinessLogic(ITargetFormatConverterSelector formatConverterSelector,
                                 ILogger logger,
                                 IPlaylistEnricher playlistEnricher,
                                 IPlaylistManager playlistManager)
    {
      _formatConverterSelector = formatConverterSelector;
      _logger = logger;
      _playlistEnricher = playlistEnricher;
      _playlistManager = playlistManager;
    }

    #endregion

    #region Methods

    /// <summary>
    ///   Converts the internal playlist to a creation request.
    /// </summary>
    /// <returns></returns>
    internal PlaylistCreationRequest ToCreationRequest(Playlist playlist)
    {
      _logger.Debug("Converting the internal playlist to a playlist creation request...");

      if (string.IsNullOrWhiteSpace(playlist.MediaType))
      {
        throw new ArgumentException("Media type must be set when converting to a playlist creation request");
      }

      if (string.IsNullOrWhiteSpace(playlist.Name))
      {
        _logger.Info("Name for playlist is empty, generating a default name...");
        playlist.Name = $"Imported Playlist - {DateTime.Now}";
      }
      
      var creationRequest = new PlaylistCreationRequest
      {
        MediaType = playlist.MediaType,
        UserId = playlist.UserId.GetValueOrDefault(),
        Name = playlist.Name,
        ItemIdList = playlist.PlaylistItemsFound
                             .Select(item => item.InternalId.Value)
                             .Distinct()
                             .ToArray(),
      };

      if (playlist.PlaylistItemsFound.Count() > creationRequest.ItemIdList.Length)
      {
        var duplicatedItems = playlist.PlaylistItemsFound.GroupBy(item => item.InternalId)
                                      .Where(group => group.Count() > 1)
                                      .Select(group => $"ID {group.Key}: ({string.Join("; ", group.Select(item => item.FullTrackInformation))})");
        _logger.Warn($"Removed all entries but one for items with the same internal id in the playlist creation request. Duplicated items:{Environment.NewLine}{string.Join(Environment.NewLine, duplicatedItems)}");
      }

      _logger.Info($"Converted the internal playlist into a playlist creation request with {creationRequest.ItemIdList.Length} items");

      return creationRequest;
    }

    #endregion

    #region Interfaces

    /// <summary>
    ///   Imports the playlist for the given request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns></returns>
    public async Task<PlaylistImportResult> ImportPlaylist(ImportPlaylist request)
    {
      var result = new PlaylistImportResult();
      _logger.Debug("Starting to import a new playlist...");
      var validationResult = request.Validate();

      if (!validationResult.Success)
      {
        var message = $"Validation of parameters for importing a new playlist failed: {validationResult}";
        _logger.Warn(message);

        result.Success = false;
        result.Message = message;
        return result;
      }

      if (validationResult.ValidationMessages.Any())
      {
        var message = $"Validation of parameters for importing a new playlist succeeded: {validationResult}";
        _logger.Info(message);
      }

      _logger.Info($"Importing playlist with parameters: {request}");
      var converter = _formatConverterSelector.GetConverterForPlaylistFormat(request.PlaylistFormat);
      var playlist = converter.DeserializeFromFile(request.PlaylistData);
      _playlistEnricher.EnrichPlaylistInformation(playlist, request);
      var creationRequest = ToCreationRequest(playlist);
      var creationResult = await _playlistManager.CreatePlaylist(creationRequest);
      _logger.Info($"Imported new playlist {creationResult.Name} (Id: {creationResult.Id})");


      result.Success = true;
      result.Name = creationResult.Name;
      result.PlaylistId = creationResult.Id;
      result.PlaylistItemsFound = playlist.PlaylistItemsFound.Count();
      result.PlaylistItemsTotal = playlist.PlaylistItemsTotal;
      return result;
    }

    #endregion
  }
}