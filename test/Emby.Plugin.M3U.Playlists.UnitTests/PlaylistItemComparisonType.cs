using System;

namespace Emby.Plugin.M3U.Playlists.UnitTests
{
  /// <summary>
  /// Contains possible properties that can be compared for a playlist item
  /// </summary>
  [Flags]
  public enum PlaylistItemComparisonType
  {
    OriginalLocation = 1,
    Duration = 2,
    Title = 4,
    Full = OriginalLocation | Duration | Title
  }
}