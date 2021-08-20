using System;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Definitions;

namespace Emby.Plugin.M3U.Playlists.Conversion
{
  /// <summary>
  ///   Selects the correct playlist converter based on the target format
  /// </summary>
  /// <seealso cref="ITargetFormatConverterSelector" />
  public class TargetFormatConverterSelector: ITargetFormatConverterSelector
  {
    #region Interfaces

    /// <inheritdoc />
    public IPlaylistConverter GetConverterForPlaylistFormat(string playlistFormat)
    {
      if (!Enum.TryParse<SupportedPlaylistFormats>(playlistFormat, out var supportedPlaylistFormat))
      {
        throw new NotSupportedException($"Given playlist format {playlistFormat} is currently not supported");
      }

      switch (supportedPlaylistFormat)
      {
        case SupportedPlaylistFormats.M3U:
          //TODO use static instance from Plugin.cs
          return new M3UPlaylistConverter();
        default:
          throw new NotImplementedException($"Converter for playlist format {supportedPlaylistFormat} is not yet implemented");
      }
    }

    #endregion
  }
}