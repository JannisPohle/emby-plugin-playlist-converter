using System;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Definitions;
using Emby.Plugin.M3U.Playlists.Models;
using MediaBrowser.Model.Logging;

namespace Emby.Plugin.M3U.Playlists.Conversion
{
  /// <summary>
  ///   Converter for the m3u playlist format
  /// </summary>
  public class M3UPlaylistConverter: IPlaylistConverter
  {
    #region Members

    private readonly ILogger _logger;

    #endregion

    #region Properties

    /// <inheritdoc />
    public SupportedPlaylistFormats TargetPlaylistFormat => SupportedPlaylistFormats.M3U;

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="M3UPlaylistConverter" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public M3UPlaylistConverter(ILogger logger)
    {
      _logger = logger;
    }

    #endregion

    #region Interfaces

    /// <inheritdoc />
    public Playlist DeserializeFromFile(byte[] rawFileContent)
    {
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public byte[] SerializeToFile(Playlist playlist)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}