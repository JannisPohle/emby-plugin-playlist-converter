using System;
using System.Linq;
using Emby.Plugin.M3U.Playlists.Definitions;
using Emby.Plugin.M3U.Playlists.Models;
using Emby.Plugin.M3U.Playlists.Service.ParameterModels;
using MediaBrowser.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emby.Plugin.M3U.Playlists.UnitTests
{
  [TestClass]
  public class ValidationTests
  {
    #region Methods

    [TestMethod]
    public void ExportPlaylist_Valid_OK()
    {
      var exportPlaylist = new ExportPlaylist
      {
        UserId = Guid.NewGuid(),
        Id = Guid.NewGuid()
      };
      var validationResult = exportPlaylist.Validate();
      Assert.IsTrue(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(0, validationResult.ValidationMessages.Count);
    }

    [TestMethod]
    public void ExportPlaylist_MissingUserId_Valid_OK()
    {
      var exportPlaylist = new ExportPlaylist
      {
        UserId = Guid.Empty,
        Id = Guid.NewGuid()
      };
      var validationResult = exportPlaylist.Validate();
      Assert.IsTrue(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(1, validationResult.ValidationMessages.Count);
      var validationMessage = validationResult.ValidationMessages.Single();
      Assert.AreEqual(ValidationResultItem.Severity.Warning, validationMessage.ValidationSeverity);
      Assert.AreEqual(nameof(ExportPlaylist.UserId), validationMessage.PropertyName);
      Assert.IsFalse(string.IsNullOrWhiteSpace(validationMessage.Message));
    }

    [TestMethod]
    public void ExportPlaylist_MissingId_Invalid_OK()
    {
      var exportPlaylist = new ExportPlaylist
      {
        Id = Guid.Empty,
        UserId = Guid.NewGuid()
      };
      var validationResult = exportPlaylist.Validate();
      Assert.IsFalse(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(1, validationResult.ValidationMessages.Count);
      var validationMessage = validationResult.ValidationMessages.Single();
      Assert.AreEqual(ValidationResultItem.Severity.Error, validationMessage.ValidationSeverity);
      Assert.AreEqual(nameof(ExportPlaylist.Id), validationMessage.PropertyName);
      Assert.IsFalse(string.IsNullOrWhiteSpace(validationMessage.Message));
    }

    /// <summary>
    /// Verifies that the overall result is invalid, if there is one error and one warning
    /// </summary>
    [TestMethod]
    public void ExportPlaylist_MissingIdAndUserId_Invalid_OK()
    {
      var exportPlaylist = new ExportPlaylist
      {
        Id = Guid.Empty,
        UserId = Guid.Empty
      };
      var validationResult = exportPlaylist.Validate();
      Assert.IsFalse(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(2, validationResult.ValidationMessages.Count);
    }

    [TestMethod]
    public void ImportPlaylist_Valid_OK()
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = Guid.NewGuid(),
        MediaType = MediaType.Audio,
        PlaylistName = PlaylistTestHelper.PLAYLIST_NAME_1,
        PlaylistFormat = SupportedPlaylistFormats.M3U.ToString(),
        PlaylistData = new byte[] { 124, 255 },
      };
      var validationResult = importPlaylist.Validate();
      Assert.IsTrue(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(0, validationResult.ValidationMessages.Count);
    }

    [TestMethod]
    public void ImportPlaylist_MissingUserId_Valid_OK()
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = null,
        MediaType = MediaType.Audio,
        PlaylistName = PlaylistTestHelper.PLAYLIST_NAME_1,
        PlaylistFormat = SupportedPlaylistFormats.M3U.ToString(),
        PlaylistData = new byte[] { 124, 255 },
      };
      var validationResult = importPlaylist.Validate();
      Assert.IsTrue(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(1, validationResult.ValidationMessages.Count);
      var validationMessage = validationResult.ValidationMessages.Single();
      Assert.AreEqual(ValidationResultItem.Severity.Warning, validationMessage.ValidationSeverity);
      Assert.AreEqual(nameof(ImportPlaylist.UserId), validationMessage.PropertyName);
      Assert.IsFalse(string.IsNullOrWhiteSpace(validationMessage.Message));
    }

    [TestMethod]
    public void ImportPlaylist_MissingPlaylistName_Valid_OK()
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = Guid.NewGuid(),
        MediaType = MediaType.Audio,
        PlaylistFormat = SupportedPlaylistFormats.M3U.ToString(),
        PlaylistData = new byte[] { 124, 255 },
      };
      var validationResult = importPlaylist.Validate();
      Assert.IsTrue(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(1, validationResult.ValidationMessages.Count);
      var validationMessage = validationResult.ValidationMessages.Single();
      Assert.AreEqual(ValidationResultItem.Severity.Warning, validationMessage.ValidationSeverity);
      Assert.AreEqual(nameof(ImportPlaylist.PlaylistName), validationMessage.PropertyName);
      Assert.IsFalse(string.IsNullOrWhiteSpace(validationMessage.Message));
    }

    [TestMethod]
    public void ImportPlaylist_EmptyPlaylistData_Invalid_OK()
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = Guid.NewGuid(),
        MediaType = MediaType.Audio,
        PlaylistName = PlaylistTestHelper.PLAYLIST_NAME_1,
        PlaylistFormat = SupportedPlaylistFormats.M3U.ToString(),
        PlaylistData = new byte[] { },
      };

      var validationResult = importPlaylist.Validate();
      Assert.IsFalse(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(1, validationResult.ValidationMessages.Count);
      var validationMessage = validationResult.ValidationMessages.Single();
      Assert.AreEqual(ValidationResultItem.Severity.Error, validationMessage.ValidationSeverity);
      Assert.AreEqual(nameof(ImportPlaylist.PlaylistData), validationMessage.PropertyName);
      Assert.IsFalse(string.IsNullOrWhiteSpace(validationMessage.Message));
    }

    [TestMethod]
    public void ImportPlaylist_MissingPlaylistData_Invalid_OK()
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = Guid.NewGuid(),
        MediaType = MediaType.Audio,
        PlaylistName = PlaylistTestHelper.PLAYLIST_NAME_1,
        PlaylistFormat = SupportedPlaylistFormats.M3U.ToString(),
      };

      var validationResult = importPlaylist.Validate();
      Assert.IsFalse(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(1, validationResult.ValidationMessages.Count);
      var validationMessage = validationResult.ValidationMessages.Single();
      Assert.AreEqual(ValidationResultItem.Severity.Error, validationMessage.ValidationSeverity);
      Assert.AreEqual(nameof(ImportPlaylist.PlaylistData), validationMessage.PropertyName);
      Assert.IsFalse(string.IsNullOrWhiteSpace(validationMessage.Message));
    }

    [TestMethod]
    public void ImportPlaylist_MissingPlaylistFormat_Invalid_OK()
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = Guid.NewGuid(),
        MediaType = MediaType.Audio,
        PlaylistName = PlaylistTestHelper.PLAYLIST_NAME_1,
        PlaylistData = new byte[] { 124, 255 },
      };

      var validationResult = importPlaylist.Validate();
      Assert.IsFalse(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(1, validationResult.ValidationMessages.Count);
      var validationMessage = validationResult.ValidationMessages.Single();
      Assert.AreEqual(ValidationResultItem.Severity.Error, validationMessage.ValidationSeverity);
      Assert.AreEqual(nameof(ImportPlaylist.PlaylistFormat), validationMessage.PropertyName);
      Assert.IsFalse(string.IsNullOrWhiteSpace(validationMessage.Message));
    }

    [TestMethod]
    public void ImportPlaylist_InvalidPlaylistFormat_Invalid_OK()
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = Guid.NewGuid(),
        MediaType = MediaType.Audio,
        PlaylistName = PlaylistTestHelper.PLAYLIST_NAME_1,
        PlaylistFormat = "UnsupportedPlaylistFormat",
        PlaylistData = new byte[] { 124, 255 },
      };

      var validationResult = importPlaylist.Validate();
      Assert.IsFalse(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(1, validationResult.ValidationMessages.Count);
      var validationMessage = validationResult.ValidationMessages.Single();
      Assert.AreEqual(ValidationResultItem.Severity.Error, validationMessage.ValidationSeverity);
      Assert.AreEqual(nameof(ImportPlaylist.PlaylistFormat), validationMessage.PropertyName);
      Assert.IsFalse(string.IsNullOrWhiteSpace(validationMessage.Message));
    }

    [TestMethod]
    public void ImportPlaylist_MissingMediaType_Invalid_OK()
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = Guid.NewGuid(),
        PlaylistName = PlaylistTestHelper.PLAYLIST_NAME_1,
        PlaylistFormat = SupportedPlaylistFormats.M3U.ToString(),
        PlaylistData = new byte[] { 124, 255 },
      };

      var validationResult = importPlaylist.Validate();
      Assert.IsFalse(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(1, validationResult.ValidationMessages.Count);
      var validationMessage = validationResult.ValidationMessages.Single();
      Assert.AreEqual(ValidationResultItem.Severity.Error, validationMessage.ValidationSeverity);
      Assert.AreEqual(nameof(ImportPlaylist.MediaType), validationMessage.PropertyName);
      Assert.IsFalse(string.IsNullOrWhiteSpace(validationMessage.Message));
    }

    [TestMethod]
    public void ImportPlaylist_InvalidMediaType_Invalid_OK()
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = Guid.NewGuid(),
        MediaType = "InvalidMediaType",
        PlaylistName = PlaylistTestHelper.PLAYLIST_NAME_1,
        PlaylistFormat = SupportedPlaylistFormats.M3U.ToString(),
        PlaylistData = new byte[] { 124, 255 },
      };

      var validationResult = importPlaylist.Validate();
      Assert.IsFalse(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(1, validationResult.ValidationMessages.Count);
      var validationMessage = validationResult.ValidationMessages.Single();
      Assert.AreEqual(ValidationResultItem.Severity.Error, validationMessage.ValidationSeverity);
      Assert.AreEqual(nameof(ImportPlaylist.MediaType), validationMessage.PropertyName);
      Assert.IsFalse(string.IsNullOrWhiteSpace(validationMessage.Message));
    }

    /// <summary>
    /// Verifies that the overall result is invalid, if there is one error and one warning
    /// </summary>
    [TestMethod]
    public void ImportPlaylist_MissingPlaylistNameAndMediaType_Invalid_OK()
    {
      var importPlaylist = new ImportPlaylist
      {
        UserId = Guid.NewGuid(),
        PlaylistFormat = SupportedPlaylistFormats.M3U.ToString(),
        PlaylistData = new byte[] { 124, 255 },
      };

      var validationResult = importPlaylist.Validate();
      Assert.IsFalse(validationResult.Success);
      Assert.IsNotNull(validationResult.ValidationMessages);
      Assert.AreEqual(2, validationResult.ValidationMessages.Count);
    }

    [TestMethod]
    public void ValidationResult_ToString_SeverityFilter()
    {
      var validationResult = new ValidationResult();
      validationResult.ValidationMessages.Add(new ValidationResultItem("Foo", "Prop1", ValidationResultItem.Severity.Info));
      validationResult.ValidationMessages.Add(new ValidationResultItem("Foo", "Prop2", ValidationResultItem.Severity.Warning));
      validationResult.ValidationMessages.Add(new ValidationResultItem("Foo", "Prop3", ValidationResultItem.Severity.Error));

      validationResult.Success = false;

      var message = validationResult.ToString(ValidationResultItem.Severity.Info);
      Assert.IsTrue(message.Contains("Prop1"));
      Assert.IsTrue(message.Contains("Prop2"));
      Assert.IsTrue(message.Contains("Prop3"));

      message = validationResult.ToString(ValidationResultItem.Severity.Warning);
      Assert.IsFalse(message.Contains("Prop1"));
      Assert.IsTrue(message.Contains("Prop2"));
      Assert.IsTrue(message.Contains("Prop3"));

      message = validationResult.ToString(ValidationResultItem.Severity.Error);
      Assert.IsFalse(message.Contains("Prop1"));
      Assert.IsFalse(message.Contains("Prop2"));
      Assert.IsTrue(message.Contains("Prop3"));
    }

    #endregion
  }
}