﻿using Emby.Plugin.M3U.Playlists.Models;

namespace Emby.Plugin.M3U.Playlists.Abstractions
{
  /// <summary>
  ///   Converts a playlist between a raw file and the internal playlist model
  /// </summary>
  public interface IPlaylistConverter
  {
    #region Methods

    /// <summary>
    ///   Deserializes the playlist from raw file content.
    /// </summary>
    /// <param name="rawFileContent">Content of the raw file.</param>
    /// <returns></returns>
    Playlist DeserializeFromFile(byte[] rawFileContent);

    /// <summary>
    ///   Serializes the internal playlist model into raw file content.
    /// </summary>
    /// <param name="playlist">The playlist.</param>
    /// <returns></returns>
    byte[] SerializeToFile(Playlist playlist);

    #endregion
  }
}