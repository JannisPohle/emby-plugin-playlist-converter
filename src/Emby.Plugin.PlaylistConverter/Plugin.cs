using System;
using System.Collections.Generic;
using Emby.Plugin.PlaylistConverter.Abstractions;
using Emby.Plugin.PlaylistConverter.Configuration;
using Emby.Plugin.PlaylistConverter.Conversion;
using Emby.Plugin.PlaylistConverter.Definitions;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Playlists;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Emby.Plugin.PlaylistConverter
{
  /// <summary>
  ///   The main plugin class
  /// </summary>
  /// <seealso cref="PluginConfiguration" />
  public class Plugin: BasePlugin<PluginConfiguration>, IHasWebPages
  {
    #region Members

    private readonly ILogger _logger;
    private readonly ILibraryManager _libraryManager;
    private readonly IPlaylistManager _playlistManager;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the playlist business logic.
    /// </summary>
    /// <value>
    /// The playlist business logic.
    /// </value>
    public static IPlaylistBusinessLogic PlaylistBusinessLogic { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="Plugin" /> class.
    /// </summary>
    /// <param name="applicationPaths">The application paths.</param>
    /// <param name="xmlSerializer">The XML serializer.</param>
    /// <param name="logManager">The logManager</param>
    /// <param name="libraryManager">The library manager</param>
    /// <param name="playlistManager">The playlist manager</param>
    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, ILogManager logManager, ILibraryManager libraryManager, IPlaylistManager playlistManager): base(applicationPaths, xmlSerializer)
    {
      _logger = logManager.GetLogger(Name);
      _libraryManager = libraryManager;
      _playlistManager = playlistManager;
      Initialize();
    }

    #endregion

    #region Methods

    private void Initialize()
    {
      var playlistEnricher = new PlaylistEnricher(_logger, _libraryManager);

      var availableTargetFormatConverters = new List<IPlaylistConverter>
      {
        new M3UPlaylistConverter(_logger)
      };
      var targetFormatConverterSelector = new TargetFormatConverterSelector(availableTargetFormatConverters, _logger);

      PlaylistBusinessLogic = new PlaylistBusinessLogic(targetFormatConverterSelector, _logger, playlistEnricher, _playlistManager);
    }

    #endregion

    #region Interfaces

    #region Implementation of IHasWebPages

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
      var importPage = new PluginPageInfo()
      {
        Name = "import",
        DisplayName = "Import Playlist",
        EnableInMainMenu = true,
        EmbeddedResourcePath = $"{GetType().Namespace}.Pages.import.html"
      };
      return new[] { importPage };
    }

    #endregion

    #endregion

    #region Overrides of BasePlugin

    /// <summary>
    ///   Gets the name.
    /// </summary>
    /// <value>
    ///   The name.
    /// </value>
    public sealed override string Name => Constants.PLUGIN_NAME;

    /// <summary>
    ///   Gets the identifier.
    /// </summary>
    /// <value>
    ///   The identifier.
    /// </value>
    public sealed override Guid Id => Constants.PluginId;

    #endregion
  }
}