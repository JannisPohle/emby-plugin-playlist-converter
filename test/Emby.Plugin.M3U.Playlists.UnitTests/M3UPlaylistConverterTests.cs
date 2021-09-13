using System;
using System.IO;
using System.Linq;
using Emby.Plugin.M3U.Playlists.Conversion;
using Emby.Plugin.M3U.Playlists.UnitTests.TestPlaylists;
using MediaBrowser.Model.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Emby.Plugin.M3U.Playlists.UnitTests
{
  [TestClass]
  public class M3UPlaylistConverterTests
  {
    private Mock<ILogger> _loggerMock;
    private M3UPlaylistConverter _converter;

    [TestInitialize]
    public void Initialize()
    {
      _loggerMock = new Mock<ILogger>();

      _converter = new M3UPlaylistConverter(_loggerMock.Object);
    }

    /// <summary>
    /// The file contains a single valid entry, including the additional information of title and duration
    /// </summary>
    [TestMethod]
    [DataRow("UTF8")]
    [DataRow("UTF8-BOM")]
    [DataRow("UTF16-BE")]
    [DataRow("UTF16-LE")]
    public void ConvertFromFile_SingleEntry_Valid_OK(string format)
    {
      var fileContent = File.ReadAllBytes($"TestPlaylists/{format}/Valid_SingleEntry.m3u");

      var playlist = _converter.DeserializeFromFile(fileContent);

      Assert.IsNotNull(playlist);
      Assert.IsTrue(string.IsNullOrWhiteSpace(playlist.Name));
      Assert.IsTrue(string.IsNullOrWhiteSpace(playlist.MediaType));
      Assert.IsNull(playlist.UserId);
      Assert.IsNotNull(playlist.PlaylistItems);
      Assert.AreEqual(1, playlist.PlaylistItems.Count);

      Assert.That.PlaylistItemsAreEqual(TestData.TravelerInTime, playlist.PlaylistItems.First());
    }

    /// <summary>
    /// The file contains multiple valid entries, including the additional information of title and duration
    /// </summary>
    [TestMethod]
    [DataRow("UTF8")]
    [DataRow("UTF8-BOM")]
    [DataRow("UTF16-BE")]
    [DataRow("UTF16-LE")]
    public void ConvertFromFile_MultipleEntries_Valid_OK(string format)
    {
      var fileContent = File.ReadAllBytes($"TestPlaylists/{format}/Valid_MultipleEntries.m3u");

      var playlist = _converter.DeserializeFromFile(fileContent);

      Assert.IsNotNull(playlist);
      Assert.IsTrue(string.IsNullOrWhiteSpace(playlist.Name));
      Assert.IsTrue(string.IsNullOrWhiteSpace(playlist.MediaType));
      Assert.IsNull(playlist.UserId);
      Assert.IsNotNull(playlist.PlaylistItems);
      Assert.AreEqual(5, playlist.PlaylistItems.Count);

      Assert.That.PlaylistItemsAreEqual(TestData.ImaginationsFromTheOtherSide, playlist.PlaylistItems[0]);
      Assert.That.PlaylistItemsAreEqual(TestData.Inquisition, playlist.PlaylistItems[1]);
      Assert.That.PlaylistItemsAreEqual(TestData.ThisWillNeverEnd, playlist.PlaylistItems[2]);
      Assert.That.PlaylistItemsAreEqual(TestData.TimeWhatIsTime, playlist.PlaylistItems[3]);
      Assert.That.PlaylistItemsAreEqual(TestData.TravelerInTime, playlist.PlaylistItems[4]);
    }

    /// <summary>
    /// The file contains a format where only the file location is included.
    /// This should still result in a playlist item per entry
    /// </summary>
    [TestMethod]
    [DataRow("UTF8")]
    [DataRow("UTF8-BOM")]
    [DataRow("UTF16-BE")]
    [DataRow("UTF16-LE")]
    public void ConvertFromFile_MultipleEntries_Valid_WithoutAdditionalInformation_OK(string format)
    {
      var fileContent = File.ReadAllBytes($"TestPlaylists/{format}/Valid_MultipleEntries_WithoutAdditionalInfo.m3u");

      var playlist = _converter.DeserializeFromFile(fileContent);

      Assert.IsNotNull(playlist);
      Assert.IsTrue(string.IsNullOrWhiteSpace(playlist.Name));
      Assert.IsTrue(string.IsNullOrWhiteSpace(playlist.MediaType));
      Assert.IsNull(playlist.UserId);
      Assert.IsNotNull(playlist.PlaylistItems);
      Assert.AreEqual(5, playlist.PlaylistItems.Count);

      //Without the additional information, only the original location is available
      Assert.That.PlaylistItemsAreEqual(TestData.ImaginationsFromTheOtherSide, playlist.PlaylistItems[0], PlaylistItemComparisonType.OriginalLocation);
      Assert.That.PlaylistItemsAreEqual(TestData.Inquisition, playlist.PlaylistItems[1], PlaylistItemComparisonType.OriginalLocation);
      Assert.That.PlaylistItemsAreEqual(TestData.Majesty, playlist.PlaylistItems[2], PlaylistItemComparisonType.OriginalLocation);
      Assert.That.PlaylistItemsAreEqual(TestData.MrSandman, playlist.PlaylistItems[3], PlaylistItemComparisonType.OriginalLocation);
      Assert.That.PlaylistItemsAreEqual(TestData.PreciousJerusalem, playlist.PlaylistItems[4], PlaylistItemComparisonType.OriginalLocation);
    }

    /// <summary>
    /// The file contains both types of entries, some only with locations, some with the additional info of title and duration.
    /// Both entries should be added as new playlist items in the end result
    /// </summary>
    [TestMethod]
    [DataRow("UTF8")]
    [DataRow("UTF8-BOM")]
    [DataRow("UTF16-BE")]
    [DataRow("UTF16-LE")]
    public void ConvertFromFile_MultipleEntries_Valid_MixedInformationState_OK(string format)
    {
      var fileContent = File.ReadAllBytes($"TestPlaylists/{format}/Valid_MultipleEntries_MixedInformationState.m3u");

      var playlist = _converter.DeserializeFromFile(fileContent);

      Assert.IsNotNull(playlist);
      Assert.IsTrue(string.IsNullOrWhiteSpace(playlist.Name));
      Assert.IsTrue(string.IsNullOrWhiteSpace(playlist.MediaType));
      Assert.IsNull(playlist.UserId);
      Assert.IsNotNull(playlist.PlaylistItems);
      Assert.AreEqual(9, playlist.PlaylistItems.Count);

      //Without the additional information, only the original location is available
      Assert.That.PlaylistItemsAreEqual(TestData.ImaginationsFromTheOtherSide, playlist.PlaylistItems[0]);
      Assert.That.PlaylistItemsAreEqual(TestData.Inquisition, playlist.PlaylistItems[1], PlaylistItemComparisonType.OriginalLocation);
      Assert.That.PlaylistItemsAreEqual(TestData.Majesty, playlist.PlaylistItems[2]);
      Assert.That.PlaylistItemsAreEqual(TestData.MrSandman, playlist.PlaylistItems[3], PlaylistItemComparisonType.OriginalLocation);
      Assert.That.PlaylistItemsAreEqual(TestData.PreciousJerusalem, playlist.PlaylistItems[4]);
      Assert.That.PlaylistItemsAreEqual(TestData.NinthWave, playlist.PlaylistItems[5]);
      Assert.That.PlaylistItemsAreEqual(TestData.ThisWillNeverEnd, playlist.PlaylistItems[6]);
      Assert.That.PlaylistItemsAreEqual(TestData.TimeWhatIsTime, playlist.PlaylistItems[7], PlaylistItemComparisonType.OriginalLocation);
      Assert.That.PlaylistItemsAreEqual(TestData.TravelerInTime, playlist.PlaylistItems[8], PlaylistItemComparisonType.OriginalLocation);
    }

    /// <summary>
    /// The invalid entries should be skipped when reading the file.
    /// If only the additional info is invalid, there can still be an entry only containing the location
    /// </summary>
    [TestMethod]
    [DataRow("UTF8")]
    [DataRow("UTF8-BOM")]
    [DataRow("UTF16-BE")]
    [DataRow("UTF16-LE")]
    public void ConvertFromFile_MultipleEntries_Valid_ContainsInvalidEntries_OK(string format)
    {
      var fileContent = File.ReadAllBytes($"TestPlaylists/{format}/Valid_ContainsInvalidEntries.m3u");

      var playlist = _converter.DeserializeFromFile(fileContent);

      Assert.IsNotNull(playlist);
      Assert.IsTrue(string.IsNullOrWhiteSpace(playlist.Name));
      Assert.IsTrue(string.IsNullOrWhiteSpace(playlist.MediaType));
      Assert.IsNull(playlist.UserId);
      Assert.IsNotNull(playlist.PlaylistItems);
      Assert.AreEqual(9, playlist.PlaylistItems.Count);

      Assert.That.PlaylistItemsAreEqual(TestData.ImaginationsFromTheOtherSide, playlist.PlaylistItems[0], PlaylistItemComparisonType.OriginalLocation);
      Assert.That.PlaylistItemsAreEqual(TestData.Inquisition, playlist.PlaylistItems[1], PlaylistItemComparisonType.OriginalLocation);
      Assert.That.PlaylistItemsAreEqual(TestData.PreciousJerusalem, playlist.PlaylistItems[2]);
      Assert.That.PlaylistItemsAreEqual(TestData.NinthWave, playlist.PlaylistItems[3], PlaylistItemComparisonType.OriginalLocation);
      Assert.That.PlaylistItemsAreEqual(TestData.ThisWillNeverEnd, playlist.PlaylistItems[4], PlaylistItemComparisonType.OriginalLocation);
      Assert.That.PlaylistItemsAreEqual(TestData.TimeWhatIsTime, playlist.PlaylistItems[5], PlaylistItemComparisonType.OriginalLocation | PlaylistItemComparisonType.FullTrackInformation | PlaylistItemComparisonType.Artist | PlaylistItemComparisonType.TrackTitle);
      Assert.That.PlaylistItemsAreEqual(TestData.TravelerInTime, playlist.PlaylistItems[6]);
      //Track information uses a different separator, and can therefore not be parsed into Artist and TrackTitle
      Assert.That.PlaylistItemsAreEqual(TestData.Majesty.SetFullTrackInformation("Blind Guardian _ Majesty"), playlist.PlaylistItems[7], PlaylistItemComparisonType.OriginalLocation | PlaylistItemComparisonType.FullTrackInformation);
      Assert.That.PlaylistItemsAreEqual(TestData.MrSandman, playlist.PlaylistItems[8], PlaylistItemComparisonType.OriginalLocation);
    }

    /// <summary>
    /// The file contains no data, an exception should be thrown
    /// </summary>
    [TestMethod]
    [DataRow("UTF8")]
    [DataRow("UTF8-BOM")]
    [DataRow("UTF16-BE")]
    [DataRow("UTF16-LE")]
    [ExpectedException(typeof(ArgumentException))]
    public void ConvertFromFile_EmptyFile_Invalid_NOK(string format)
    {
      var fileContent = File.ReadAllBytes($"TestPlaylists/{format}/Invalid_Empty.m3u");

      _converter.DeserializeFromFile(fileContent);
    }

    /// <summary>
    /// The file contains some entries, but is missing the header.
    /// An exception should be thrown
    /// </summary>
    [TestMethod]
    [DataRow("UTF8")]
    [DataRow("UTF8-BOM")]
    [DataRow("UTF16-BE")]
    [DataRow("UTF16-LE")]
    [ExpectedException(typeof(InvalidDataException))]
    public void ConvertFromFile_MissingHeader_Invalid_NOK(string format)
    {
      var fileContent = File.ReadAllBytes($"TestPlaylists/{format}/Invalid_MissingHeader.m3u");

      _converter.DeserializeFromFile(fileContent);
    }

    /// <summary>
    /// The file contains a valid header, but no entries.
    /// An exception should be thrown
    /// </summary>
    [TestMethod]
    [DataRow("UTF8")]
    [DataRow("UTF8-BOM")]
    [DataRow("UTF16-BE")]
    [DataRow("UTF16-LE")]
    [ExpectedException(typeof(InvalidDataException))]
    public void ConvertFromFile_NoEntries_Invalid_NOK(string format)
    {
      var fileContent = File.ReadAllBytes($"TestPlaylists/{format}/Invalid_NoEntries.m3u");

      _converter.DeserializeFromFile(fileContent);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow(new byte[0])]
    [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
    public void ConvertFromFile_InvalidInput_NOK(byte[] data)
    {
      _converter.DeserializeFromFile(data);
    }
  }
}
