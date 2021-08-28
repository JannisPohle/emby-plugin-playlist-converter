namespace Emby.Plugin.M3U.Playlists.Models
{
  /// <summary>
  ///   Represents the result of the playlist import
  /// </summary>
  public class PlaylistImportResult
  {
    #region Properties

    /// <summary>
    ///   Gets or sets a value indicating whether importing the playlist was a success.
    /// </summary>
    /// <value>
    ///   <c>true</c> if success; otherwise, <c>false</c>.
    /// </value>
    public bool Success { get; set; }

    /// <summary>
    ///   Gets or sets a message, in case importing was unsuccessful.
    /// </summary>
    /// <value>
    ///   The message.
    /// </value>
    public string Message { get; set; }

    /// <summary>
    ///   Gets or sets the name of the imported playlist.
    /// </summary>
    /// <value>
    ///   The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    ///   Gets or sets the identifier of the imported playlist.
    /// </summary>
    /// <value>
    ///   The playlist identifier.
    /// </value>
    public string PlaylistId { get; set; }

    #endregion
  }
}