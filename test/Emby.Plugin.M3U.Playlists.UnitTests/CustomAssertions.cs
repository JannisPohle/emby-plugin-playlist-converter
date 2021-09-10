using System;
using System.Collections.Generic;
using System.Linq;
using Emby.Plugin.M3U.Playlists.Models;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Playlists;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emby.Plugin.M3U.Playlists.UnitTests
{
  /// <summary>
  ///   Contains extensions for the tests
  /// </summary>
  public static class CustomAssertions
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

      AssertPropertyByComparisonType(expected.OriginalLocation, actual.OriginalLocation, comparisonType, PlaylistItemComparisonType.OriginalLocation, assertForNullIfPropertyNotSpecified);
      AssertPropertyByComparisonType(expected.Duration, actual.Duration, comparisonType, PlaylistItemComparisonType.Duration, assertForNullIfPropertyNotSpecified);
      AssertPropertyByComparisonType(expected.FullTrackInformation, actual.FullTrackInformation, comparisonType, PlaylistItemComparisonType.FullTrackInformation, assertForNullIfPropertyNotSpecified);
      AssertPropertyByComparisonType(expected.Artist, actual.Artist, comparisonType, PlaylistItemComparisonType.Artist, assertForNullIfPropertyNotSpecified);
      AssertPropertyByComparisonType(expected.TrackTitle, actual.TrackTitle, comparisonType, PlaylistItemComparisonType.TrackTitle, assertForNullIfPropertyNotSpecified);
    }

    private static void AssertPropertyByComparisonType(object expected,
                                                       object actual,
                                                       PlaylistItemComparisonType comparisonType,
                                                       PlaylistItemComparisonType flag,
                                                       bool assertForNullIfPropertyNotSpecified)
    {
      if (comparisonType.HasFlag(flag))
      {
        Assert.AreEqual(expected, actual);
      }
      else if (assertForNullIfPropertyNotSpecified)
      {
        Assert.IsNull(actual, $"Expected property to be null, actual value: {actual}");
      }
    }

    /// <summary>
    ///   Asserts that the playlist is equal to the given values.
    /// </summary>
    /// <param name="assert">The assert.</param>
    /// <param name="actualPlaylist">The actual playlist.</param>
    /// <param name="expectedName">The expected name.</param>
    /// <param name="expectedUserId">The expected user identifier.</param>
    /// <param name="expectedMediaType">Expected type of the media.</param>
    /// <param name="expectedPlaylistItemCount">The expected playlist item count.</param>
    /// <param name="expectedPlaylistItemsFoundCount">The expected number of playlist items that were found in emby</param>
    public static void PlaylistsAreEqual(this Assert assert,
                                         Playlist actualPlaylist,
                                         string expectedName = PlaylistTestHelper.PLAYLIST_NAME_1,
                                         string expectedUserId = PlaylistTestHelper.USER_ID_1,
                                         string expectedMediaType = MediaType.Audio,
                                         int expectedPlaylistItemCount = 0,
                                         int expectedPlaylistItemsFoundCount = 0)
    {
      Assert.AreEqual(expectedMediaType, actualPlaylist.MediaType);
      Assert.AreEqual(expectedName, actualPlaylist.Name);
      Assert.AreEqual(expectedUserId.GetHashCode(), actualPlaylist.UserId);
      Assert.AreEqual(expectedPlaylistItemCount, actualPlaylist.PlaylistItemsTotal);
      Assert.AreEqual(expectedPlaylistItemsFoundCount, actualPlaylist.PlaylistItemsFound.Count());
    }

    /// <summary>
    /// Asserts that the creation request is equal to the given values
    /// </summary>
    /// <param name="assert">The assert.</param>
    /// <param name="actualCreationRequest">The actual creation request.</param>
    /// <param name="expectedMediaType">Expected type of the media.</param>
    /// <param name="expectedName">The expected name.</param>
    /// <param name="expectedUserId">The expected user identifier.</param>
    /// <param name="expectedItemIds">The expected item ids.</param>
    /// <param name="expectAutogeneratedName">if set to <c>true</c> the playlist name is expected to be autogenerated and start with "Imported Playlist". The <paramref name="expectedName"/> param is not evaluated in this case</param>
    public static void PlaylistCreateResultsAreEqual(this Assert assert,
                                                     PlaylistCreationRequest actualCreationRequest,
                                                     string expectedMediaType = MediaType.Audio,
                                                     string expectedName = PlaylistTestHelper.PLAYLIST_NAME_1,
                                                     string expectedUserId = PlaylistTestHelper.USER_ID_1,
                                                     IEnumerable<long> expectedItemIds = null,
                                                     bool expectAutogeneratedName = false)
    {
      expectedItemIds ??= Enumerable.Empty<long>();
      Assert.IsNotNull(actualCreationRequest);
      Assert.AreEqual(expectedMediaType, actualCreationRequest.MediaType);

      if (!expectAutogeneratedName)
      {
        Assert.AreEqual(expectedName, actualCreationRequest.Name);
      }
      else
      {
        Assert.IsFalse(string.IsNullOrWhiteSpace(actualCreationRequest.Name));
        Assert.IsTrue(actualCreationRequest.Name.StartsWith("Imported Playlist"));
      }

      Assert.AreEqual(expectedUserId.GetHashCode(), actualCreationRequest.UserId);
      Assert.AreEqual(expectedItemIds.Count(), actualCreationRequest.ItemIdList.Length);
      Assert.IsTrue(expectedItemIds.All(id => actualCreationRequest.ItemIdList.Contains(id)));
    }

    #endregion
  }
}