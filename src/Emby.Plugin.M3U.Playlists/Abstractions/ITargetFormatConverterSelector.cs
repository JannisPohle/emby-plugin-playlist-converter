namespace Emby.Plugin.M3U.Playlists.Abstractions
{
  /// <summary>
  ///   Selects the correct playlist converter based on the given playlist format
  /// </summary>
  public interface ITargetFormatConverterSelector
  {
    #region Methods

    /// <summary>
    ///   Gets the correct converter for the given playlist format.
    /// </summary>
    /// <param name="playlistFormat">The playlist format.</param>
    /// <returns></returns>
    IPlaylistConverter GetConverterForPlaylistFormat(string playlistFormat);

    #endregion
  }
}