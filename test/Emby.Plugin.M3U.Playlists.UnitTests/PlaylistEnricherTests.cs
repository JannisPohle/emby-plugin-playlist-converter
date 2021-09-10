using System;
using System.Linq;
using Emby.Plugin.M3U.Playlists.Conversion;
using Emby.Plugin.M3U.Playlists.UnitTests.TestPlaylists;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Querying;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Emby.Plugin.M3U.Playlists.UnitTests
{
  [TestClass]
  public class PlaylistEnricherTests
  {
    #region Members

    private Mock<ILibraryManager> _libraryManagerMock;
    private Mock<ILogger> _loggerMock;
    private PlaylistEnricher _playlistEnricher;

    #endregion

    #region Methods

    [TestInitialize]
    public void Initialize()
    {
      _libraryManagerMock = new Mock<ILibraryManager>();
      _loggerMock = new Mock<ILogger>();
      _libraryManagerMock.Setup(mock => mock.GetInternalId(It.IsAny<Guid>())).Returns<Guid>(value => value.ToString().GetHashCode());
      _libraryManagerMock.Setup(mock => mock.GetInternalId(It.IsAny<string>())).Returns<string>(value => value.GetHashCode());

      _libraryManagerMock.Setup(mock => mock.GetArtists(It.IsAny<InternalItemsQuery>()))
                         .Returns(new QueryResult<Tuple<BaseItem, ItemCounts>> { Items = new Tuple<BaseItem, ItemCounts>[0], TotalRecordCount = 0 });
      _playlistEnricher = new PlaylistEnricher(_loggerMock.Object, _libraryManagerMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EnrichPlaylistInformation_PlaylistIsNull_NOK()
    {
      var importPlaylist = PlaylistTestHelper.CreateImportPlaylist();
      _playlistEnricher.EnrichPlaylistInformation(null, importPlaylist);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EnrichPlaylistInformation_ImportPlaylistIsNull_NOK()
    {
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist();
      _playlistEnricher.EnrichPlaylistInformation(playlist, null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EnrichPlaylistInformation_PlaylistItemsNull_NOK()
    {
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist();
      playlist.PlaylistItems = null;
      var importPlaylist = PlaylistTestHelper.CreateImportPlaylist();
      _playlistEnricher.EnrichPlaylistInformation(playlist, importPlaylist);
    }

    [TestMethod]
    public void EnrichPlaylistInformation_NoPlaylistItems_OK()
    {
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist();
      var importPlaylist = PlaylistTestHelper.CreateImportPlaylist();
      _playlistEnricher.EnrichPlaylistInformation(playlist, importPlaylist);
      Assert.That.PlaylistsAreEqual(playlist);
      _libraryManagerMock.Verify(mock => mock.QueryItems(It.IsAny<InternalItemsQuery>()), Times.Never);
    }

    [TestMethod]
    public void EnrichPlaylistInformation_PlaylistItemFound_OK()
    {
      var result = PlaylistTestHelper.CreateQueryResult(PlaylistTestHelper.BASE_ITEM_ID_1);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.Name == TestData.ThisWillNeverEnd.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds == null)))
        .Returns(result);
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist();
      playlist.PlaylistItems.Add(TestData.ThisWillNeverEnd);
      var importPlaylist = PlaylistTestHelper.CreateImportPlaylist(PlaylistTestHelper.USER_ID_2);
      _playlistEnricher.EnrichPlaylistInformation(playlist, importPlaylist);
      Assert.That.PlaylistsAreEqual(playlist, expectedUserId: PlaylistTestHelper.USER_ID_2, expectedPlaylistItemCount: 1, expectedPlaylistItemsFoundCount: 1);
      var playlistItem = playlist.PlaylistItems.Single();
      Assert.IsTrue(playlistItem.Found);
      Assert.AreEqual(PlaylistTestHelper.BASE_ITEM_ID_1.GetHashCode(), playlistItem.InternalId);
      _libraryManagerMock.Verify(mock => mock.QueryItems(It.IsAny<InternalItemsQuery>()), Times.Once);
      //GetInternalId is called once for the playlist item and once for the user id
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<Guid>()), Times.Once);
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<string>()), Times.Once);
      //GetArtists is called once for every playlist item
      _libraryManagerMock.Verify(mock => mock.GetArtists(It.IsAny<InternalItemsQuery>()), Times.Once);
      _libraryManagerMock.VerifyNoOtherCalls();
    }


    [TestMethod]
    public void EnrichPlaylistInformation_MultipleItemsFound_TakesFirst_OK()
    {
      var result = PlaylistTestHelper.CreateQueryResult(PlaylistTestHelper.BASE_ITEM_ID_3, PlaylistTestHelper.BASE_ITEM_ID_1,
                                                        PlaylistTestHelper.BASE_ITEM_ID_2);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.Name == TestData.Inquisition.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds == null)))
        .Returns(result);
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist();
      playlist.PlaylistItems.Add(TestData.Inquisition);
      var importPlaylist = PlaylistTestHelper.CreateImportPlaylist(playlistName: PlaylistTestHelper.PLAYLIST_NAME_2);
      _playlistEnricher.EnrichPlaylistInformation(playlist, importPlaylist);
      Assert.That.PlaylistsAreEqual(playlist, PlaylistTestHelper.PLAYLIST_NAME_2, expectedPlaylistItemCount: 1, expectedPlaylistItemsFoundCount: 1);
      var playlistItem = playlist.PlaylistItems.Single();
      Assert.IsTrue(playlistItem.Found);
      Assert.AreEqual(PlaylistTestHelper.BASE_ITEM_ID_3.GetHashCode(), playlistItem.InternalId);
      _libraryManagerMock.Verify(mock => mock.QueryItems(It.IsAny<InternalItemsQuery>()), Times.Once);
      //GetInternalId is called once for the playlist item and once for the user id
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<Guid>()), Times.Once);
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<string>()), Times.Once);
      //GetArtists is called once for every playlist item
      _libraryManagerMock.Verify(mock => mock.GetArtists(It.IsAny<InternalItemsQuery>()), Times.Once);
      _libraryManagerMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void EnrichPlaylistInformation_NoItemFoundByName_OK()
    {
      var emptyResult = PlaylistTestHelper.CreateQueryResult();
      var result = PlaylistTestHelper.CreateQueryResult(PlaylistTestHelper.BASE_ITEM_ID_4);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.Name == TestData.ImaginationsFromTheOtherSide.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds == null)))
        .Returns(emptyResult);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.SearchTerm == TestData.ImaginationsFromTheOtherSide.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds == null)))
        .Returns(result);
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist();
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide);
      var importPlaylist = PlaylistTestHelper.CreateImportPlaylist();
      _playlistEnricher.EnrichPlaylistInformation(playlist, importPlaylist);
      Assert.That.PlaylistsAreEqual(playlist, expectedPlaylistItemCount: 1, expectedPlaylistItemsFoundCount: 1);
      var playlistItem = playlist.PlaylistItems.Single();
      Assert.IsTrue(playlistItem.Found);
      Assert.AreEqual(PlaylistTestHelper.BASE_ITEM_ID_4.GetHashCode(), playlistItem.InternalId);
      _libraryManagerMock.Verify(mock => mock.QueryItems(It.IsAny<InternalItemsQuery>()), Times.Exactly(2));
      //GetInternalId is called once for the playlist item and once for the user id
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<Guid>()), Times.Once);
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<string>()), Times.Once);
      //GetArtists is called once for every playlist item
      _libraryManagerMock.Verify(mock => mock.GetArtists(It.IsAny<InternalItemsQuery>()), Times.Once);
      _libraryManagerMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void EnrichPlaylistInformation_NoItemFound_OK()
    {
      var emptyResult = PlaylistTestHelper.CreateQueryResult();

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.Name == TestData.TimeWhatIsTime.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Video
                                                                          && query.ArtistIds == null)))
        .Returns(emptyResult);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.SearchTerm == TestData.TimeWhatIsTime.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Video
                                                                          && query.ArtistIds == null)))
        .Returns(emptyResult);
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist();
      playlist.PlaylistItems.Add(TestData.TimeWhatIsTime);
      var importPlaylist = PlaylistTestHelper.CreateImportPlaylist(mediaType: MediaType.Video);
      _playlistEnricher.EnrichPlaylistInformation(playlist, importPlaylist);
      Assert.That.PlaylistsAreEqual(playlist, expectedMediaType: MediaType.Video, expectedPlaylistItemCount: 1, expectedPlaylistItemsFoundCount: 0);
      var playlistItem = playlist.PlaylistItems.Single();
      Assert.IsFalse(playlistItem.Found);
      Assert.IsNull(playlistItem.InternalId);
      _libraryManagerMock.Verify(mock => mock.QueryItems(It.IsAny<InternalItemsQuery>()), Times.Exactly(2));
      //GetInternalId is called never for the playlist item and once for the user id
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<Guid>()), Times.Never);
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<string>()), Times.Once);
      //GetArtists is called once for every playlist item
      _libraryManagerMock.Verify(mock => mock.GetArtists(It.IsAny<InternalItemsQuery>()), Times.Once);
      _libraryManagerMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void EnrichPlaylistInformation_PlaylistItemWithoutTitle_OK()
    {
      var result = PlaylistTestHelper.CreateQueryResult(PlaylistTestHelper.BASE_ITEM_ID_2);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.Name == "01 Mr. Sandman"
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds == null)))
        .Returns(result);
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist();
      //MrSandman does not contain a title
      playlist.PlaylistItems.Add(TestData.MrSandman);
      var importPlaylist = PlaylistTestHelper.CreateImportPlaylist();
      _playlistEnricher.EnrichPlaylistInformation(playlist, importPlaylist);
      Assert.That.PlaylistsAreEqual(playlist, expectedPlaylistItemCount: 1, expectedPlaylistItemsFoundCount: 1);
      var playlistItem = playlist.PlaylistItems.Single();
      Assert.IsTrue(playlistItem.Found);
      Assert.AreEqual(PlaylistTestHelper.BASE_ITEM_ID_2.GetHashCode(), playlistItem.InternalId);
      _libraryManagerMock.Verify(mock => mock.QueryItems(It.IsAny<InternalItemsQuery>()), Times.Exactly(1));
      //GetInternalId is called once for the playlist item and once for the user id
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<Guid>()), Times.Once);
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<string>()), Times.Once);
      //GetArtists is never called, because the artist is not set in the playlist item
      _libraryManagerMock.Verify(mock => mock.GetArtists(It.IsAny<InternalItemsQuery>()), Times.Never);
      _libraryManagerMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void EnrichPlaylistInformation_MultiplePlaylistItems_OK()
    {
      var mrSandmanResult = PlaylistTestHelper.CreateQueryResult(PlaylistTestHelper.BASE_ITEM_ID_2);
      var travelerInTimeResult = PlaylistTestHelper.CreateQueryResult(PlaylistTestHelper.BASE_ITEM_ID_4);
      var ninthWaveResult = PlaylistTestHelper.CreateQueryResult(PlaylistTestHelper.BASE_ITEM_ID_1, PlaylistTestHelper.BASE_ITEM_ID_3);
      var emptyResult = PlaylistTestHelper.CreateQueryResult();

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.Name == "01 Mr. Sandman"
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds == null)))
        .Returns(mrSandmanResult);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.Name == TestData.NinthWave.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds == null)))
        .Returns(emptyResult);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.SearchTerm == TestData.NinthWave.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds == null)))
        .Returns(ninthWaveResult);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.Name == TestData.TravelerInTime.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds == null)))
        .Returns(travelerInTimeResult);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.Name == TestData.ImaginationsFromTheOtherSide.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds == null)))
        .Returns(emptyResult);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.SearchTerm == TestData.ImaginationsFromTheOtherSide.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds == null)))
        .Returns(emptyResult);
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist();
      //MrSandman does not contain a title
      playlist.PlaylistItems.Add(TestData.MrSandman);
      playlist.PlaylistItems.Add(TestData.NinthWave);
      playlist.PlaylistItems.Add(TestData.TravelerInTime);
      playlist.PlaylistItems.Add(TestData.ImaginationsFromTheOtherSide);
      var importPlaylist = PlaylistTestHelper.CreateImportPlaylist();
      _playlistEnricher.EnrichPlaylistInformation(playlist, importPlaylist);
      Assert.That.PlaylistsAreEqual(playlist, expectedPlaylistItemCount: 4, expectedPlaylistItemsFoundCount: 3);
      var playlistItem = playlist.PlaylistItems.First(item => item.OriginalLocation == TestData.MrSandman.OriginalLocation);
      Assert.IsTrue(playlistItem.Found);
      Assert.AreEqual(PlaylistTestHelper.BASE_ITEM_ID_2.GetHashCode(), playlistItem.InternalId);
      playlistItem = playlist.PlaylistItems.First(item => item.OriginalLocation == TestData.NinthWave.OriginalLocation);
      Assert.IsTrue(playlistItem.Found);
      Assert.AreEqual(PlaylistTestHelper.BASE_ITEM_ID_1.GetHashCode(), playlistItem.InternalId);
      playlistItem = playlist.PlaylistItems.First(item => item.OriginalLocation == TestData.TravelerInTime.OriginalLocation);
      Assert.IsTrue(playlistItem.Found);
      Assert.AreEqual(PlaylistTestHelper.BASE_ITEM_ID_4.GetHashCode(), playlistItem.InternalId);
      playlistItem = playlist.PlaylistItems.First(item => item.OriginalLocation == TestData.ImaginationsFromTheOtherSide.OriginalLocation);
      Assert.IsFalse(playlistItem.Found);
      Assert.IsNull(playlistItem.InternalId);
      _libraryManagerMock.Verify(mock => mock.QueryItems(It.IsAny<InternalItemsQuery>()), Times.Exactly(6));
      //GetInternalId is called 3 times for the found playlist items and once for the user id
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<Guid>()), Times.Exactly(3));
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<string>()), Times.Once);
      //GetArtists is called once for every playlist item, but not for the playlist item without the artist set (Mr. Sandman)
      _libraryManagerMock.Verify(mock => mock.GetArtists(It.IsAny<InternalItemsQuery>()), Times.Exactly(3));
      _libraryManagerMock.VerifyNoOtherCalls();
    }
    
    [TestMethod]
    public void EnrichPlaylistInformation_ArtistsFound_OK()
    {
      var artists = new QueryResult<Tuple<BaseItem, ItemCounts>>()
      {
        Items = new[]
        {
          new Tuple<BaseItem, ItemCounts>(new MusicArtist() { Id = Guid.NewGuid() }, new ItemCounts()),
          new Tuple<BaseItem, ItemCounts>(new MusicArtist() { Id = Guid.NewGuid() }, new ItemCounts()),
        },
        TotalRecordCount = 2
      };
      _libraryManagerMock.Setup(mock => mock.GetArtists(It.IsAny<InternalItemsQuery>()))
                         .Returns(artists);

      var result = PlaylistTestHelper.CreateQueryResult(PlaylistTestHelper.BASE_ITEM_ID_1);

      _libraryManagerMock
        .Setup(mock => mock.QueryItems(It.Is<InternalItemsQuery>(query => query.Name == TestData.ThisWillNeverEnd.TrackTitle
                                                                          && query.MediaTypes.Length == 1
                                                                          && query.MediaTypes.First() == MediaType.Audio
                                                                          && query.ArtistIds.Length == 2)))
        .Returns(result);
      var playlist = PlaylistTestHelper.CreateEmptyPlaylist();
      playlist.PlaylistItems.Add(TestData.ThisWillNeverEnd);
      var importPlaylist = PlaylistTestHelper.CreateImportPlaylist(PlaylistTestHelper.USER_ID_2);
      _playlistEnricher.EnrichPlaylistInformation(playlist, importPlaylist);
      Assert.That.PlaylistsAreEqual(playlist, expectedUserId: PlaylistTestHelper.USER_ID_2, expectedPlaylistItemCount: 1, expectedPlaylistItemsFoundCount: 1);
      var playlistItem = playlist.PlaylistItems.Single();
      Assert.IsTrue(playlistItem.Found);
      Assert.AreEqual(PlaylistTestHelper.BASE_ITEM_ID_1.GetHashCode(), playlistItem.InternalId);
      _libraryManagerMock.Verify(mock => mock.QueryItems(It.IsAny<InternalItemsQuery>()), Times.Once);
      //GetInternalId is called once for the playlist item, twice for the artists and once for the user id
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<Guid>()), Times.Exactly(3));
      _libraryManagerMock.Verify(mock => mock.GetInternalId(It.IsAny<string>()), Times.Once);
      //GetArtists is called once for every playlist item
      _libraryManagerMock.Verify(mock => mock.GetArtists(It.IsAny<InternalItemsQuery>()), Times.Once);
      _libraryManagerMock.VerifyNoOtherCalls();
    }
    #endregion
  }
}