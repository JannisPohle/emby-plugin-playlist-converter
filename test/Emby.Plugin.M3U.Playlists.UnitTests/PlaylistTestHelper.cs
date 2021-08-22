﻿using System;
using System.Collections.Generic;
using System.Linq;
using Emby.Plugin.M3U.Playlists.Models;
using Emby.Plugin.M3U.Playlists.Service.ParameterModels;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Querying;

namespace Emby.Plugin.M3U.Playlists.UnitTests
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
    ///   Creates a valid import playlist with the given values.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="mediaType">Type of the media.</param>
    /// <param name="playlistName">Name of the playlist.</param>
    /// <returns></returns>
    public static ImportPlaylist CreateImportPlaylist(string userId = USER_ID_1,
                                                      string mediaType = MediaType.Audio,
                                                      string playlistName = PLAYLIST_NAME_1)
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = Guid.Parse(userId),
        MediaType = mediaType,
        PlaylistName = playlistName
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

    #endregion
  }
}