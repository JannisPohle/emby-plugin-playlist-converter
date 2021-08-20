using System;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Models;

namespace Emby.Plugin.M3U.Playlists.Conversion
{
  /// <summary>
  ///   Converter for the m3u playlist format
  /// </summary>
  public class M3UPlaylistConverter: IPlaylistConverter
  {
    #region Interfaces

    /// <inheritdoc />
    public Playlist DeserializeFromFile(byte[] rawFileContent)
    {
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public byte[] SerializeToFile(Playlist playlist)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}