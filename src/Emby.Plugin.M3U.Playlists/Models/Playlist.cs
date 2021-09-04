using System.Collections.Generic;

namespace Emby.Plugin.M3U.Playlists.Models
{
  /// <summary>
  ///   Represents an internal playlist
  /// </summary>
  public class Playlist
  {
    #region Properties

    /// <summary>
    ///   Gets or sets the playlist name.
    /// </summary>
    /// <value>
    ///   The playlist name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    ///   Gets or sets the type of media that is contained in the playlist.
    /// </summary>
    /// <value>
    ///   The type of the media.
    /// </value>
    public string MediaType { get; set; }

    /// <summary>
    ///   Gets or sets the playlist items.
    /// </summary>
    /// <value>
    ///   The playlist items.
    /// </value>
    public IList<PlaylistItem> PlaylistItems { get; set; }

    /// <summary>
    ///   Gets or sets the user identifier that owns the playlist.
    /// </summary>
    /// <value>
    ///   The user identifier.
    /// </value>
    public long? UserId { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="Playlist" /> class.
    /// </summary>
    public Playlist()
    {
      PlaylistItems = new List<PlaylistItem>();
    }

    #endregion

    #region Methods

    #region Overrides of Object

    /// <inheritdoc />
    public override string ToString()
    {
      return
        $"{nameof(Name)}: {Name}; {nameof(MediaType)}: {MediaType}; {nameof(UserId)}: {UserId}; {nameof(PlaylistItems)} (count): {PlaylistItems?.Count}";
    }

    #endregion

    #endregion
  }
}