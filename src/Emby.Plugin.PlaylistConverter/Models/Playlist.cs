using System.Collections.Generic;
using System.Linq;

namespace Emby.Plugin.PlaylistConverter.Models
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

    /// <summary>
    ///   Gets the total number of playlist items in this playlist.
    /// </summary>
    /// <value>
    ///   The playlist items total.
    /// </value>
    public int PlaylistItemsTotal => PlaylistItems.Count;

    /// <summary>
    ///   Gets all playlist items that were found in emby.
    /// </summary>
    /// <value>
    ///   The playlist items that were found.
    /// </value>
    public IEnumerable<PlaylistItem> PlaylistItemsFound => PlaylistItems.Where(item => item.Found);

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