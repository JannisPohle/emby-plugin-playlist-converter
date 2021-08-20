using System;
using System.Collections.Generic;
using Emby.Plugin.M3U.Playlists.Abstractions;
using Emby.Plugin.M3U.Playlists.Configuration;
using Emby.Plugin.M3U.Playlists.Conversion;
using Emby.Plugin.M3U.Playlists.Definitions;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Emby.Plugin.M3U.Playlists
{
  /// <summary>
  ///   The main plugin class
  /// </summary>
  /// <seealso cref="PluginConfiguration" />
  public class Plugin: BasePlugin<PluginConfiguration>, IHasWebPages
  {
    #region Members

    private readonly ILogger _logger;

    #endregion

    #region Properties

    /// <summary>
    ///   Gets the playlist enricher.
    /// </summary>
    /// <value>
    ///   The playlist enricher.
    /// </value>
    public static IPlaylistEnricher PlaylistEnricher { get; private set; }

    /// <summary>
    ///   Gets the target format converter selector.
    /// </summary>
    /// <value>
    ///   The target format converter selector.
    /// </value>
    public static ITargetFormatConverterSelector TargetFormatConverterSelector { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="Plugin" /> class.
    /// </summary>
    /// <param name="applicationPaths">The application paths.</param>
    /// <param name="xmlSerializer">The XML serializer.</param>
    /// <param name="logger">The logger</param>
    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, ILogger logger): base(applicationPaths, xmlSerializer)
    {
      _logger = logger;
      Initialize();
    }

    #endregion

    #region Methods

    private void Initialize()
    {
      PlaylistEnricher = new PlaylistEnricher(_logger);

      var availableTargetFormatConverters = new List<IPlaylistConverter>
      {
        new M3UPlaylistConverter(_logger)
      };
      TargetFormatConverterSelector = new TargetFormatConverterSelector(availableTargetFormatConverters, _logger);
    }

    #endregion

    #region Interfaces

    #region Implementation of IHasWebPages

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
      //TODO
      throw new NotImplementedException();
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
    public override string Name => Constants.PLUGIN_NAME;

    /// <summary>
    ///   Gets the identifier.
    /// </summary>
    /// <value>
    ///   The identifier.
    /// </value>
    public override Guid Id => Constants.PluginId;

    #endregion
  }
}