using System;
using System.Collections.Generic;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Conversion;
using Emby.Plugin.M3U.Playlists.Definitions;
using MediaBrowser.Model.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Emby.Plugin.M3U.Playlists.UnitTests
{
  [TestClass]
  public class TargetFormatConverterSelectorTests
  {
    #region Members

    private Mock<IPlaylistConverter> _availablePlaylistConverter;
    private List<IPlaylistConverter> _availablePlaylistConverters;
    private TargetFormatConverterSelector _converterSelector;
    private Mock<ILogger> _loggerMock;

    #endregion

    #region Methods

    [TestInitialize]
    public void Initialize()
    {
      _loggerMock = new Mock<ILogger>();
      _availablePlaylistConverter = new Mock<IPlaylistConverter>();
      _availablePlaylistConverter.Setup(mock => mock.TargetPlaylistFormat).Returns(SupportedPlaylistFormats.M3U);
      _availablePlaylistConverters = new List<IPlaylistConverter> { _availablePlaylistConverter.Object };
      _converterSelector = new TargetFormatConverterSelector(_availablePlaylistConverters, _loggerMock.Object);
    }

    [TestMethod]
    [DataRow("M3U")]
    [DataRow("m3u")]
    public void GetConverter_OK(string format)
    {
      var converter = _converterSelector.GetConverterForPlaylistFormat(format);
      Assert.IsNotNull(converter);
      Assert.AreSame(_availablePlaylistConverter.Object, converter);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("UnsupportedFormat")]
    [ExpectedException(typeof(NotSupportedException))]
    public void GetConverter_FormatIsNotSupported_NOK(string format)
    {
      _converterSelector.GetConverterForPlaylistFormat(format);
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void GetConverter_FormatIsNotImplemented_NOK()
    {
      _converterSelector = new TargetFormatConverterSelector(new IPlaylistConverter[0], _loggerMock.Object);
      _converterSelector.GetConverterForPlaylistFormat(SupportedPlaylistFormats.M3U.ToString());
    }

    #endregion
  }
}