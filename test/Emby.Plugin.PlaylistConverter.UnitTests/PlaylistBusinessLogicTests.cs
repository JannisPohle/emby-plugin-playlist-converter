using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emby.Plugin.PlaylistConverter.Abstractions;
using Emby.Plugin.PlaylistConverter.Conversion;
using Emby.Plugin.PlaylistConverter.Definitions;
using Emby.Plugin.PlaylistConverter.Service.ParameterModels;
using Emby.Plugin.PlaylistConverter.UnitTests.TestPlaylists;
using MediaBrowser.Controller.Playlists;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Playlists;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Emby.Plugin.PlaylistConverter.UnitTests
{
  [TestClass]
  public class PlaylistBusinessLogicTests
  {
    #region Members

    private PlaylistBusinessLogic _businessLogic;

    private Mock<ITargetFormatConverterSelector> _formatConverterSelectorMock;
    private Mock<ILogger> _loggerMock;
    private Mock<IPlaylistEnricher> _playlistEnricherMock;
    private Mock<IPlaylistManager> _playlistManagerMock;
    private Mock<IPlaylistConverter> _playlistConverterMock;

    #endregion

    #region Methods

    [TestInitialize]
    public void Initialize()
    {
      _loggerMock = new Mock<ILogger>();
      _formatConverterSelectorMock = new Mock<ITargetFormatConverterSelector>();
      _playlistEnricherMock = new Mock<IPlaylistEnricher>();
      _playlistManagerMock = new Mock<IPlaylistManager>();
      _playlistConverterMock = new Mock<IPlaylistConverter>();

      _formatConverterSelectorMock.Setup(mock => mock.GetConverterForPlaylistFormat(It.IsAny<string>())).Returns(_playlistConverterMock.Object);

      _businessLogic = new PlaylistBusinessLogic(_formatConverterSelectorMock.Object, _loggerMock.Object,
                                                 _playlistEnricherMock.Object, _playlistManagerMock.Object);
    }

    [TestMethod]
    public void ToCreationRequest_OK()
    {
      var ids = new List<long>
      {
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode()
      };
      var playlist = PlaylistTestHelper.CreateDefaultPlaylist();
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.AddInternalId(ids[0]).SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.AddInternalId(ids[1]).SetFound());
      playlist.PlaylistItems.Add(TestData.Majesty.AddInternalId(ids[2]).SetFound());
      playlist.PlaylistItems.Add(TestData.NinthWave.AddInternalId(ids[3]).SetFound());
      playlist.PlaylistItems.Add(TestData.PreciousJerusalem.AddInternalId(ids[4]).SetFound());
      var creationRequest = _businessLogic.ToCreationRequest(playlist);
      Assert.That.PlaylistCreateResultsAreEqual(creationRequest, expectedItemIds: ids);
    }

    [TestMethod]
    public void ToCreationRequest_DuplicateIds_OK()
    {
      var ids = new List<long>
      {
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode()
      };
      var playlist = PlaylistTestHelper.CreateDefaultPlaylist();
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.AddInternalId(ids[0]).SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.AddInternalId(ids[1]).SetFound());
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.AddInternalId(ids[0]).SetFound());
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.AddInternalId(ids[0]).SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.AddInternalId(ids[1]).SetFound());
      var creationRequest = _businessLogic.ToCreationRequest(playlist);
      Assert.That.PlaylistCreateResultsAreEqual(creationRequest, expectedItemIds: ids);
    }

    [TestMethod]
    public void ToCreationRequest_ContainsNotFoundEntries_OK()
    {
      var ids = new List<long>
      {
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode()
      };
      var playlist = PlaylistTestHelper.CreateDefaultPlaylist(mediaType: MediaType.Video, name: PlaylistTestHelper.PLAYLIST_NAME_2, userId: PlaylistTestHelper.USER_ID_2);
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.AddInternalId(ids[0]).SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.SetFound(false));
      playlist.PlaylistItems.Add(TestData.Majesty.AddInternalId(ids[1]).SetFound());
      playlist.PlaylistItems.Add(TestData.NinthWave.SetFound(false));
      playlist.PlaylistItems.Add(TestData.PreciousJerusalem.AddInternalId(ids[2]).SetFound());
      var creationRequest = _businessLogic.ToCreationRequest(playlist);

      Assert.That.PlaylistCreateResultsAreEqual(creationRequest, MediaType.Video, PlaylistTestHelper.PLAYLIST_NAME_2, PlaylistTestHelper.USER_ID_2,
                                                ids);
    }

    [TestMethod]
    public void ToCreationRequest_FoundEntriesWithMissingId_OK()
    {
      var ids = new List<long>
      {
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode()
      };
      var playlist = PlaylistTestHelper.CreateDefaultPlaylist(name: PlaylistTestHelper.PLAYLIST_NAME_2);
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.AddInternalId(ids[0]).SetFound());
      playlist.PlaylistItems.Add(TestData.Majesty.SetFound(false));
      playlist.PlaylistItems.Add(TestData.NinthWave.SetFound());
      playlist.PlaylistItems.Add(TestData.PreciousJerusalem.AddInternalId(ids[1]).SetFound());
      var creationRequest = _businessLogic.ToCreationRequest(playlist);
      Assert.That.PlaylistCreateResultsAreEqual(creationRequest, expectedName: PlaylistTestHelper.PLAYLIST_NAME_2, expectedItemIds: ids);
    }

    [TestMethod]
    public void ToCreationRequest_NameIsMissing_Autogenerate_OK()
    {
      var ids = new List<long>
      {
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode()
      };
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist()
                                       .AddMediaType()
                                       .AddUserId();
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.AddInternalId(ids[0]).SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.AddInternalId(ids[1]).SetFound());
      playlist.PlaylistItems.Add(TestData.Majesty.AddInternalId(ids[2]).SetFound());
      playlist.PlaylistItems.Add(TestData.NinthWave.AddInternalId(ids[3]).SetFound());
      playlist.PlaylistItems.Add(TestData.PreciousJerusalem.AddInternalId(ids[4]).SetFound());
      var creationRequest = _businessLogic.ToCreationRequest(playlist);
      Assert.That.PlaylistCreateResultsAreEqual(creationRequest, expectAutogeneratedName: true, expectedItemIds: ids);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ToCreationRequest_MediaTypeMissing_NOK()
    {
      var ids = new List<long>
      {
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode(),
        Guid.NewGuid().GetHashCode()
      };
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist()
                                       .AddName()
                                       .AddUserId();
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.AddInternalId(ids[0]).SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.AddInternalId(ids[1]).SetFound());
      playlist.PlaylistItems.Add(TestData.Majesty.AddInternalId(ids[2]).SetFound());
      playlist.PlaylistItems.Add(TestData.NinthWave.AddInternalId(ids[3]).SetFound());
      playlist.PlaylistItems.Add(TestData.PreciousJerusalem.AddInternalId(ids[4]).SetFound());
      _businessLogic.ToCreationRequest(playlist);
    }

    [TestMethod]
    public async Task ImportPlaylist_OK()
    {
      _playlistConverterMock.Setup(mock => mock.DeserializeFromFile(It.IsAny<byte[]>())).Returns(PlaylistTestHelper.CreateDefaultPlaylist());

      _playlistManagerMock.Setup(mock => mock.CreatePlaylist(It.IsAny<PlaylistCreationRequest>()))
                          .ReturnsAsync(new PlaylistCreationResult { Id = Guid.NewGuid().ToString(), Name = PlaylistTestHelper.PLAYLIST_NAME_1 });

      var request = PlaylistTestHelper.CreateImportPlaylist(playlistData: new byte[] { 128, 255 });

      var result = await _businessLogic.ImportPlaylist(request);

      Assert.IsTrue(result.Success, result.Message);
      Assert.AreEqual(PlaylistTestHelper.PLAYLIST_NAME_1, result.Name);
      Assert.IsFalse(string.IsNullOrWhiteSpace(result.PlaylistId));
      Assert.AreEqual(0, result.PlaylistItemsTotal);
      Assert.AreEqual(0, result.PlaylistItemsFound);
      Assert.AreEqual(0, result.PlaylistItemsNotFound);

      _playlistConverterMock.Verify(mock => mock.DeserializeFromFile(It.IsAny<byte[]>()), Times.Once);
      _playlistEnricherMock.Verify(mock => mock.EnrichPlaylistInformation(It.IsAny<Models.Playlist>(), It.IsAny<ImportPlaylist>()), Times.Once);
      _playlistManagerMock.Verify(mock => mock.CreatePlaylist(It.IsAny<PlaylistCreationRequest>()), Times.Once);
      _formatConverterSelectorMock.Verify(mock => mock.GetConverterForPlaylistFormat(SupportedPlaylistFormats.M3U.ToString()), Times.Once);
      _formatConverterSelectorMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ImportPlaylist_WithItems_OK()
    {
      var playlistResult = PlaylistTestHelper.CreateDefaultPlaylist()
                                             .AddPlaylistItem(TestData.ImaginationsFromTheOtherSide.SetFound().AddInternalId())
                                             .AddPlaylistItem(TestData.NinthWave.SetFound(false))
                                             .AddPlaylistItem(TestData.Inquisition.SetFound())
                                             .AddPlaylistItem(TestData.MrSandman.SetFound().AddInternalId());
      _playlistConverterMock.Setup(mock => mock.DeserializeFromFile(It.IsAny<byte[]>())).Returns(playlistResult);

      _playlistManagerMock.Setup(mock => mock.CreatePlaylist(It.IsAny<PlaylistCreationRequest>()))
                          .ReturnsAsync(new PlaylistCreationResult { Id = Guid.NewGuid().ToString(), Name = PlaylistTestHelper.PLAYLIST_NAME_2 });

      var request = PlaylistTestHelper.CreateImportPlaylist(playlistData: new byte[] { 128, 255 });

      var result = await _businessLogic.ImportPlaylist(request);

      Assert.IsTrue(result.Success, result.Message);
      Assert.AreEqual(PlaylistTestHelper.PLAYLIST_NAME_2, result.Name);
      Assert.IsFalse(string.IsNullOrWhiteSpace(result.PlaylistId));
      Assert.AreEqual(4, result.PlaylistItemsTotal);
      Assert.AreEqual(2, result.PlaylistItemsFound);
      //Inquisition has Found set to true, but is missing the internal id --> Should be counted as not found
      Assert.AreEqual(2, result.PlaylistItemsNotFound);

      _playlistConverterMock.Verify(mock => mock.DeserializeFromFile(It.IsAny<byte[]>()), Times.Once);
      _playlistEnricherMock.Verify(mock => mock.EnrichPlaylistInformation(It.IsAny<Models.Playlist>(), It.IsAny<ImportPlaylist>()), Times.Once);
      _playlistManagerMock.Verify(mock => mock.CreatePlaylist(It.IsAny<PlaylistCreationRequest>()), Times.Once);
      _formatConverterSelectorMock.Verify(mock => mock.GetConverterForPlaylistFormat(SupportedPlaylistFormats.M3U.ToString()), Times.Once);
      _formatConverterSelectorMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ImportPlaylist_RequestValidationFailed_OK()
    {
      //Playlist data is null
      var request = PlaylistTestHelper.CreateImportPlaylist();

      var result = await _businessLogic.ImportPlaylist(request);

      Assert.IsFalse(result.Success);
      Assert.IsFalse(string.IsNullOrWhiteSpace(result.Message));
      Assert.IsNull(result.Name);
      Assert.IsNull(result.PlaylistId);
    }

    #endregion
  }
}