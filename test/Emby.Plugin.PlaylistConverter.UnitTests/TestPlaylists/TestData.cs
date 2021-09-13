using System;
using Emby.Plugin.PlaylistConverter.Models;

namespace Emby.Plugin.PlaylistConverter.UnitTests.TestPlaylists
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
        FullTrackInformation = "Blind Guardian - Imaginations From the Other Side",
        Artist = "Blind Guardian",
        TrackTitle = "Imaginations From the Other Side"
      };

    public static PlaylistItem Inquisition =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(41),
        OriginalLocation = "Medien/Musik/Blind Guardian/Follow the Blind/01 Inquisition.flac",
        FullTrackInformation = "Blind Guardian - Inquisition",
        TrackTitle = "Inquisition",
        Artist = "Blind Guardian",
      };

    public static PlaylistItem ThisWillNeverEnd =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(307),
        OriginalLocation = "Medien/Musik/Blind Guardian/A Twist in the Myth/1-01 This Will Never End.flac",
        FullTrackInformation = "Blind Guardian - This Will Never End",
        TrackTitle = "This Will Never End",
        Artist = "Blind Guardian",
      };

    public static PlaylistItem TimeWhatIsTime =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(345),
        OriginalLocation = "Medien/Musik/Blind Guardian/Somewhere Far Beyound/01 Time What Is Time.flac",
        FullTrackInformation = "Blind Guardian - Time What Is Time",
        TrackTitle = "Time What Is Time",
        Artist = "Blind Guardian",
      };

    public static PlaylistItem TravelerInTime =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(361),
        OriginalLocation = "Medien/Musik/Blind Guardian/Tales from the Twilight World/01 Traveler in Time.flac",
        FullTrackInformation = "Blind Guardian - Traveler in Time",
        Artist = "Blind Guardian",
        TrackTitle = "Traveler in Time"
      };

    public static PlaylistItem Majesty =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(451),
        OriginalLocation = "Medien/Musik/Blind Guardian/Battalions of Fear/01 Majesty.flac",
        FullTrackInformation = "Blind Guardian - Majesty",
        TrackTitle = "Majesty",
        Artist = "Blind Guardian",
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
        FullTrackInformation = "Blind Guardian - Precious Jerusalem",
        TrackTitle = "Precious Jerusalem",
        Artist = "Blind Guardian",
      };

    public static PlaylistItem NinthWave =>
      new PlaylistItem
      {
        Duration = TimeSpan.FromSeconds(569),
        OriginalLocation = "Medien/Musik/Blind Guardian/Beyond the Red Mirror/01 The Ninth Wave.flac",
        FullTrackInformation = "Blind Guardian - The Ninth Wave",
        TrackTitle = "The Ninth Wave",
        Artist = "Blind Guardian",
      };
  }
}
