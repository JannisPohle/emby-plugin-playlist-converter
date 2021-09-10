using System;
using System.IO;
using System.Linq;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Models;
using Emby.Plugin.M3U.Playlists.Service.ParameterModels;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
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

    private readonly ILibraryManager _libraryManager;
    private readonly ILogger _logger;

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="PlaylistEnricher" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="libraryManager">The library manager</param>
    public PlaylistEnricher(ILogger logger, ILibraryManager libraryManager)
    {
      _logger = logger;
      _libraryManager = libraryManager;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Finds the matching media item.
    /// </summary>
    /// <param name="playlistItem">The playlist item.</param>
    /// <param name="requestMediaType">Type of the request media.</param>
    /// <returns></returns>
    private BaseItem FindMatchingMediaItem(PlaylistItem playlistItem, string requestMediaType)
    {
      var searchTerm = GetSearchTerm(playlistItem);
      var artists = GetArtists(playlistItem);
      var mediaTypes = new[] { requestMediaType };
      
      var query = new InternalItemsQuery
      {
        Name = searchTerm,
        MediaTypes = mediaTypes,
        ArtistIds = artists
      };
      _logger.Debug($"Trying to find a matching media item by name {searchTerm} and media type {requestMediaType}");
      var result = _libraryManager.QueryItems(query);

      if (result?.Items?.Any() ?? false)
      {
        return result.Items.First();
      }

      //Try using the title as a search term as a fallback
      query = new InternalItemsQuery
      {
        SearchTerm = searchTerm,
        MediaTypes = mediaTypes,
        ArtistIds = artists
      };
      _logger.Debug("Found no matching item by name, trying to find a media item by using the name as a search term");
      result = _libraryManager.QueryItems(query);

      return result?.Items?.FirstOrDefault();
    }

    /// <summary>
    /// Gets the artists for the given playlist item.
    /// </summary>
    /// <param name="playlistItem">The playlist item.</param>
    /// <returns></returns>
    private long[] GetArtists(PlaylistItem playlistItem)
    {
      if (string.IsNullOrWhiteSpace(playlistItem.Artist))
      {
        _logger.Debug("Artist is not set for playlist item, query for media item will be performed without the artist filter");
        return null;
      }

      var query = new InternalItemsQuery()
      {
        Name = playlistItem.Artist
      };
      var artists = _libraryManager.GetArtists(query);

      if (artists.TotalRecordCount == 0)
      {
        _logger.Warn($"Found no artist for name {playlistItem.Artist}, query for media item will be performed without the artist filter");
        return null;
      }

      _logger.Debug($"Found {artists.TotalRecordCount} artists for name {playlistItem.Artist}: {string.Join(", ", artists.Items.Select(artist => artist.Item1.Name))}");

      return artists.Items.Select(artist => _libraryManager.GetInternalId(artist.Item1.Id)).ToArray();
    }

    /// <summary>
    /// Gets the search term for the playlist item.
    /// </summary>
    /// <param name="playlistItem">The playlist item.</param>
    /// <returns></returns>
    private string GetSearchTerm(PlaylistItem playlistItem)
    {
      var searchTerm = playlistItem.TrackTitle ?? playlistItem.FullTrackInformation;

      if (string.IsNullOrWhiteSpace(searchTerm))
      {
        searchTerm = Path.GetFileNameWithoutExtension(playlistItem.OriginalLocation);
        _logger.Debug($"Title of playlist item is empty, using file name as a fallback: {searchTerm}");
      }

      return searchTerm;
    }

    #endregion

    #region Interfaces

    /// <inheritdoc />
    public void EnrichPlaylistInformation(Playlist playlist, ImportPlaylist request)
    {
      if (playlist == null)
      {
        throw new ArgumentNullException(nameof(playlist), "Playlist cannot be null for enrichment");
      }

      if (playlist.PlaylistItems == null)
      {
        throw new ArgumentNullException(nameof(playlist), "Playlist items can not be null for enrichment");
      }

      if (request == null)
      {
        throw new ArgumentNullException(nameof(request), "Request for importing the playlist can not be null for enrichment");
      }

      _logger.Debug("Starting to enrich the playlist information for importing...");
      playlist.Name = request.PlaylistName;
      playlist.MediaType = request.MediaType;
      playlist.UserId = _libraryManager.GetInternalId(request.UserId?.ToString());

      foreach (var playlistItem in playlist.PlaylistItems)
      {
        var matchingItem = FindMatchingMediaItem(playlistItem, request.MediaType);

        if (matchingItem == null)
        {
          _logger.Warn($"Found no matching media item for imported playlist item {playlistItem}. Item will be ignored for the playlist creation");
          playlistItem.Found = false;

          continue;
        }

        playlistItem.Found = true;
        playlistItem.InternalId = _libraryManager.GetInternalId(matchingItem.Id);
        _logger.Debug($"Found a matching media item for imported playlist item {playlistItem}: {matchingItem} (Id: {matchingItem.Id})");
      }
      
      _logger.Info($"Found matching media items for {playlist.PlaylistItemsFound.Count()} items in the imported playlist out of {playlist.PlaylistItemsTotal} total imported playlist items");
    }

    #endregion
  }
}