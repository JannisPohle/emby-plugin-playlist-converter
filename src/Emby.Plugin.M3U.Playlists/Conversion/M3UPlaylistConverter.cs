using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    #region Static

    private const string TAG_HEADER = "#EXTM3U";
    private const string TAG_INFO_LINE = "#EXTINF";
    private const string TAG_START = "#";
    private const char TAG_SEPARATOR = ':';
    private const char TAG_INFO_SEPARATOR = ',';
    private const char TAG_TRACK_INFO_SEPARATOR = '-';

    #endregion

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

    #region Methods

    private List<PlaylistItem> Parse(IEnumerable<string> lines)
    {
      _logger.Debug("Starting to parse file as M3U playlist...");
      var playlistItems = new List<PlaylistItem>();
      var firstLine = lines.FirstOrDefault();

      if (firstLine == null)
      {
        _logger.Warn("File contains no data, can not create a new playlist");

        throw new ArgumentException("File data is empty");
      }

      if (!firstLine.Trim().Equals(TAG_HEADER, StringComparison.OrdinalIgnoreCase))
      {
        _logger.Warn("File is missing the M3U header, can not create a new playlist");

        throw new InvalidDataException("Not a valid M3U file. Missing header.");
      }

      PlaylistItem currentItem = null;

      foreach (var line in lines.Skip(1).Select(line => line.Trim()))
      {
        if (line.StartsWith(TAG_START))
        {
          if (ParseInfoLine(line, out var newPlaylistItem))
          {
            currentItem = newPlaylistItem;
          }
        }
        else
        {
          currentItem = ParseLocation(line, currentItem);

          if (currentItem == null)
          {
            _logger.Debug($"Line was not recognized as a valid playlist item, skipped line: {line}");

            continue;
          }

          _logger.Debug($"Parsed a new playlist item: {currentItem}");
          playlistItems.Add(currentItem);
          currentItem = null;
        }
      }

      _logger.Debug($"Finished parsing file as M3U playlist, found {playlistItems.Count} items in the playlist.");

      return playlistItems;
    }

    private PlaylistItem ParseLocation(string line, PlaylistItem currentItem)
    {
      _logger.Debug($"Trying to parse line as file location for item {currentItem?.ToString() ?? "<null>"}: '{line}'");

      if (string.IsNullOrWhiteSpace(line))
      {
        _logger.Warn("File location is empty, skipping entry");

        return null;
      }

      if (currentItem == null)
      {
        currentItem = new PlaylistItem();
      }

      currentItem.OriginalLocation = line;
      _logger.Debug($"Line was parsed as file location: {currentItem}");

      return currentItem;
    }

    private bool ParseInfoLine(string line, out PlaylistItem playlistItem)
    {
      _logger.Debug($"Trying to parse line as info line: '{line}'");
      playlistItem = null;
      var infoItems = line.Split(TAG_SEPARATOR);

      if (!infoItems.FirstOrDefault()?.Equals(TAG_INFO_LINE, StringComparison.OrdinalIgnoreCase) ?? true)
      {
        _logger.Debug($"Commented line does not start with {TAG_INFO_LINE}, skipping parsing of line ('{line}')");

        return false;
      }

      if (infoItems.Length < 2)
      {
        _logger.Warn($"Info line malformed (Does not contain two parts, split by a semicolon): '{line}'");

        return false;
      }

      var infoTags = infoItems[1].Split(TAG_INFO_SEPARATOR);

      if (infoTags.Length < 2)
      {
        _logger.Warn($"Info tag is malformed (Does not contain two parts, split by a comma): '{infoItems[1]}'");

        return false;
      }

      playlistItem = new PlaylistItem
      {
        FullTrackInformation = infoTags[1].Trim()
      };
      var trackInformation = playlistItem.FullTrackInformation.Split(new[] { TAG_TRACK_INFO_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

      if (trackInformation.Length == 2)
      {
        playlistItem.Artist = trackInformation[0].Trim();
        playlistItem.TrackTitle = trackInformation[1].Trim();
      }
      else
      {
        _logger.Debug("Could not parse full track information into artist and track title, split by '-' did not create 2 entries");
      }

      if (!int.TryParse(infoTags[0], out var duration) || duration < 0)
      {
        _logger.Warn($"Info tag is malformed, duration is not valid: {infoTags[0]}");
        _logger.Debug($"Created playlist item from info line: {playlistItem}");

        return true;
      }

      playlistItem.Duration = TimeSpan.FromSeconds(duration);
      _logger.Debug($"Created playlist item from info line: {playlistItem}");

      return true;
    }

    private string GetFileContent(byte[] rawFileContent)
    {
      var (encoding, strippedFileContent) = GetFileEncoding(rawFileContent);
      var fileContent = encoding.GetString(strippedFileContent);

      if (string.IsNullOrWhiteSpace(fileContent))
      {
        _logger.Warn("File does not contain any data, skip parsing the file");

        throw new ArgumentException("File data is empty", nameof(rawFileContent));
      }

      return fileContent;
    }

    /// <summary>
    /// Determines the file encoding and removes the byte order mark from the raw file content.
    /// </summary>
    /// <param name="rawFileContent">Content of the raw file.</param>
    /// <returns>Returns the detected encoding and the file content without the byte order mark</returns>
    private (Encoding Encoding, byte[] StrippedFileContent) GetFileEncoding(byte[] rawFileContent)
    {
      //Other encodings do not have a bom, so we can't check for that
      var supportedEncodings = new List<Encoding> { Encoding.UTF32, Encoding.UTF8, Encoding.Unicode, Encoding.BigEndianUnicode, };
      var encoding = supportedEncodings.FirstOrDefault(e => DoesFileStartWithBom(rawFileContent, e.GetPreamble()));

      if (encoding == null)
      {
        _logger.Warn("Could not detect the file encoding by its byte order mark. Using UTF8 as a fallback.");

        return (Encoding.UTF8, rawFileContent);
      }

      return (encoding, RemoveBomFromFileContent(encoding, rawFileContent));
    }

    private static byte[] RemoveBomFromFileContent(Encoding encoding, IEnumerable<byte> rawFileContent)
    {
      var bom = encoding.GetPreamble();

      return rawFileContent.Skip(bom.Length).ToArray();
    }

    private static bool DoesFileStartWithBom(IReadOnlyList<byte> rawFileContent, IReadOnlyCollection<byte> bom)
    {
      if (rawFileContent.Count < bom.Count)
      {
        return false;
      }

      return !bom.Where((character, index) => character != rawFileContent[index]).Any();
    }

    #endregion

    #region Interfaces

    /// <inheritdoc />
    public Playlist DeserializeFromFile(byte[] rawFileContent)
    {
      if (rawFileContent == null)
      {
        _logger.Warn("File does not contain any data, skip parsing the file");

        throw new ArgumentNullException(nameof(rawFileContent), "File data is empty");
      }

      var fileContent = GetFileContent(rawFileContent);
      var lines = fileContent.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
      _logger.Debug($"Found {lines.Length} lines in the file data");
      var playlistItems = Parse(lines.Select(line => line.Trim()));

      if (!playlistItems.Any())
      {
        _logger.Warn("Found no playlist items in the file data. No playlist is created.");

        throw new InvalidDataException("File data contained no playlist entries");
      }

      var playlist = new Playlist
      {
        PlaylistItems = playlistItems
      };
      _logger.Info($"Finished parsing the file data, created a new playlist with {playlistItems.Count} entries.");

      return playlist;
    }

    /// <inheritdoc />
    public byte[] SerializeToFile(Playlist playlist)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}