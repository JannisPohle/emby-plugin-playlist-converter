using System;

namespace Emby.Plugin.PlaylistConverter.UnitTests
{
  /// <summary>
  /// Contains possible properties that can be compared for a playlist item
  /// </summary>
  [Flags]
  public enum PlaylistItemComparisonType
  {
    OriginalLocation = 1,
    Duration = 2,
    FullTrackInformation = 4,
    Artist = 8,
    TrackTitle = 16,
    Full = OriginalLocation | Duration | FullTrackInformation | Artist | TrackTitle
  }
}