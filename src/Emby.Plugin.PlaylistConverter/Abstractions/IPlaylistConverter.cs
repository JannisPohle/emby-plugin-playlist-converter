using Emby.Plugin.PlaylistConverter.Definitions;
using Emby.Plugin.PlaylistConverter.Models;

namespace Emby.Plugin.PlaylistConverter.Abstractions
{
  /// <summary>
  ///   Converts a playlist between a raw file and the internal playlist model
  /// </summary>
  public interface IPlaylistConverter
  {
    #region Properties

    /// <summary>
    ///   Gets the target playlist format for this converter.
    /// </summary>
    /// <value>
    ///   The target playlist format.
    /// </value>
    SupportedPlaylistFormats TargetPlaylistFormat { get; }

    #endregion

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