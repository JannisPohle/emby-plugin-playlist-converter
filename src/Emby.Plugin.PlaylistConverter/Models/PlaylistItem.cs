using System;

namespace Emby.Plugin.PlaylistConverter.Models
{
  /// <summary>
  ///   Represents a single item in the playlist
  /// </summary>
  public class PlaylistItem
  {
    #region Members

    private bool _found;

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets a value indicating whether this <see cref="PlaylistItem" /> was found in emby.
    /// </summary>
    /// <value>
    ///   <c>true</c> if found; otherwise, <c>false</c>.
    /// </value>
    public bool Found
    {
      get => _found && InternalId.HasValue;
      set => _found = value;
    }

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
    public string FullTrackInformation { get; set; }

    /// <summary>
    ///   Gets or sets the artist.
    /// </summary>
    /// <value>
    ///   The artist.
    /// </value>
    public string Artist { get; set; }

    /// <summary>
    ///   Gets or sets the track title.
    /// </summary>
    /// <value>
    ///   The track title.
    /// </value>
    public string TrackTitle { get; set; }

    /// <summary>
    ///   Gets or sets the location from the original playlist.
    /// </summary>
    /// <value>
    ///   The location.
    /// </value>
    public string OriginalLocation { get; set; }

    /// <summary>
    ///   Gets or sets the internal identifier.
    /// </summary>
    /// <value>
    ///   The internal identifier.
    /// </value>
    public long? InternalId { get; set; }

    #endregion

    #region Methods

    #region Overrides of Object

    /// <inheritdoc />
    public override string ToString()
    {
      return $"{nameof(FullTrackInformation)}: {FullTrackInformation}; {nameof(OriginalLocation)}: {OriginalLocation}; {nameof(Duration)}: {Duration}; {nameof(Found)}: {Found}; {nameof(Artist)}: {Artist}; {nameof(TrackTitle)}: {TrackTitle}";
    }

    #endregion

    #endregion
  }
}