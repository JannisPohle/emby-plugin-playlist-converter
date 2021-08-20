using System;

namespace Emby.Plugin.M3U.Playlists.Definitions
{
  /// <summary>
  ///   Constants for the plugin
  /// </summary>
  public static class Constants
  {
    #region Static

    /// <summary>
    ///   The plugin name
    /// </summary>
    public const string PLUGIN_NAME = "M3U Playlists";

    #endregion

    #region Properties

    /// <summary>
    ///   The plugin identifier
    /// </summary>
    public static Guid PluginId => new Guid("E5162304-66CF-40F6-A64F-D77C43C1E7B7");

    #endregion
  }
}