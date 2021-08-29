using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Conversion;
using Emby.Plugin.M3U.Playlists.Definitions;
using Emby.Plugin.M3U.Playlists.Service.ParameterModels;
using Emby.Plugin.M3U.Playlists.UnitTests.TestPlaylists;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Playlists;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Playlists;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Emby.Plugin.M3U.Playlists.UnitTests
{
  [TestClass]
  public class PlaylistBusinessLogicTests
  {
    #region Members

    private PlaylistBusinessLogic _businessLogic;

    private Mock<ITargetFormatConverterSelector> _formatConverterSelectorMock;
    private Mock<ILibraryManager> _libraryManagerMock;
    private Mock<ILogger> _loggerMock;
    private Mock<IPlaylistEnricher> _playlistEnricherMock;
    private Mock<IPlaylistManager> _playlistManagerMock;
    private Mock<IPlaylistConverter> _playlistConverterMock;

    #endregion

    #region Methods

    [TestInitialize]
    public void Initialize()
    {
      _libraryManagerMock = new Mock<ILibraryManager>();
      _loggerMock = new Mock<ILogger>();
      _formatConverterSelectorMock = new Mock<ITargetFormatConverterSelector>();
      _playlistEnricherMock = new Mock<IPlaylistEnricher>();
      _playlistManagerMock = new Mock<IPlaylistManager>();
      _playlistConverterMock = new Mock<IPlaylistConverter>();

      _formatConverterSelectorMock.Setup(mock => mock.GetConverterForPlaylistFormat(It.IsAny<string>())).Returns(_playlistConverterMock.Object);
      _libraryManagerMock.Setup(mock => mock.GetInternalId(It.IsAny<Guid>())).Returns<Guid>(value => value.ToByteArray().Length);
      _libraryManagerMock.Setup(mock => mock.GetInternalId(It.IsAny<string>())).Returns<string>(value => value.Length);

      _businessLogic = new PlaylistBusinessLogic(_formatConverterSelectorMock.Object, _libraryManagerMock.Object, _loggerMock.Object,
                                                 _playlistEnricherMock.Object, _playlistManagerMock.Object);
    }

    [TestMethod]
    public void ToCreationRequest_OK()
    {
      var ids = new List<Guid>
      {
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid()
      };
      var playlist = PlaylistTestHelper.CreateDefaultPlaylist();
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.AddInternalId(ids[0].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.AddInternalId(ids[1].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.Majesty.AddInternalId(ids[2].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.NinthWave.AddInternalId(ids[3].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.PreciousJerusalem.AddInternalId(ids[4].ToString()).SetFound());
      var creationRequest = _businessLogic.ToCreationRequest(playlist);
      Assert.That.PlaylistCreateResultsAreEqual(creationRequest, expectedItemIds: ids);
    }

    [TestMethod]
    public void ToCreationRequest_ContainsNotFoundEntries_OK()
    {
      var ids = new List<Guid>
      {
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid()
      };
      var playlist = PlaylistTestHelper.CreateDefaultPlaylist(mediaType: MediaType.Video, name: PlaylistTestHelper.PLAYLIST_NAME_2, userId: PlaylistTestHelper.USER_ID_2);
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.AddInternalId(ids[0].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.SetFound(false));
      playlist.PlaylistItems.Add(TestData.Majesty.AddInternalId(ids[1].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.NinthWave.SetFound(false));
      playlist.PlaylistItems.Add(TestData.PreciousJerusalem.AddInternalId(ids[2].ToString()).SetFound());
      var creationRequest = _businessLogic.ToCreationRequest(playlist);

      Assert.That.PlaylistCreateResultsAreEqual(creationRequest, MediaType.Video, PlaylistTestHelper.PLAYLIST_NAME_2, PlaylistTestHelper.USER_ID_2,
                                                ids);
    }

    [TestMethod]
    public void ToCreationRequest_FoundEntriesWithMissingId_OK()
    {
      var ids = new List<Guid>
      {
        Guid.NewGuid(),
        Guid.NewGuid()
      };
      var playlist = PlaylistTestHelper.CreateDefaultPlaylist(name: PlaylistTestHelper.PLAYLIST_NAME_2);
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.AddInternalId(ids[0].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.Majesty.SetFound(false));
      playlist.PlaylistItems.Add(TestData.NinthWave.SetFound());
      playlist.PlaylistItems.Add(TestData.PreciousJerusalem.AddInternalId(ids[1].ToString()).SetFound());
      var creationRequest = _businessLogic.ToCreationRequest(playlist);
      Assert.That.PlaylistCreateResultsAreEqual(creationRequest, expectedName: PlaylistTestHelper.PLAYLIST_NAME_2, expectedItemIds: ids);
    }

    [TestMethod]
    public void ToCreationRequest_NameIsMissing_Autogenerate_OK()
    {
      var ids = new List<Guid>
      {
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid()
      };
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist()
                                       .AddMediaType()
                                       .AddUserId();
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.AddInternalId(ids[0].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.AddInternalId(ids[1].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.Majesty.AddInternalId(ids[2].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.NinthWave.AddInternalId(ids[3].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.PreciousJerusalem.AddInternalId(ids[4].ToString()).SetFound());
      var creationRequest = _businessLogic.ToCreationRequest(playlist);
      Assert.That.PlaylistCreateResultsAreEqual(creationRequest, expectAutogeneratedName: true, expectedItemIds: ids);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ToCreationRequest_MediaTypeMissing_NOK()
    {
      var ids = new List<Guid>
      {
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid()
      };
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist()
                                       .AddName()
                                       .AddUserId();
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide.AddInternalId(ids[0].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.Inquisition.AddInternalId(ids[1].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.Majesty.AddInternalId(ids[2].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.NinthWave.AddInternalId(ids[3].ToString()).SetFound());
      playlist.PlaylistItems.Add(TestData.PreciousJerusalem.AddInternalId(ids[4].ToString()).SetFound());
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

      _playlistConverterMock.Verify(mock => mock.DeserializeFromFile(It.IsAny<byte[]>()), Times.Once);
      //There are no playlist items
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<Guid>()), Times.Never);
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