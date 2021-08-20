using Emby.Plugin.M3U.Playlists.Models;

namespace Emby.Plugin.M3U.Playlists.Abstractions
{
  /// <summary>
  ///   Interface for a model that offers an internal validation
  /// </summary>
  public interface IValidatingModel
  {
    #region Methods

    /// <summary>
    ///   Validates this instance.
    /// </summary>
    /// <returns></returns>
    ValidationResult Validate();

    #endregion
  }
}