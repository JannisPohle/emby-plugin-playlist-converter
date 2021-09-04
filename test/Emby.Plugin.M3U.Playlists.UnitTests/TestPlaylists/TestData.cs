using System;
using Emby.Plugin.M3U.Playlists.Models;

namespace Emby.Plugin.M3U.Playlists.UnitTests.TestPlaylists
{
  /// <summary>
  /// Contains the valid items from the test data for assertions
  /// </summary>
  public static class TestData
  {
    public static PlaylistItem ImaginationsFromTheOtherSide =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(438),
        OriginalLocation = "Medien/Musik/Blind Guardian/Imaginations From the Other Side/01 Imaginations From the Other Side.flac",
        Title = "Blind Guardian - Imaginations From the Other Side"
      };

    public static PlaylistItem Inquisition =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(41),
        OriginalLocation = "Medien/Musik/Blind Guardian/Follow the Blind/01 Inquisition.flac",
        Title = "Blind Guardian - Inquisition"
      };

    public static PlaylistItem ThisWillNeverEnd =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(307),
        OriginalLocation = "Medien/Musik/Blind Guardian/A Twist in the Myth/1-01 This Will Never End.flac",
        Title = "Blind Guardian - This Will Never End"
      };

    public static PlaylistItem TimeWhatIsTime =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(345),
        OriginalLocation = "Medien/Musik/Blind Guardian/Somewhere Far Beyound/01 Time What Is Time.flac",
        Title = "Blind Guardian - Time What Is Time"
      };

    public static PlaylistItem TravelerInTime =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(361),
        OriginalLocation = "Medien/Musik/Blind Guardian/Tales from the Twilight World/01 Traveler in Time.flac",
        Title = "Blind Guardian - Traveler in Time"
      };

    public static PlaylistItem Majesty =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(451),
        OriginalLocation = "Medien/Musik/Blind Guardian/Battalions of Fear/01 Majesty.flac",
        Title = "Blind Guardian - Majesty"
      };

    public static PlaylistItem MrSandman =>
      new PlaylistItem
      {
        OriginalLocation = "Medien/Musik/Blind Guardian/The forgotten Tales/01 Mr. Sandman.flac"
      };

    public static PlaylistItem PreciousJerusalem =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(382),
        OriginalLocation = "Medien/Musik/Blind Guardian/A Night at the Opera/01 Precious Jerusalem.flac",
        Title = "Blind Guardian - Precious Jerusalem"
      };

    public static PlaylistItem NinthWave =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(569),
        OriginalLocation = "Medien/Musik/Blind Guardian/Beyond the Red Mirror/01 The Ninth Wave.flac",
        Title = "Blind Guardian - The Ninth Wave"
      };

    /// <summary>
    /// Adds the internal identifier to the playlist item.
    /// </summary>
    /// <param name="playlistItem">The playlist item.</param>
    /// <param name="internalId">The internal identifier.</param>
    /// <returns></returns>
    public static PlaylistItem AddInternalId(this PlaylistItem playlistItem, long? internalId = null)
    {
      playlistItem.InternalId = internalId ?? Guid.NewGuid().GetHashCode();

      return playlistItem;
    }

    /// <summary>
    /// Sets found to the given value on the playlist item
    /// </summary>
    /// <param name="playlistItem">The playlist item.</param>
    /// <param name="found">Found property of the playlist item is set to this value</param>
    /// <returns></returns>
    public static PlaylistItem SetFound(this PlaylistItem playlistItem, bool found = true)
    {
      playlistItem.Found = found;

      return playlistItem;
    }
  }
}
