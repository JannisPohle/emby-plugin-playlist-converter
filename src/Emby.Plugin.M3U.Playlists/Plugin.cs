using System;
using System.Collections.Generic;
using Emby.Plugin.M3U.Playlists.Configuration;
using Emby.Plugin.M3U.Playlists.Definitions;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
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
    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="Plugin" /> class.
    /// </summary>
    /// <param name="applicationPaths">The application paths.</param>
    /// <param name="xmlSerializer">The XML serializer.</param>
    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer): base(applicationPaths, xmlSerializer)
    { }

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