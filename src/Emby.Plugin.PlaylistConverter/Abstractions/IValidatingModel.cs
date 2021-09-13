using Emby.Plugin.PlaylistConverter.Models;

namespace Emby.Plugin.PlaylistConverter.Abstractions
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