using Emby.Plugin.M3U.Playlists.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emby.Plugin.M3U.Playlists.UnitTests
{
  /// <summary>
  ///   Contains extensions for the tests
  /// </summary>
  public static class Extensions
  {
    #region Methods

    /// <summary>
    ///   Asserts that the given playlist items are equal.
    ///   Based on the <paramref name="comparisonType" /> only specific properties are compared.
    /// </summary>
    /// <param name="assert">The assert.</param>
    /// <param name="expected">The expected playlist item.</param>
    /// <param name="actual">The actual playlist item.</param>
    /// <param name="comparisonType">The type of the comparison.</param>
    /// <param name="assertForNullIfPropertyNotSpecified">
    ///   Indicates whether properties that are not specified by the
    ///   <paramref name="comparisonType" /> should be asserted to be null
    /// </param>
    public static void PlaylistItemsAreEqual(this Assert assert,
                                             PlaylistItem expected,
                                             PlaylistItem actual,
                                             PlaylistItemComparisonType comparisonType = PlaylistItemComparisonType.Full,
                                             bool assertForNullIfPropertyNotSpecified = true)
    {
      if (expected == null)
      {
        Assert.IsNull(actual);

        return;
      }

      Assert.IsNotNull(actual);

      if (comparisonType.HasFlag(PlaylistItemComparisonType.OriginalLocation))
      {
        Assert.AreEqual(expected.OriginalLocation, actual.OriginalLocation);
      }
      else if (assertForNullIfPropertyNotSpecified)
      {
        Assert.IsNull(actual.OriginalLocation, $"Expected original location to be null, actual value: {actual.OriginalLocation}");
      }

      if (comparisonType.HasFlag(PlaylistItemComparisonType.Duration))
      {
        Assert.AreEqual(expected.Duration, actual.Duration);
      }
      else if (assertForNullIfPropertyNotSpecified)
      {
        Assert.IsNull(actual.Duration, $"Expected duration to be null, actual value: {actual.Duration}");
      }

      if (comparisonType.HasFlag(PlaylistItemComparisonType.Title))
      {
        Assert.AreEqual(expected.Title, actual.Title);
      }
      else if (assertForNullIfPropertyNotSpecified)
      {
        Assert.IsNull(actual.Title, $"Expected title to be null, actual value: {actual.Title}");
      }
    }

    #endregion
  }
}