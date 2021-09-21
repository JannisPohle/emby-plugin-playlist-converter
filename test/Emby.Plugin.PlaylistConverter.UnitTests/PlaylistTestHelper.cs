using System;
using System.Collections.Generic;
using System.Linq;
using Emby.Plugin.PlaylistConverter.Definitions;
using Emby.Plugin.PlaylistConverter.Models;
using Emby.Plugin.PlaylistConverter.Service.ParameterModels;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Querying;

namespace Emby.Plugin.PlaylistConverter.UnitTests
{
  public static class PlaylistTestHelper
  {
    #region Static

    public const string PLAYLIST_NAME_1 = "Foo";
    public const string PLAYLIST_NAME_2 = "Bar";

    public const string USER_ID_1 = "6fb6eb28-aeab-4f6d-a1fc-f19141036bd0";
    public const string USER_ID_2 = "b63cc3fb-b138-476a-8906-d2b9f61c6499";

    public const string BASE_ITEM_ID_1 = "e1d040b5-46e2-4741-a8d5-ce376b9fc077";
    public const string BASE_ITEM_ID_2 = "59d19cf0-ce8e-43c7-93c5-bf1c18054ed4";
    public const string BASE_ITEM_ID_3 = "03547649-cfc8-46ad-9cce-486dd43d8279";
    public const string BASE_ITEM_ID_4 = "3728414e-80eb-45c5-af13-45791c233122";

    #endregion

    #region Methods

    /// <summary>
    /// Creates a valid import playlist with the given values.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="mediaType">Type of the media.</param>
    /// <param name="playlistName">Name of the playlist.</param>
    /// <param name="playlistFormat">The playlist format.</param>
    /// <param name="playlistData">The playlist data.</param>
    /// <returns></returns>
    public static ImportPlaylist CreateImportPlaylist(string userId = USER_ID_1,
                                                      string mediaType = MediaType.Audio,
                                                      string playlistName = PLAYLIST_NAME_1,
                                                      SupportedPlaylistFormats? playlistFormat = SupportedPlaylistFormats.M3U,
                                                      byte[] playlistData = null)
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = Guid.Parse(userId),
        MediaType = mediaType,
        PlaylistName = playlistName,
        PlaylistFormat = playlistFormat?.ToString(),
        PlaylistData = playlistData
      };

      return importPlaylist;
    }

    /// <summary>
    ///   Creates an empty playlist with an empty list of playlist items.
    /// </summary>
    /// <returns></returns>
    public static Playlist CreateEmptyPlaylist()
    {
      return new Playlist
      {
        PlaylistItems = new List<PlaylistItem>()
      };
    }

    /// <summary>
    /// Creates the default playlist with the given values.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="mediaType">Type of the media.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns></returns>
    public static Playlist CreateDefaultPlaylist(string name = PLAYLIST_NAME_1, string mediaType = MediaType.Audio, string userId = USER_ID_1)
    {
      return CreateEmptyPlaylist()
             .AddName(name)
             .AddMediaType(mediaType)
             .AddUserId(userId);
    }

    /// <summary>
    /// Adds a name to the playlist and returns the playlist.
    /// </summary>
    /// <param name="playlist">The playlist.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public static Playlist AddName(this Playlist playlist, string name = PLAYLIST_NAME_1)
    {
      playlist.Name = name;

      return playlist;
    }


    /// <summary>
    /// Adds the media type to the playlist and returns the playlist.
    /// </summary>
    /// <param name="playlist">The playlist.</param>
    /// <param name="mediaType">Type of the media.</param>
    /// <returns></returns>
    public static Playlist AddMediaType(this Playlist playlist, string mediaType = MediaType.Audio)
    {
      playlist.MediaType = mediaType;

      return playlist;
    }

    /// <summary>
    /// Adds the user identifier to the playlist and returns the playlist.
    /// </summary>
    /// <param name="playlist">The playlist.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns></returns>
    public static Playlist AddUserId(this Playlist playlist, string userId = USER_ID_1)
    {
      playlist.UserId = userId?.GetHashCode();

      return playlist;
    }

    /// <summary>
    /// Adds the playlist item to the playlist and returns the playlist
    /// </summary>
    /// <param name="playlist">The playlist.</param>
    /// <param name="item">The playlist item to be added</param>
    /// <returns></returns>
    public static Playlist AddPlaylistItem(this Playlist playlist, PlaylistItem item)
    {
      playlist.PlaylistItems.Add(item);

      return playlist;
    }

    /// <summary>
    ///   Creates the query result with an audio item for each given identifier.
    /// </summary>
    /// <param name="itemIds">The item ids.</param>
    /// <returns></returns>
    public static QueryResult<BaseItem> CreateQueryResult(params string[] itemIds)
    {
      var items = itemIds.Select(id => new Audio { Id = Guid.Parse(id) }).ToArray<BaseItem>();

      var result = new QueryResult<BaseItem>
      {
        Items = items
      };

      return result;
    }

    /// <summary>
    /// Adds the internal identifier to the playlist item.
    /// </summary>
    /// <param name="playlistItem">The playlist item.</param>
    /// <param name="internalId">The internal identifier.</param>
    /// <returns></returns>
    public static PlaylistItem AddInternalId(this PlaylistItem playlistItem, long? internalId = null)
    {
      playlistItem.InternalId = internalId ?? Guid.NewGuid().GetHashCode();

      return playlistItem;
    }

    /// <summary>
    /// Sets found to the given value on the playlist item
    /// </summary>
    /// <param name="playlistItem">The playlist item.</param>
    /// <param name="found">Found property of the playlist item is set to this value</param>
    /// <returns></returns>
    public static PlaylistItem SetFound(this PlaylistItem playlistItem, bool found = true)
    {
      playlistItem.Found = found;

      return playlistItem;
    }

    /// <summary>
    /// Sets the full track information on the playlist item
    /// </summary>
    /// <param name="playlistItem">The playlist item.</param>
    /// <param name="fullTrackInformation">Full track information of the playlist item is set to this value</param>
    /// <returns></returns>
    public static PlaylistItem SetFullTrackInformation(this PlaylistItem playlistItem, string fullTrackInformation)
    {
      playlistItem.FullTrackInformation = fullTrackInformation;

      return playlistItem;
    }

    /// <summary>
    /// Sets the track title on the playlist item
    /// </summary>
    /// <param name="playlistItem">The playlist item.</param>
    /// <param name="title">The track title of the playlist item is set to this value</param>
    /// <returns></returns>
    public static PlaylistItem SetTrackTitle(this PlaylistItem playlistItem, string title)
    {
      playlistItem.TrackTitle = title;

      return playlistItem;
    }

    #endregion
  }
}