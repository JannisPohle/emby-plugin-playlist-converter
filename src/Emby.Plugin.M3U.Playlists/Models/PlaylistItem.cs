using System;

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

    /// <summary>
    ///   Gets or sets the duration of the playlist item.
    /// </summary>
    /// <value>
    ///   The duration.
    /// </value>
    public TimeSpan? Duration { get; set; }

    /// <summary>
    ///   Gets or sets the title.
    /// </summary>
    /// <value>
    ///   The title.
    /// </value>
    public string Title { get; set; }

    /// <summary>
    ///   Gets or sets the location from the original playlist.
    /// </summary>
    /// <value>
    ///   The location.
    /// </value>
    public string OriginalLocation { get; set; }

    /// <summary>
    /// Gets or sets the internal identifier.
    /// </summary>
    /// <value>
    /// The internal identifier.
    /// </value>
    public Guid? InternalId { get; set; }

    #endregion

    #region Methods

    #region Overrides of Object

    /// <inheritdoc />
    public override string ToString()
    {
      return $"{nameof(Title)}: {Title}; {nameof(OriginalLocation)}: {OriginalLocation}; {nameof(Duration)}: {Duration}; {nameof(Found)}: {Found}";
    }

    #endregion

    #endregion
  }
}