namespace Emby.Plugin.M3U.Playlists.Models
{
  /// <summary>
  ///   Represents a single item in the playlist
  /// </summary>
  public class PlaylistItem
  {
    #region Properties

    /// <summary>
    ///   Gets or sets a value indicating whether this <see cref="PlaylistItem" /> was found in emby.
    /// </summary>
    /// <value>
    ///   <c>true</c> if found; otherwise, <c>false</c>.
    /// </value>
    public bool Found { get; set; }

    #endregion

    #region Methods

    #region Overrides of Object

    /// <inheritdoc />
    public override string ToString()
    {
      //TODO implement
      return base.ToString();
    }

    #endregion

    #endregion
  }
}