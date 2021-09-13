using System;
using System.Collections.Generic;
using System.Linq;
using Emby.Plugin.PlaylistConverter.Abstractions;
using Emby.Plugin.PlaylistConverter.Definitions;
using MediaBrowser.Model.Logging;

namespace Emby.Plugin.PlaylistConverter.Conversion
{
  /// <summary>
  ///   Selects the correct playlist converter based on the target format
  /// </summary>
  /// <seealso cref="ITargetFormatConverterSelector" />
  public class TargetFormatConverterSelector: ITargetFormatConverterSelector
  {
    #region Members

    private readonly Dictionary<SupportedPlaylistFormats, IPlaylistConverter> _availablePlaylistConverters;
    private readonly ILogger _logger;

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="TargetFormatConverterSelector" /> class.
    /// </summary>
    /// <param name="availablePlaylistConverters">The available playlist converters.</param>
    /// <param name="logger">The logger</param>
    public TargetFormatConverterSelector(IEnumerable<IPlaylistConverter> availablePlaylistConverters, ILogger logger)
    {
      if (availablePlaylistConverters == null)
      {
        throw new ArgumentNullException(nameof(availablePlaylistConverters));
      }

      _logger = logger;
      _availablePlaylistConverters = availablePlaylistConverters.ToDictionary(converter => converter.TargetPlaylistFormat, converter => converter);
    }

    #endregion

    #region Interfaces

    /// <inheritdoc />
    public IPlaylistConverter GetConverterForPlaylistFormat(string playlistFormat)
    {
      _logger.Debug($"Trying to find converter for playlist format {playlistFormat}...");

      if (!Enum.TryParse<SupportedPlaylistFormats>(playlistFormat, true, out var supportedPlaylistFormat))
      {
        _logger.Warn($"Given playlist format {playlistFormat} is not a supported playlist format.");
        throw new NotSupportedException($"Given playlist format {playlistFormat} is currently not supported");
      }

      if (!_availablePlaylistConverters.TryGetValue(supportedPlaylistFormat, out var converter))
      {
        _logger.Warn($"Playlist format {supportedPlaylistFormat} is not yet implemented");
        throw new NotImplementedException($"Converter for playlist format {supportedPlaylistFormat} is not yet implemented");
      }

      _logger.Debug($"Found a playlist converter for playlist format {supportedPlaylistFormat}");
      return converter;
    }

    #endregion
  }
}